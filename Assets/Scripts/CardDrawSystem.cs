using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using SimpleCardDrawAndSpread_HandCard;
using SimpleCardDrawAndSpread_CardDictionary;
using SimpleCardDrawAndSpread_RemoveCard;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;

// Hello. Thank you for purchasing Asset. In this set, the pulled cards are moved to the hand part and aligned at regular intervals.
// This script is not the correct answer, so please consider it as one of the implementation scripts. Thank you.

namespace SimpleCardDrawAndSpread_CardDrag
{
    public enum CardDragType
    {
        CardPos,
        MousePos,
    }
    [Serializable]
    public class CardDrawSystem : MonoBehaviourPunCallbacks
    {        
        CardDictionary _CardDictionary;
        //Variable that sets the region where the card is used.
        [Header("Card Use Ground")]
        public Transform CardUseGround;
        public float CardUseDistance;

        [Header("Card Draw")]
        public Transform CardCreatePos;
        public Transform CardHandPos;

        [Space(10)]
        public GameObject CardOb;
        public List<Sprite> CardSprites;
        public float CardDrawDelay;
        public List<GameObject> PlayerHandCardList;

        [Space(10)]
        public int FirstDrawCount;
        public int NomalDrawCount;

        [Header("Card Drag")]
        public CardDragType CardDragType;

        [Header("Card Spread")]
        public float HandCard_CardSpreadRange; //Must be entered as an integer.
        public float HandCard_EachCardSpreadDistance; //Must be entered as an integer.
        public float HandCard_EachCardSpreadLimitCount;

        [HideInInspector] public GameObject HandCard_CardSpreadOb;
        [HideInInspector] public GameObject HandCard_EachCardOb;

        [HideInInspector] public List<Vector3> HandCard_EachCardDistanceList;
        [HideInInspector] public List<float> HandCard_EachCardAngleList;

        [Header("Card Move Speed")]
        public float CardSpeed_Draw;
        public float CardSpeed_HandSpread;

        public bool CardDelete = false;
        public bool CardDelete2 = false;
        //被丟出去的卡片,而且turn不為0
        public List<RemoveCard> RemoveCardList = new List<RemoveCard>();        
        public List<RemoveCard> RemoveCardList2 = new List<RemoveCard>();
        //每回合可以丟出去的卡片數
        public List<int> roundCards = new List<int> {1,1,1,2,1,1,2};
        public List<int> roundCards2 = new List<int> {1,1,1,2,1,1,2};
        [Header("回合")]
        //丟出去卡片等待，
        public int round = 0;                               
        public int round2 = 0;
        public int PlayerID;
        [Header("暫停")]
        public bool roundFinish2;
        public bool roundFinish;
        public bool isPause = false;
        public PhotonView _pv;
        public PhotonView photonView;
        public int totalScore;
        public int totalScore2;
        [Header("timer")]
        public float timer = 0f;
        public float targetTime = 60f;
        public bool isTimeup = false;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("05-Game");
            //The first time you start a game, you draw a card as many as the FirstDrawCount number.
            _CardDictionary = new CardDictionary();
            _CardDictionary.Init();
            InitPlayer();
            photonView = GetComponent<PhotonView>();
            StartCoroutine(PlayerCardDrawManager(FirstDrawCount));
        }

        // Update is called once per frame
        void Update()
        {
            /*if(round != round2){
                Debug.Log("Round error!");
            }*/

            //計算時間
            timer += Time.deltaTime;
            if(roundFinish && roundFinish2) {
                Debug.Log("player" + PlayerID + " time pause over!");
                //Time.timeScale = 1f;
                isPause = false;
                roundFinish = roundFinish2 = false;
                timer = 0;
            }
            else if(timer >= targetTime){
                isTimeup = true;
            }
        }

        private void InitPlayer(){
            if(PhotonNetwork.NickName == "1") {
                Debug.Log("I'm 1");
                PlayerID = 1;
            }
            else if(PhotonNetwork.NickName == "2"){
                Debug.Log("I'm 2");
                PlayerID = 2;
            }
            else {
                Debug.Log("Who are you?");
                SceneManager.LoadScene("05-LobbyLoading");
            }
        //PlayerCardDrawManager(FirstDrawCount);
        }

        public void Button_CardDraw_Manager()
        {
            //Draw cards as many as the NomalDrawCount number by recalling a particular button or function.
            Debug.Log("Button_cardDraw_Manager");
            StartCoroutine(PlayerCardDrawManager(NomalDrawCount));
        }

