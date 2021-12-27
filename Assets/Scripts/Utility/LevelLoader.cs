using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Card.Controller;
using Constants;
using DataModels.Card;
using DataModels.JsonLevelModel;
using DataModels.User;
using ResourceContainers;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Utility 
{
    public class LevelLoader : GenericSingleton<LevelLoader>
    {
        public LevelJsonInfo CurrentLevel { get; set; }
        private const int PivotAngel = 0;
        private const int PivotLayer = 0;
        
        public void LoadLevelAccordingToProgress()
        {
            CurrentLevel = JsonManager.Instance.LoadJsonLevel(PlayerStateModel.LevelProgress);

            InitiateLevelCards(CurrentLevel);
        }

        private async void InitiateLevelCards(LevelJsonInfo levelJsonInfo)
        {
            await GenerateCards(levelJsonInfo);
            //ShowOpenedCards();
        }

        private async Task GenerateCards(LevelJsonInfo levelJsonInfo)
        {
            InitiateHandCards(levelJsonInfo.HandCards);
            InitiatePivotCard(levelJsonInfo.PivotCard);
            InitiateBoardCards(levelJsonInfo.BoardCards);
        }
        
        private async Task<dynamic> CardCreator(JsonCard jsonCard)
        {
            var cardPrefab = Instantiate(ResourcesContainer.Instance.BasicCardPrefab,
                ResourcesContainer.Instance.BoardCanvas.transform);
            return jsonCard.Type switch
            {
                1 => new PlayingCardModel(jsonCard, cardPrefab),
                2 => new ArrowsCardModel(jsonCard, cardPrefab),
                _ => null
            };
        }
        
        private void InitiatePivotCard(JsonCard pivotCard)
        {
            var pivotPos = GetPivotCardVector2();
            pivotCard.X = pivotPos.x;
            pivotCard.Y = pivotPos.y;
            
            // TODO dynamic
            var pivot = CardCreator(pivotCard).Result as PlayingCardModel;
            DeckController.Instance.CreatedCards.Add(pivot.CardId,  pivot);
            DeckController.Instance.PivotCard = pivot;
            DeckController.Instance.PivotCard.CardController.Open(pivot);
            pivot.CardView.SetActiveNextCardBoxColliderInHand(pivot.CardBlocking[0]);
        }
        private Vector2 GetPivotCardVector2()
        {
            // TODO get vector2 depens on screen resolution
            return new Vector2(0,-385);
        }
        private void InitiateBoardCards(List<JsonCard> jsonCard)
         {
             for (int i = 0; i < jsonCard.Count; i++)
             {
                 var cardModel = CardCreator(jsonCard[i]).Result;
                 //var card = Task.Run(() => CardCreator(jsonCard[i])).Result; 
                 DeckController.Instance.CreatedCards.Add(cardModel.CardId,  cardModel);
                 DeckController.Instance.BoardCards.Add(cardModel.CardId,  cardModel);
                 
                 var blockers = jsonCard[i].Blockers;
                 if (blockers.Count == 0)
                 {
                     cardModel.CardController.Open(cardModel);
                     // TODO DEL
                     //DeckController.Instance.OpenedCardsId.Add(card.CardId);
                 }
             }
         }
        private void InitiateHandCards(List<JsonCard> cardsInHand)
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                var card = CardCreator(cardsInHand[i]).Result;
                //var card = Task.Run(() => CardCreator(cardsInHand[i])).Result;
                card.IsInHand = true;
                DeckController.Instance.CreatedCards.Add(card.CardId, card );
                DeckController.Instance.HandCards.Add(card.CardId, card);
            }
        }
        /*private void ShowOpenedCards()
        {
            var openedCardsId = DeckController.Instance.OpenedCardsId;
            for (int i = 0; i < openedCardsId.Count; i++)
            {
                var card = DeckController.Instance.BoardCards.GetClassModel(openedCardsId[i]);
                card.CardController.Open(card);
            }
        }*/

        public void DestroyCurrentLevel()
        {
            DeckController.Instance.CreatedCards.Clear();
            DeckController.Instance.BoardCards.Clear();
            DeckController.Instance.HandCards.Clear();
            DeckController.Instance.PivotCards.Clear();
            DeckController.Instance.DisappearedCards.Clear();
            DeckController.Instance.OpenedCardsId.Clear();
            DeckController.Instance.isHandIsEmpty = false;
            SceneManager.LoadScene(SceneNames.PIPlitaire.ToString());
        }
    }
}