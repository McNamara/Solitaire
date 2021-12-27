using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Card
{
    public class CardInput : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            int cardId = GetComponent<CardComponents>().Id;
            
            // TODO move to another class when cardId is param for method to another class 
            var card = DeckController.Instance.BoardCards.GetClassModel(cardId);
            if (card == null)
            {
                card = DeckController.Instance.HandCards.GetClassModel(cardId);
                if (card == null)
                {
                    if (cardId == DeckController.Instance.PivotCard.CardId)
                        DeckController.Instance.PivotCard.CardController.OnClick(DeckController.Instance.PivotCard);
                    else
                        Debug.Log("Cant find clicked card in BoardCards,HandCards and its not Pivot. Abort.");
                }
                else
                    card.CardController.OnClick(card);
            }
            else
                card.CardController.OnClick(card);
        }
    }
}