        public IEnumerator PlayerCardDrawManager(int CardCount)
        {
            while(true){
                if(!isPause)
                {
                    if(PlayerID == 1){
                        round = round + 1;
                        Debug.Log("Round: " + (round));
                    }
                    else{
                        round2 = round2 + 1;
                        Debug.Log("Round2: " + (round2));
                        //Debug.Log("(PlayerCardDrawManager)RoundCards2[" + round2 + "]: " + roundCards2[round2]);
                    }

                    for (int i = 0; i < CardCount; i++)
                    {
                        yield return new WaitForSeconds(CardDrawDelay);
                        //Randomly determine the image for the card.
                        int Rnum_CardSprite = UnityEngine.Random.Range(0, (int)CardSprites.Count);

                        //Create and set card objects.
                        GameObject newOb = Instantiate(CardOb, CardCreatePos.position, Quaternion.identity);
                        newOb.transform.SetParent(CardHandPos, true);
                    

                        //Change the central image of the card that you created using Rnum_CardSprite, which contains arbitrary numbers.
                        HandCardSystem input_HandCardSystem = newOb.GetComponent<HandCardSystem>();
                        input_HandCardSystem.CardIcon_Sprite.sprite = CardSprites[Rnum_CardSprite];
                
                        //add card information
                        _CardDictionary.CardDrawSystemCard = _CardDictionary.GetCardInfoByNumber(Rnum_CardSprite);

                        //CardDictionary Cards = CardDictionary.GetCardInfoByNumber(Rnum_CardSprite);
                        input_HandCardSystem.id = _CardDictionary.CardDrawSystemCard.id;
                        input_HandCardSystem.name = _CardDictionary.CardDrawSystemCard.name;
                        input_HandCardSystem.score = _CardDictionary.CardDrawSystemCard.score;
                        input_HandCardSystem.turn = _CardDictionary.CardDrawSystemCard.turn;
                        input_HandCardSystem.portion = _CardDictionary.CardDrawSystemCard.portion;
                        input_HandCardSystem.skill = _CardDictionary.CardDrawSystemCard.skill;

                        //Save to list for use in card sorting.
                        PlayerHandCardList.Add(newOb);

                        //Draw card layer setting.
                        CardLayerCheckManager();

                        //Set the numerical value to expand the card you hold in your hand first.
                        CardSpreadSettingManager();

                        //Delays card objects so that they do not overlap.
                        yield return new WaitForSeconds(CardDrawDelay);

                    }
                    break;
                }
                else {
                    yield return new WaitForSeconds(CardDrawDelay);
                }
            }
        }

        public void CardLayerCheckManager()
        {
            //First, delete an empty object in the list.
            for (int checkcount = 0; checkcount < PlayerHandCardList.Count;)
            {
                if (PlayerHandCardList[checkcount] == null)
                {
                    Debug.Log("Have empty object");
                    PlayerHandCardList.RemoveAt(checkcount);
                    CardDelete = true;
                    checkcount = 0;
                }
                else
                {
                    checkcount++;
                }
            }

            //Card layer setting.
            for (int i = 0; i < PlayerHandCardList.Count; i++)
            {
                HandCardSystem input_HandCardSystem = PlayerHandCardList[i].GetComponent<HandCardSystem>();

                for (int i2 = 0; i2 < input_HandCardSystem.CardLayers.Length; i2++)
                {
                    input_HandCardSystem.CardLayers[i2].sortingOrder = i;
                    input_HandCardSystem.GetComponent<SortingGroup>().sortingOrder = i; //Avoid possible problems with masks or multiple images.
                }

            }


        }

