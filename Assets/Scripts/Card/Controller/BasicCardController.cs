namespace Card.Controller
{
    public  class BasicCardController<TDynamic>
    {
        public virtual void OnClick(TDynamic cardModel){}
        public virtual void OnClickDataState(TDynamic cardModel){}
        
        
        
        public virtual  bool IsBoardCardMatchToPivot(TDynamic cardModel){return false;}
        public virtual  bool IsCardCanBeMovedFromHandToPivot(TDynamic cardModel){return false;}
        public virtual  bool IsCardCanDestroyBarrier(TDynamic cardModel){return false;}
        
        
        
        public virtual void Open(TDynamic cardModel){}
        public virtual void OpenDataState(TDynamic cardModel){}
 
        
        
        public virtual void PlayToPivot(TDynamic cardModel){}
        public virtual void PlayToPivotDataState(TDynamic cardModel){}
        
        
        
        public virtual void PlayToBarrier(TDynamic cardModel){}
        


        /// <summary>
        ///     What this card is Blocking
        /// </summary>
        public virtual void RemoveBlocking(TDynamic cardModel){}
        public virtual void RemoveBlockingDataState(TDynamic cardModel){}
        
        
        /// <summary>
        ///     What is BLocked card
        /// </summary>
        public virtual void RemoveBlocked(TDynamic cardModel, int value){}
        public virtual void RemoveBlockedDataState(TDynamic cardModel, int value){}
        
        
        
        public virtual void ActionPerTurn(TDynamic cardModel){}
        
        
        
        // TODO replaced cardModel.CardBlockers.count?  
        public  virtual  bool? IsCardBlocked(TDynamic cardModel)
        {
            /*if (cardModel.CardBlockers.Count == 0)
                return true;
            if (cardModel.CardBlockers.Count > 0)
                return false;*/

            return null;
        }
    }
}