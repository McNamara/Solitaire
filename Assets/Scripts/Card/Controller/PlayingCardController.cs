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
                PlayFromPivot(cardModel);
            else if (cardModel.IsInHand)
                PlayFromHand(cardModel);
            else
                PlayFromBoard(cardModel);
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

        
        private void PlayFromPivot(PlayingCardModel cardModel) 
        {
            // if IsCardCanDestroyBarrier (cardModel)
            // PlayCardToBarrier => hide this card
            // remove barier
        }
        private void PlayFromPivotDataState(PlayingCardModel cardModel){}
        private void PlayFromPivotViewState(PlayingCardModel cardModel){}

        
        private void PlayFromHand(PlayingCardModel cardModel)
        {
            if(IsCardCanBeMovedFromHandToPivot(cardModel))
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
        }
        private void PlayFromHandDataState(PlayingCardModel cardModel){}
        private void PlayFromHandViewState(PlayingCardModel cardModel){}
        
        
        private void  PlayFromBoard(PlayingCardModel cardModel)
        {
            if (cardModel.CardBlockers.Count == 0)
                if (IsBoardCardMatchToPivot(cardModel))
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
        private void  PlayFromBoardDataState(PlayingCardModel cardModel){}
        private void  PlayFromBoardViewState(PlayingCardModel cardModel){}
        
        
        
        public override bool IsBoardCardMatchToPivot(PlayingCardModel cardModel)
        {
            int cardRank = (int) cardModel.CardRank;
            int pivotRank = (int) DeckController.Instance.PivotCard.CardRank;
            
            if (Math.Abs(cardRank-pivotRank) == 1
                || pivotRank == (int) CardRanks.Two && cardRank == (int) CardRanks.Ace
                || cardRank == (int) CardRanks.Two && pivotRank == (int) CardRanks.Ace)
                return true;
            return false;
        }
        public override bool IsCardCanBeMovedFromHandToPivot(PlayingCardModel cardModel) {return true;}
        public override bool IsCardCanDestroyBarrier(PlayingCardModel cardModel) {return false;}
        
        
        
        public override void Open(PlayingCardModel cardModel)
        {
            OpenDataState(cardModel);
            OpenViewState(cardModel);
        }
        public override void OpenDataState(PlayingCardModel cardModel)
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
        private void OpenViewState(PlayingCardModel cardModel)
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
        private void PlayToPivotViewState(PlayingCardModel cardModel)
        {
            cardModel.CardView.UpdateViewPositionAngleLayerAccordingData(cardModel);
        }
        
        
        public override void PlayToBarrier(PlayingCardModel cardModel) {}
        
        
        
        /// <summary>
        ///     What this card is Blocking
        /// </summary>
        public override void RemoveBlocking(PlayingCardModel cardModel)
        {
            RemoveBlockingDataState(cardModel);
            RemoveBlockingViewState(cardModel);
        }
        public override void RemoveBlockingDataState(PlayingCardModel cardModel)
        {
            GenericDictionary listToRemove;
            if (cardModel.IsInHand)
                listToRemove = DeckController.Instance.HandCards;
            else
                listToRemove = DeckController.Instance.BoardCards;
            
            for (int i = 0; i < cardModel.CardBlocking.Count; i++)
            {
                var card = listToRemove.GetClassModel(cardModel.CardBlocking[i]);
                card.CardController.RemoveBlockedDataState(card, cardModel.CardId);
            }
        }
        private void RemoveBlockingViewState(PlayingCardModel cardModel)
        {
            RemoveBlockedViewState(cardModel);
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
                cardModel.CardController.OpenDataState(cardModel);
        }
        private void RemoveBlockedViewState(PlayingCardModel cardModel)
        {
            if (cardModel.CardBlockers.Count == 0)
                cardModel.CardController.OpenViewState(cardModel);
        }
    }
}