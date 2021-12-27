using System;
using DataModels.Card;
using Enums;
using Enums.Card;
using Utility;

namespace Card.Controller
{
    public class PlayingCardController : BasicCardController<PlayingCardModel>
    {
        public override void OnClick(PlayingCardModel cardModel)
        {
            if (cardModel.IsItPivot)
                PlayAsPivotCard(cardModel);
            
            if (cardModel.IsInHand)
            {
                if(IsCardCanBeMovedFromHandToPivot(cardModel))
                {
                    PlayAsHandCard(cardModel);
                }
            }
            
            if (cardModel.CardBlockers.Count == 0)
            {
                PlayAsBoardCard(cardModel);
            }
        }


        private void PlayAsPivotCard(PlayingCardModel cardModel) 
        {
            // if IsCardCanDestroyBarrier (cardModel)
            // PlayCardToBarrier => hide this card
            // remove barier
        }
        private void  PlayAsHandCard(PlayingCardModel cardModel)
        {
            RemoveBlocked(cardModel, cardModel.CardId);
            //cuz after PlayToPivotDataState PivotCard will be different.We need deactivate previous PivotCard collider.
            DeckController.Instance.PivotCard.CardView.DeActivatePivotBoxCollider();
            PlayToPivot(cardModel);
            cardModel.IsInHand = false;
            var cardBlocking = cardModel.CardBlocking;
            if (cardBlocking.Count!=0)
                cardModel.CardView.SetActiveNextCardBoxColliderInHand(cardBlocking[0]);
            else
            {
                DeckController.Instance.isHandIsEmpty = true;
                DeckController.Instance.CheckForEndLevelConditions();
            }
            // TODO INVOKE NEXT TURN ACTIONS for arrows
        }
        private void  PlayAsBoardCard(PlayingCardModel cardModel)
        {
            if (IsCardCanBeMovedFromBoardToPivot(cardModel))
            {
                //cuz after PlayToPivotDataState PivotCard will be different.We need deactivate previous PivotCard collider.
                DeckController.Instance.PivotCard.CardView.DeActivatePivotBoxCollider();
                PlayToPivot(cardModel);
                RemoveBlocking(cardModel);
                if(DeckController.Instance.isHandIsEmpty)
                    DeckController.Instance.CheckForEndLevelConditions();
                // TODO INVOKE NEXT TURN ACTIONS for arrows
            }
        }
        
        
        
        

        public override void OnClickDataState(PlayingCardModel cardModel)
        {
            if(cardModel.IsItPivot)
                return;
            if (cardModel.IsInHand)
                return;
            if (cardModel.CardBlockers.Count == 0)
                return;
        }
        
        
        
        
        
        public override bool IsCardCanBeMovedFromHandToPivot(PlayingCardModel cardModel) {return true;}
        
        public override bool IsCardCanBeMovedFromBoardToPivot(PlayingCardModel cardModel)
        {
            int cardRank = (int) cardModel.CardRank;
            int pivotRank = (int) DeckController.Instance.PivotCard.CardRank;
            
            if (Math.Abs(cardRank-pivotRank) == 1
                || pivotRank == (int) CardRanks.Two && cardRank == (int) CardRanks.Ace
                || cardRank == (int) CardRanks.Two && pivotRank == (int) CardRanks.Ace)
                return true;
            return false;
        }
        
        public override bool IsCardCanDestroyBarrier(PlayingCardModel cardModel) {return false;}
        
        
        
        public override void Open(PlayingCardModel cardModel)
        {
            SetDataToOpenState(cardModel);
            SetViewToOpenState(cardModel);
        }
        public override void SetDataToOpenState(PlayingCardModel cardModel)
        {
            if (cardModel.CardRank != CardRanks.None && cardModel.CardSuit != CardSuits.None)
                // Moved to constructor to prevent situation when Value this card will be taken before Open
                //DeckController.Instance.RemoveCardFromFreeCardsPool(new[] {(int) cardModel.CardRank, (int) cardModel.CardSuit});
                return;

            cardModel.CardRank = GetNewValue(cardModel.CardRank,cardModel.CardSuit, out var cardSuit);
            cardModel.CardSuit = cardSuit;

            cardModel.IsCardOpen = true;
            DeckController.Instance.OpenedCardsId.Add(cardModel.CardId);
        }
        private CardRanks GetNewValue(CardRanks oldRank, CardSuits oldSuit, out CardSuits newSuit)
        {
            var oldValue = new []{(int) oldRank, (int) oldSuit};
            var value = DeckController.Instance.GetNewCardValue(oldValue);
            
            newSuit = (CardSuits) value[1];
            return (CardRanks) value[0];
        }
        public override void SetViewToOpenState(PlayingCardModel cardModel)
        {
            cardModel.CardView.UpdateViewValueAccordingData(cardModel);
            cardModel.CardView.RotateCardToOpenState(cardModel);
        }
        
        
        
        public override void PlayToPivot(PlayingCardModel cardModel)
        {
            // Not sure where it should be
            //DeckController.Instance.PivotCard.CardView.DeActivatePivotBoxCollider();
            PlayToPivotDataState(cardModel);
            PlayToPivotViewState(cardModel);
        }
        public override void PlayToPivotDataState(PlayingCardModel cardModel)
        {
            CopyPivotData(cardModel);
            if (cardModel.IsInHand)
                DeckController.Instance.MoveCardFromHandToPivot(cardModel);
            else
                DeckController.Instance.MoveCardFromBoardToPivot(cardModel);
        }
        private void CopyPivotData(PlayingCardModel cardModel)
        {
            var pivotCard = DeckController.Instance.PivotCard;
            
            cardModel.CardX = pivotCard.CardX;
            cardModel.CardY = pivotCard.CardY;
            cardModel.CardAngle = pivotCard.CardAngle;
            cardModel.CardLayer = pivotCard.CardLayer + 1;
            cardModel.IsItPivot = true;
        }
        public override void PlayToPivotViewState(PlayingCardModel cardModel)
        {
            cardModel.CardView.UpdateViewPositionAngleLayerAccordingData(cardModel);
        }
        
        
        public override void PlayToBarrier(PlayingCardModel cardModel) {}
        
        
        
        /// <summary>
        ///     What this card is Blocking
        /// </summary>
        public override void RemoveBlocking(PlayingCardModel cardModel)
        {
            GenericDictionary listToRemove;
            if (cardModel.IsInHand)
                listToRemove = DeckController.Instance.HandCards;
            else
                listToRemove = DeckController.Instance.BoardCards;
            
            for (int i = 0; i < cardModel.CardBlocking.Count; i++)
            {
                var card = listToRemove.GetClassModel(cardModel.CardBlocking[i]);
                card.CardController.RemoveBlocked(card, cardModel.CardId);
            }
        }
        
        
        
        /// <summary>
        ///     What is BLocked card
        /// </summary>
        public override void RemoveBlocked(PlayingCardModel cardModel, int value)
        {
            RemoveBlockedDataState(cardModel, value);
            RemoveBlockedViewState(cardModel);
        }
        public override void RemoveBlockedDataState(PlayingCardModel cardModel, int value)
        {
            var cardBlockers = cardModel.CardBlockers;
            cardBlockers.Remove(value);
            
            if (cardBlockers.Count == 0)
                cardModel.CardController.SetDataToOpenState(cardModel);
        }
        private void RemoveBlockedViewState(PlayingCardModel cardModel)
        {
            if (cardModel.CardBlockers.Count == 0)
                cardModel.CardController.SetViewToOpenState(cardModel);
        }
    }
}