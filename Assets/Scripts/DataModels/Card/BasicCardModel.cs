using System.Collections.Generic;
using Card.View;
using Enums.Card;
using UnityEngine;

namespace DataModels.Card
{
    
    public class BasicCardModel<TBasicCardController>
    {
        public TBasicCardController CardController;

        public int CardId { get; set; }
        
        public float CardX { get; set; }
        public float CardY { get; set; }
        public float CardAngle { get; set; }
        public int CardLayer { get; set; }
        
        // TODO T?
        public CardTypes CardType { get; set; }
        
        public bool IsCardOpen { get; set; }
        public bool IsInHand  { get; set; }
        public bool IsItPivot  { get; set; }
        
        public List<int> CardBlockers;

        public List<int> CardBlocking;
    }
}