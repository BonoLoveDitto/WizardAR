using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCardDrawAndSpread_Card{
    [Serializable]
    public class Card
    {
        public int id;
        public string name;
        public int score;
        public int turn;
        public string portion;
        public string skill;
    }
}