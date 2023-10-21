using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleCardDrawAndSpread_CardDrag;
using TMPro;
namespace RoundDisplay
{
    public class roundDisplay : MonoBehaviour
    {
        public TextMeshProUGUI roundText; // 用來顯示回合數的UI Text物件
        public int round = 0;
        //CardDrawSystem cardDrawSystem = new CardDrawSystem();

        void Start() 
        {
            roundText = GetComponent<TextMeshProUGUI>();
            if(roundText == null ){
                Debug.Log("roundText is null");
            }
            roundText.text = "Initial";
            //UpdateRoundText();
        }

        // Update is called once per frame
        void Update()
        {
            /*if (round != cardDrawSystem.round2)
            {
                //UpdateRoundText();
                Debug.Log("isupdate2");
            }*/
        }
        public void UpdateRoundText(int round)
        {
            // 從CardDrawSystem獲取round2值
            //round = cardDrawSystem.round2;
            Debug.Log("Round: " + round);
            // 更新UI Text上的文字
            string newText = "Round: " + round;
            roundText.text = newText;
        }    
    }
}