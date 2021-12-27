using System;
using System.Collections.Generic;
using Card;
using Card.Controller;
using Card.View;
using DataModels.JsonLevelModel;
using Enums;
using Enums.Card;
using UnityEngine;
using Utility;

namespace DataModels.Card
{
    public class PlayingCardModel : BasicCardModel<PlayingCardController>
    {
        public readonly PlayingCardView CardView;

        public new CardTypes CardType { get; set; } = CardTypes.Playing;

        public CardSuits CardSuit { get; set; } = CardSuits.None;

        public CardRanks CardRank { get; set; } = CardRanks.None;

        public new List<int> CardBlockers { get; set; }

        public new List<int> CardBlocking { get; set; }

        public PlayingCardModel(JsonCard jsonCard, GameObject cardPrefab)
        {
            SetBoardDataToThisModel(jsonCard);
            cardPrefab.name = $"PlayingCard_ID{CardId}";
            cardPrefab.GetComponent<CardComponents>().Id = CardId;
            CardController = new PlayingCardController();
            CardView = new PlayingCardView(this, cardPrefab);
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
            if (jsonCard.Rank != null && jsonCard.Suit != null)
            {
                CardRank = (CardRanks) jsonCard.Rank;
                CardSuit = (CardSuits) jsonCard.Suit;
                DeckController.Instance.RemoveCardFromFreeCardsPool(new[] {(int) CardRank, (int) CardSuit});
            }
        }
    }
}