        public void CardSpreadSettingManager()
        {
            //The way to expand the card in this script is to find the angle using two guide objects in the CardHandPos position.

            //Set to the initial value.
            HandCard_CardSpreadOb.transform.position = new Vector3(CardHandPos.position.x, CardHandPos.position.y, CardHandPos.position.z);
            HandCard_CardSpreadOb.transform.rotation = Quaternion.Euler(0, 0, 0);

            //Check if it's an integer or not.
            if (CardHandPos.position.y >= 0)//integer
            {
                HandCard_CardSpreadOb.transform.position = new Vector3(HandCard_CardSpreadOb.transform.position.x, -(CardHandPos.position.y + HandCard_CardSpreadRange), HandCard_CardSpreadOb.transform.position.z);

            }
            else
            {
                HandCard_CardSpreadOb.transform.position = new Vector3(HandCard_CardSpreadOb.transform.position.x, -(System.Math.Abs(CardHandPos.position.y) + HandCard_CardSpreadRange), HandCard_CardSpreadOb.transform.position.z);

            }

            //Move by numeric value
            HandCard_EachCardOb.transform.localPosition = new Vector3(0, HandCard_CardSpreadRange, 0);



            //Initial setting of numerical values to unfold the card.
            float SpreadPoint_Start = 0;
            float SpreadPoint_End = 0;

            //If the number of cards exceeds a certain number, the value is obtained to set it not to widen left and right.
            if (PlayerHandCardList.Count > HandCard_EachCardSpreadLimitCount)
            {
                float num1 = PlayerHandCardList.Count - HandCard_EachCardSpreadLimitCount; //Finds the number of cards that exceed the set value.
                float num2 = num1 / PlayerHandCardList.Count * 100; //Find out what percentage of the total number of cards you have with num1.
                float num3 = HandCard_EachCardSpreadDistance * (1 - num2 / 100); //Finds how much percentage of the num2 value should be reduced from the existing value at which the card is deployed.

                SpreadPoint_Start = (num3 * (PlayerHandCardList.Count - 1)) / 2;
                SpreadPoint_End = num3;
            }
            else
            {
                //Find the angle that will unfold as many cards as you draw.
                //SpreadPoint_Start is the left end and SpreadPoint_End is the right end angle.
                SpreadPoint_Start = (HandCard_EachCardSpreadDistance * (PlayerHandCardList.Count - 1)) / 2;
                SpreadPoint_End = HandCard_EachCardSpreadDistance;
            }

            //Initializes the position and angle values for each card.
            HandCard_EachCardDistanceList.Clear();
            HandCard_EachCardAngleList.Clear();

            //Use the repeat statement to rotate HandCard_CardSpreadOb to store the x, y values in that position.
            for (int i = 0; i < PlayerHandCardList.Count; i++)
            {
                //This method ensures that the card is not aligned properly due to the decimal value, and that the rotated value is still available for the card rotation value.
                HandCard_CardSpreadOb.transform.rotation = Quaternion.Euler(0, 0, (SpreadPoint_Start - ((SpreadPoint_End) * i)));

                HandCard_EachCardDistanceList.Add(HandCard_EachCardOb.transform.position);
                HandCard_EachCardAngleList.Add(HandCard_CardSpreadOb.transform.eulerAngles.z);
            }

            //

        }

        public void AddRemoveCard(int HandCardNumber){
            //因為Monobehavoiur不能被壓縮，所以加入新的一個class去存要被刪除的卡片，而且只需要知道他的生效回合跟分數
            HandCardSystem inputHandCardSystem = PlayerHandCardList[HandCardNumber].GetComponent<HandCardSystem>();
            RemoveCard rc = new RemoveCard();
            rc.turn = inputHandCardSystem.turn;
            rc.score = inputHandCardSystem.score;
            //第一回合只能丟咒語卡
            if(round == 0 && inputHandCardSystem.portion == "咒語"){      //記得改if(_CardDrawSystem.round == 0 && inputHandCardSystem.portion != "咒語")
                Debug.Log("Player 1: 第一回合只能丟咒語卡");
            }
            else if(round2 == 0 && inputHandCardSystem.portion == "咒語"){   
                Debug.Log("Player 2: 第一回合只能丟咒語卡");
            }
            else { 
                Debug.Log("不是第一回合或者是咒語卡");
                //將準備移除的卡片加入移除卡片的List中 
                if(PlayerID == 1){
                    RemoveCardList.Add(rc);
                    SendData(rc, "rc");
                    //Remove the used cards from the list and re-align them with the layers of the cards in your hand.
                    PlayerHandCardList.RemoveAt(HandCardNumber);
                    roundCards[round] = roundCards[round] - 1;
                    Debug.Log("RoundCards[" + round + "]: " + roundCards[round]);
                }
                else{
                    RemoveCardList2.Add(rc);
                    SendData(rc, "rc");
                    //Remove the used cards from the list and re-align them with the layers of the cards in your hand.    
                    PlayerHandCardList.RemoveAt(HandCardNumber);
                    roundCards2[round2] = roundCards2[round2] - 1;
                    //SendData();
                    Debug.Log("RoundCards2[" + round2 + "]: " + roundCards2[round2]);
                }
                        
                //如果該回合卡片丟完了，將所以丟出去卡片的生效回合(turn)減1，並將回合+1及抽卡，進入下一回合
                if (roundCards[round] <= 0 && PlayerID != 2) {
                    if(!roundFinish2){
                        Debug.Log("Player 1: Time Pause");
                        //Time.timeScale = 0f;
                        isPause = true;
                    }
                    roundFinish = true;
                    SendData(roundFinish, "rf");
                    Debug.Log("Round end!!");
                    CardDelete = true;
                    countTurn();          
                    Button_CardDraw_Manager(); 
                }
                else if (roundCards2[round2] <= 0 && PlayerID != 1) {
                    if(!roundFinish){
                        Debug.Log("Player 2: Time pause");
                        //Time.timeScale = 0f;
                        isPause = true;
                    }
                    roundFinish2 = true;
                    SendData(roundFinish2, "rf");
                    Debug.Log("Round end!!");
                    CardDelete2 = true;
                    countTurn2();         
                    Button_CardDraw_Manager(); 
                }
                //_CardDrawSystem.CardLayerCheckManager();
                //_CardDrawSystem.CardSpreadSettingManager();
                         
                //When the numerical alignment is complete, use automatic movement to move the card in your hand to that position.
                for (int i = 0; i < PlayerHandCardList.Count; i++)
                {
                    PlayerHandCardList[i].GetComponent<HandCardSystem>().HandSpreadTrigger = true;
                }
            }
        }

