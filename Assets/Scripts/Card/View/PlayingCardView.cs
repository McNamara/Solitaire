using System;
using Card.Controller;
using DataModels.Card;
using Enums;
using ResourceContainers;
using UnityEngine;
using Utility;

namespace Card.View
{
    public class PlayingCardView : BasicCardView<PlayingCardModel>
    {
        public PlayingCardModel CardModel;
        public GameObject CardPrefab;
        private const int ViewOpenStateAngelY = 180;
        
        public PlayingCardView(dynamic cardModel, GameObject cardPrefab)
        {
            CardModel = cardModel;
            CardPrefab = cardPrefab;
            UpdateViewPositionAngleLayerAccordingData(cardModel);
        }
        
        public virtual void UpdateViewPositionAngleLayerAccordingData(dynamic cardModel)
        {
            var rectTransform = CardPrefab.GetComponent<RectTransform>();
            
            rectTransform.localPosition = new Vector3(cardModel.CardX, cardModel.CardY, 0);

            var eulerAngles = rectTransform.eulerAngles;
            eulerAngles = new Vector3(eulerAngles.x,eulerAngles.y,cardModel.CardAngle);
            rectTransform.eulerAngles = eulerAngles;

            var cardComponents = CardPrefab.GetComponent<CardComponents>();
            cardComponents.BackSkin.sortingOrder = cardModel.CardLayer;
            cardComponents.FaceValue.sortingOrder = cardModel.CardLayer;
        }
        
        public void UpdateViewValueAccordingData(dynamic cardModel)
        {
            var cardComponents = cardModel.CardView.CardPrefab.GetComponent<CardComponents>();
            int spriteIndex = ((int)cardModel.CardSuit-1) * (Enum.GetValues(typeof(CardRanks)).Length-2) +
                              (int)cardModel.CardRank - 1;
            cardComponents.FaceValue.sprite = ResourcesContainer.Instance.PlayingCardValueSuitSprites[spriteIndex];
        }
        
        public void RotateCardToOpenState(dynamic cardModel)
        {
            cardModel.CardView.CardPrefab.transform.Rotate(Vector3.up,ViewOpenStateAngelY, Space.Self);
            cardModel.CardView.CardPrefab.GetComponent<BoxCollider>().enabled = true;
        }

        public void SetActiveNextCardBoxColliderInHand(int cardId)
        {
            var card = DeckController.Instance.HandCards.GetClassModel(cardId);
            card.CardView.CardPrefab.GetComponent<BoxCollider>().enabled = true;
        }
        public void DeActivatePivotBoxCollider()
        {
            DeckController.Instance.PivotCard.CardView.CardPrefab.GetComponent<BoxCollider>().enabled = false;
        }
    }
}