using Card;
using Card.Controller;
using Card.View;
using DataModels.JsonLevelModel;
using Enums;
using Enums.Card;
using UnityEngine;

namespace DataModels.Card
{
    public class ArrowsCardModel : BasicCardModel<ArrowsCardController>
    {
        // TODO DEL
        public ArrowsCardController CardController;   
        public ArrowsCardView CardView;

        public bool IsItAdditionalCardValue = false;

        public new CardTypes CardType { get; set; } = CardTypes.Arrows;

        public  CardSuits CardSuit { get; set; }

        public int CardRank { get; set; }

        public ArrowsCardModel(JsonCard jsonCard, GameObject cardPrefab)
        {
            SetBoardDataToThisModel(jsonCard);
            cardPrefab.name = $"ArrowsCard_ID{CardId}";
            cardPrefab.GetComponent<CardComponents>().Id = CardId;
            CardController = new ArrowsCardController();
            CardView = new ArrowsCardView(this, cardPrefab);
        }
        private void SetBoardDataToThisModel(JsonCard jsonCard)
        {
            CardId = jsonCard.Id;
            CardX = jsonCard.X;
            CardY = jsonCard.Y;
            CardAngle = jsonCard.Angle;
            CardLayer = jsonCard.Layer;
            CardBlocking = jsonCard.Blocking;
            CardBlockers = jsonCard.Blockers;
        }
    }
}