using System.Collections;
using System.Collections.Generic;
using SimpleCardDrawAndSpread_CardDrag;
using UnityEngine;

namespace SimpleCardDrawAndSpread_CardDictionary{
    public class CardDictionary : MonoBehaviour
    {
        public class Card
        {
            public int id;
            public string name;
            public int score;
            public int turn;
            public string portion;
            public string skill;
        }
        public List<Card> Cards = new List<Card>();
        public Card CardDrawSystemCard = new Card();
        public Dictionary<int, Card> cardDictionary = new Dictionary<int, Card>();
        // Start is called before the first frame update
        void Start()
        {
            int cardNumber = 0;
            // 藥水卡(IDNumber, Name, Score, Turn, Portion, Skill)
            Cards.Add(new Card() { id = cardNumber++, name = "長鼻子", score = 2, turn = 3, portion = "Nose", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "巫婆鼻", score = 3, turn = 4, portion = "Nose", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "豬鼻", score = 5, turn = 6, portion = "Nose", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "伏地魔", score = 4, turn = 5, portion = "Nose", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "厚道", score = 2, turn = 3, portion = "Chin", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "克蘇魯", score = 6, turn = 7, portion = "Chin", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "屁屁下巴", score = 3, turn = 4, portion = "Chin", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "顏色", score = 2, turn =  3, portion = "EyeBrow", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "粗眉毛", score = 3, turn = 4, portion = "EyeBrow", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "沒有眉毛", score = 1, turn = 1, portion = "EyeBrow", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "香吉士眉毛", score = 5, turn = 6, portion = "EyeBrow", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "發光", score = 2, turn = 3, portion = "Eyes", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "我推", score = 6, turn = 7, portion = "Eyes", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "病懨懨", score = 1, turn = 1, portion = "Lips", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "流血", score = 3, turn = 4, portion = "Lips", skill = "" });
            Cards.Add(new Card() { id = cardNumber++, name = "口紅", score = 2, turn = 3, portion = "Lips", skill = "" });
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
            Cards.Add(new Card() { id = cardNumber++, name = "01", score = 0, turn = 0, portion = "咒語", skill = "抽一張牌且下回合可以額外打出一張牌" });
            Cards.Add(new Card() { id = cardNumber++, name = "03", score = 0, turn = 0, portion = "咒語", skill = "加速偶數藥水0/-3" });
            Cards.Add(new Card() { id = cardNumber++, name = "04", score = 0, turn = 0, portion = "咒語", skill = "使下一張藥水+2/-3" });
            Cards.Add(new Card() { id = cardNumber++, name = "05", score = 0, turn = 0, portion = "咒語", skill = "使敵人下一張藥水0/+2" });
            Cards.Add(new Card() { id = cardNumber++, name = "06", score = 0, turn = 0, portion = "咒語", skill = "已觸發的特徵 每有一個種類加1分" });
            Cards.Add(new Card() { id = cardNumber++, name = "冷光神諭", score = 0, turn = 0, portion = "咒語", skill = "雙方玩家抽1張牌" });
            Cards.Add(new Card() { id = cardNumber++, name = "親王", score = 0, turn = 0, portion = "咒語", skill = "若你牌堆中沒有分數5以上的牌,賦予你的牌堆中所有特徵+1/-1" });

            // 添加卡信息到字典中
            cardNumber = 0;
            foreach (Card CardInfo in Cards)
            {
                cardDictionary.Add(cardNumber, CardInfo);
                cardNumber++;
            }
        }

        public Card GetCardInfoByNumber(int cardNumber)
        {
            if (cardDictionary.ContainsKey(cardNumber))
            {
                return cardDictionary[cardNumber];
            }
            return null;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}