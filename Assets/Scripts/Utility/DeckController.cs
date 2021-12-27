using System;
using System.Collections.Generic;
using System.Linq;
using DataModels.Card;
using Enums;
using Enums.Card;
using UnityEngine;
using Random = System.Random;

namespace Utility
{
    public class DeckController : GenericSingleton<DeckController>
    {
        [NonSerialized] public List<GameObject> GeneratedPrefabs = new List<GameObject>();
        public GenericDictionary CreatedCards = new GenericDictionary();
        public GenericDictionary BoardCards = new GenericDictionary();
        public GenericDictionary HandCards = new GenericDictionary();
        // TODO dynamic
        public PlayingCardModel PivotCard;
        public GenericDictionary PivotCards = new GenericDictionary();
        public GenericDictionary DisappearedCards = new GenericDictionary();
        /*[NonSerialized]*/ public List<int> OpenedCardsId = new List<int>();

        public bool isHandIsEmpty;
        public int RanksCount = Enum.GetNames(typeof(CardRanks)).Length -2;
        public int SuitsCount = Enum.GetNames(typeof(CardSuits)).Length -2;
        
        private Dictionary<int, int[]> FreeCardsPool = new Dictionary<int, int[]>();
        private int cardsInDeck;
        private int decksCount = 0;
        private const int RankArrayIndex = 0;
        private const int SuitArrayIndex = 1;
        
        public DeckController()
        { 
            cardsInDeck = RanksCount * SuitsCount;
            AddNewDeckToFreeCardsPool();
            /*PreLevelLoadCardsPrefabs();*/
        }
        /*private void PreLevelLoadCardsPrefabs()
        {
            for (int i = 0; i < cardsInDeck; i++)
            {
                var cardPrefab = Load(Constants.Level.BasicCardPrefab,
                    ResourcesContainer.Instance.BoardCanvas.transform).Result;
                GeneratedPrefabs.Add(cardPrefab);
            }
        }*/
        
        public void AddNewDeckToFreeCardsPool()
        {
            decksCount++;
            var suitsCount = Enum.GetNames(typeof(CardSuits)).Length - 2;
            var ranksCount = Enum.GetNames(typeof(CardRanks)).Length - 2;
            
            int startingKeyIndex = FreeCardsPool.Count == 0 ? -1 : FreeCardsPool.Keys.Max();
            
            for (int suits = (int)CardSuits.None +1; suits <= suitsCount; suits++)
            {
                for (int rank = (int)CardRanks.None +1; rank <= ranksCount; rank++)
                {
                    var key = startingKeyIndex + (suits-1)*ranksCount + rank; 
                    FreeCardsPool.Add(key, new[]{rank,suits});
                }
            }
        }
        
        public int[] GetNewCardValue(int[] oldValue)
        {
            int[] value = ExtractRandomCardFromFreeCardsPool();
            
            if (oldValue[RankArrayIndex] != (int)CardSuits.None && oldValue[SuitArrayIndex] != (int)CardSuits.None)
                FreeCardsPool.Add(FreeCardsPool.Keys.Max()+1, oldValue);
            
            return value;
        }
        private int[] ExtractRandomCardFromFreeCardsPool()
        {
            if (FreeCardsPool.Count == 0)
                AddNewDeckToFreeCardsPool();
            
            var keyList = new List<int>(FreeCardsPool.Keys);
            var random = new Random();
            int key = keyList[random.Next(keyList.Count)];
            
            int[] value = FreeCardsPool[key];
            
            FreeCardsPool.Remove(key);
            
            return value;
        }
        
        public void RemoveCardFromFreeCardsPool(int[] value,bool addNewDeckIfValueMissing = true)
        {
            var item = FreeCardsPool.First(x => x.Value == value);
            if (item.Value != null)
                FreeCardsPool.Remove(item.Key);
            else
            {
                if (addNewDeckIfValueMissing)
                {
                    AddNewDeckToFreeCardsPool();
                    item = FreeCardsPool.First(x => x.Value == value);
                    FreeCardsPool.Remove(item.Key);
                }
            }
        }
        
        public void MoveCardFromHandToPivot(dynamic cardModel)
        {
            ChangePivotCard(cardModel);
            HandCards.Remove(cardModel.CardId);
        }
        public void MoveCardFromBoardToPivot(dynamic cardModel)
        {
            ChangePivotCard(cardModel);
            BoardCards.Remove(cardModel.CardId);
            
            if (BoardCards.GetCount() == 0)
                PlayingProcessManager.Instance.GameWon();
        }
        private void ChangePivotCard(dynamic cardModel)
        {
            OpenedCardsId.Remove(PivotCard.CardId);
            PivotCards.Add(PivotCard.CardId, PivotCard);
            PivotCard = cardModel;
        }

        // TODO MOVE TO ANOTHER PLACE?
        public void CheckForEndLevelConditions()
        {
            if (!IsNextTurnAvailable())
                PlayingProcessManager.Instance.GameLost();
        }
        private bool IsNextTurnAvailable()
        {
            if (HandCards.GetCount()!=0)
                return true;
            var openedCardsIdCount = OpenedCardsId.Count;
            for (int i = 0; i < openedCardsIdCount; i++)
            {
                var cardModel = BoardCards.GetClassModel(OpenedCardsId[i]);
                
                if (!cardModel.IsItPivot)
                    if(cardModel.CardController.IsCardCanBeMovedToPivot(cardModel))
                        return true;
            }

            return false;
        }
    }
}