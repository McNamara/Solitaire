using DataModels.Card;
using DataModels.JsonLevelModel;
using UnityEngine;
using Utility;

namespace Card.View
{
    public class ArrowsCardView : PlayingCardView
    {
        public ArrowsCardModel CardModel;
        public GameObject CardPrefab;
        public ArrowsCardView(ArrowsCardModel cardModel, GameObject cardPrefab) : base(cardModel, cardPrefab)
        {
            //todo dynamic
            //UpdateViewPositionAngleLayerAccordingData(cardModel);
        }
    }
}