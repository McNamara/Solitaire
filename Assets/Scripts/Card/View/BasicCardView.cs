using DataModels.Card;
using JetBrains.Annotations;
using UnityEngine;

namespace Card.View
{
    public abstract class BasicCardView<T>
    {
        // GraphView.Layer
        
         public BasicCardModel<T> CardModel;
         
         public virtual void OnCardClicked(T cardModel) { }
    }
}