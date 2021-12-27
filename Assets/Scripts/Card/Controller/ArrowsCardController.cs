using System.Collections.Generic;
using DataModels.Card;

namespace Card.Controller
{
    public class ArrowsCardController : BasicCardController<ArrowsCardModel>
    {
        public override void OnClick(ArrowsCardModel cardModel){}
        
        public override void Open(ArrowsCardModel cardModel){}
        
        public override void PlayToPivot(ArrowsCardModel cardModel)
        {
            // TODO провіряєм чи значення card.IsItAdditionalCardValue == true
            // TODO якшо так додаєм 52карти в деку
            // TODO видаляєм звітам це cardModel.RanK cardModel.Siut
            // TODO якшо ні іграєм анімацію цієї карти, сетаем її як Півот, додаєм до списку півот
        }
        
        public override void ActionPerTurn(ArrowsCardModel cardModel)
        {
            if (cardModel.IsCardOpen)
                return;
            // TODO провіряєм чи значення card.IsItAdditionalCardValue == true
                // TODO якщо так
                    // TODO НЕ повертаємо це значення в колоду
                    // TODO card.IsItAdditionalCardValue = false
                    
                // TODO якшо ні
                    // TODO - повертаємо
                    
            // TODO чекаэм чи наступне значення э в деці
                // TODO якщо немає, то
                    // TODO card.IsItAdditionalCardValue = true
                    // TODO задаєм цій карті потрібне значення 
                // TODO якшо є то
                    // TODO задаєм цій карті потрібне значення
                    // TODO забираєм карту з колоди
        }

        /// <summary>
        ///     What this card is Blocking
        /// </summary>
        public override void RemoveBlocking(ArrowsCardModel cardModel)
        {
            
        }
        
        /// <summary>
        ///     What is BLocked card
        /// </summary>
        public override void RemoveBlocked(ArrowsCardModel cardModel, int value) {}
    }
}