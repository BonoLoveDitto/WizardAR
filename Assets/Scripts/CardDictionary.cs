using System.Collections;
using System.Collections.Generic;
using SimpleCardDrawAndSpread_CardDrag;
using SimpleCardDrawAndSpread_Card;
using UnityEngine;

namespace SimpleCardDrawAndSpread_CardDictionary{
    public class CardDictionary : MonoBehaviour
    {
        public List<Card> Cards = new List<Card>();
        public Card CardDrawSystemCard = new Card();
        public Dictionary<int, Card> cardDictionary = new Dictionary<int, Card>();
        
        //用來辨認組合的flag
        public bool combination_CthulhuChin = false, combination_SmallDevelWing = false;
        public bool combination2_WitchNose = false, combination2_KindChin = false;
        public bool combination3_Mustache = false, combination3_BigEar = false, combination3_SmallEar = false, combination3_LongNose = false;
        public bool marvelOdd = false, marvelEven = false, frozen = false;
        // Start is called before the first frame update
        public void Init()
        {
            int cardNumber = 0;
            //藥水卡(IDNumber, Name, Score, Turn, Portion, Skill)
            Cards.Add(new Card() { id = cardNumber++, name = "長鼻子", score = 2, turn = 3, portion = "Nose", skill = "" });           
            Cards.Add(new Card() { id = cardNumber++, name = "巫婆鼻", score = 3, turn = 4, portion = "Nose", skill = "" });          
            Cards.Add(new Card() { id = cardNumber++, name = "豬鼻", score = 5, turn = 6, portion = "Nose", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "伏地魔", score = 4, turn = 5, portion = "Nose", skill = "" });           
            Cards.Add(new Card() { id = cardNumber++, name = "厚道", score = 2, turn = 5, portion = "Chin", skill = "" });           
            Cards.Add(new Card() { id = cardNumber++, name = "克蘇魯", score = 6, turn = 3, portion = "Chin", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "屁屁下巴", score = 3, turn = 4, portion = "Chin", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "龍角", score = 10, turn = 10, portion = "Derivative", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "鹿角", score = 3, turn = 4, portion = "Derivative", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "小惡魔翅膀", score = 4, turn = 5, portion = "Derivative", skill = "" });  
            Cards.Add(new Card() { id = cardNumber++, name = "牛角", score = 2, turn = 3, portion = "Derivative", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "精靈", score = 6, turn = 7, portion = "Ear", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "兔子", score = 3, turn = 4, portion = "Ear", skill = "" });           
            Cards.Add(new Card() { id = cardNumber++, name = "大耳朵", score = 2, turn = 3, portion = "Ear", skill = "" });            
            Cards.Add(new Card() { id = cardNumber++, name = "小耳朵", score = 3, turn = 4, portion ="Ear", skill = "" });              
            Cards.Add(new Card() { id = cardNumber++, name = "8字鬍", score = 2, turn = 3, portion = "Beard", skill = "" });            
            Cards.Add(new Card() { id = cardNumber++, name = "希特勒鬍", score = 10, turn = 10, portion = "Beard", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "山羊鬍子", score = 6, turn = 7, portion = "Beard", skill = "" });
            // 咒語卡
            Cards.Add(new Card() { id = cardNumber++, name = "01 快手", score = 0, turn = 0, portion = "咒語", skill = "抽一張牌且下回合可以額外打出一張牌" });
            Cards.Add(new Card() { id = cardNumber++, name = "02 奇數驚奇", score = 0, turn = 0, portion = "咒語", skill = "抽一張牌且下回合可以額外打出一張牌" });
            Cards.Add(new Card() { id = cardNumber++, name = "03 偶數驚奇", score = 0, turn = 0, portion = "咒語", skill = "加速偶數藥水0/-3" });
            Cards.Add(new Card() { id = cardNumber++, name = "04 咒力強化", score = 0, turn = 0, portion = "咒語", skill = "使下一張藥水+2/-3" });
            Cards.Add(new Card() { id = cardNumber++, name = "05 急速凍結", score = 0, turn = 0, portion = "咒語", skill = "使敵人下一張藥水0/+2" });
            Cards.Add(new Card() { id = cardNumber++, name = "06 執法者", score = 0, turn = 0, portion = "咒語", skill = "已觸發的特徵 每有一個種類加1分" });
            Cards.Add(new Card() { id = cardNumber++, name = "07 冷光神諭的祝福", score = 0, turn = 0, portion = "咒語", skill = "雙方玩家抽1張牌" });
            Cards.Add(new Card() { id = cardNumber++, name = "08 親王", score = 0, turn = 0, portion = "咒語", skill = "若你牌堆中沒有分數5以上的牌,賦予你的牌堆中所有特徵+1/-1" });

            // 添加卡信息到字典中
            cardNumber = 0;
            foreach (Card CardInfo in Cards)
            {
                cardDictionary.Add(cardNumber, CardInfo);
                cardNumber++;
            }
            Debug.Log("CardDictionary");
        }
        
        //取得卡片資訊
        public Card GetCardInfoByNumber(int cardNumber)
        {
            Debug.Log("GetCardInforByNumber");
            if (cardDictionary.ContainsKey(cardNumber))
            {
                Debug.Log("cardNumber = " + cardNumber);
                return cardDictionary[cardNumber];
            }
            return null;
        }
        
        //組合加分
        public int CombinationBonus(){
            if(combination_SmallDevelWing && combination_CthulhuChin){
                combination_SmallDevelWing = combination_CthulhuChin = false;
                return 3;
            }
            else if(combination2_WitchNose && combination2_KindChin){
                combination2_WitchNose = combination2_KindChin = false;
                return 3;
            }
            else if(combination3_Mustache && (combination3_SmallEar || combination3_BigEar) && combination3_LongNose){
                combination3_BigEar = combination3_Mustache = combination3_Mustache = combination3_SmallEar = false;
                return 6;
            }
            else{
                return 0;
            }
        }

        //看看生效的卡片有沒有再組合裡面
        public void FlagCombinationId(string name){
            if(name == "克蘇魯"){
                combination_CthulhuChin = true;
            }
            else if(name == "小惡魔翅膀"){
                combination_SmallDevelWing = true;
            }
            else if(name == "厚道"){
                combination2_KindChin = true;
            }
            else if(name == "巫婆鼻"){  
                combination2_WitchNose = true;
            }
            else if(name == "大耳朵"){
                combination3_BigEar = true;
            }
            else if(name == "小耳朵"){
                combination3_SmallEar = true;
            }
            else if(name == "8字鬍"){
                combination3_Mustache = true;
            }
            else if(name == "長鼻子"){
                combination3_LongNose = true;
            }
        }

        //咒語卡flag
        public void spellCard(string name){
            if(name == "02 奇數驚奇"){
                marvelOdd = true;
            }
            else if(name == "03 偶數驚奇"){
                marvelEven = true;
            }
        }

        //奇偶數驚奇
        public int spellEffect(int turn, int round){            
            if(marvelOdd && (round % 2) == 1){
                return turn - 3;
            }
            else if(marvelEven && (round % 2) == 0){
                return turn - 3;
            }
            else if(frozen){
                frozen = false;
                return turn + 2;
            }
            else{
                return turn;
            }
        }
    }
}