        //每次打出一張牌，都會將生效回合-1(記得當卡片生效後，不要把卡片刪除，PhotonViewObject的順序會有問題)
        public void countTurn() {
            if (CardDelete) {
                Debug.Log("CardDelete: " + CardDelete);
                for (int i = 0; i < RemoveCardList.Count; i++){
                    RemoveCardList[i].turn = RemoveCardList[i].turn - 1;
                    if(RemoveCardList[i].turn <= 0) {
                        //將卡片內容生效
                    }
                    Debug.Log("id: " + i);
                    Debug.Log("turn: " + RemoveCardList[i].turn);
                }
                CardDelete = false;
                //onDeleteCard();
            }
            else{
                Debug.Log("CardDelete: " + CardDelete);
            }
        }

        public void countTurn2() {
            if (CardDelete2) {
                Debug.Log("CardDelete: " + CardDelete2);
                for (int i = 0; i < RemoveCardList2.Count; i++){
                    RemoveCardList2[i].turn = RemoveCardList2[i].turn - 1;
                    if(RemoveCardList2[i].turn <= 0) {
                        //將卡片內容生效
                    }
                    Debug.Log("id: " + i);
                    Debug.Log("turn: " + RemoveCardList2[i].turn);
                }
                CardDelete2 = false;
                //onDeleteCard();
            }
            else{
                Debug.Log("CardDelete: " + CardDelete2);
            }
        }

        //壓縮檔案
        public static byte[] ObjectToByteArray(object obj) {
            if(obj == null){
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        //解壓縮資料
        public static object ByteArrayObject(byte[] arrBytes){
            using(var memoryStream = new MemoryStream()){
                BinaryFormatter formatter = new BinaryFormatter();
                memoryStream.Write(arrBytes, 0, arrBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var obj = formatter.Deserialize(memoryStream);
                return obj;
            }
        }

        //傳送資料
        private void SendData(object obj, string str)
        {
            var _byteData = ObjectToByteArray(obj);
            Debug.Log("(Sending)");
            if(str == "rc")
            {
                photonView.RPC("ReceiveRemoveCardList", RpcTarget.Others, _byteData);
            }
            else if(str == "rf")
            {
                photonView.RPC("ReceiveRoundFinish", RpcTarget.Others, _byteData);
            }
            Debug.Log("Send Data!");
        }

        [PunRPC] //接收資料後會做的事情
        public void ReceiveRemoveCardList(byte[] data){
            RemoveCard _data = (RemoveCard)ByteArrayObject(data);
            Debug.Log("(ReceiveRemoveCardList)turn: " + _data.turn + ", score: " +  _data.score);
            if(PlayerID == 1){
                RemoveCardList2.Add(_data);
            }
            else if(PlayerID == 2) {
                RemoveCardList2.Add(_data);
            }
        }

        [PunRPC]
        public void ReceiveRoundFinish(byte[] data){
            bool _data = (bool)ByteArrayObject(data);
            if(PlayerID == 1){
                roundFinish2 = _data;
                Debug.Log("(ReceiveRoundFinish)RoundFinish2 = "+ roundFinish2);
            }
            else if(PlayerID == 2){
                roundFinish = _data;
                Debug.Log("(ReceiveRoundFinish)RoundFinish = "+ roundFinish);
            }
        }
    }
}