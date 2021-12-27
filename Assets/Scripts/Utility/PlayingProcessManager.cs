using System;
using System.Diagnostics;
using DataModels.User;
using ResourceContainers;
using UnityEngine;

namespace Utility
{
    public class PlayingProcessManager : MonoBehaviour
    {
        public static PlayingProcessManager Instance;
        
        
        private void Start()
        {
            Instance = this;
        }

        public void PlayButton()
        {
            ResourcesContainer.Instance.LobbyPanel.SetActive(false);
            ResourcesContainer.Instance.PlayPanel.SetActive(true);
            ResourcesContainer.Instance.NextLevelText.text = $"NEXT LEVEL {PlayerStateModel.LevelProgress}";
        }
        
        public void BackButton()
        {
            ResourcesContainer.Instance.LobbyPanel.SetActive(true);
            ResourcesContainer.Instance.PlayPanel.SetActive(false);
        }
        
        public void StarButton()
        {
            ResourcesContainer.Instance.PlayPanel.SetActive(false);
            ResourcesContainer.Instance.ButtonRestartFromBoard.SetActive(true);
            LevelLoader.Instance.LoadLevelAccordingToProgress();
        }

        public void NextLevelButton()
        {
            ResourcesContainer.Instance.EndLevelPanel.SetActive(false);
            LevelLoader.Instance.DestroyCurrentLevel();
            PlayerStateModel.LevelProgress++;
            LevelLoader.Instance.LoadLevelAccordingToProgress();
        }
        
        public void RestartButton()
        {
            ResourcesContainer.Instance.EndLevelPanel.SetActive(false);
            LevelLoader.Instance.DestroyCurrentLevel();
            LevelLoader.Instance.LoadLevelAccordingToProgress();
        }
        
        public void GameWon()
        {
            ResourcesContainer.Instance.EndLevelPanel.SetActive(true);
            ResourcesContainer.Instance.EndLevelText.text = "LOOK AT YOU.\nYOU WON!\nIT'S AMAZING.";
            ResourcesContainer.Instance.ButtonRestartFromBoard.SetActive(false);
            ResourcesContainer.Instance.NextButton.enabled = true;
        }

        public void GameLost()
        {
            ResourcesContainer.Instance.EndLevelPanel.SetActive(true);
            ResourcesContainer.Instance.EndLevelText.text = "YOU LOST.\nSHAME ON YOU.";
            ResourcesContainer.Instance.ButtonRestartFromBoard.SetActive(false);
            ResourcesContainer.Instance.NextButton.enabled = false;
        }
    }
    
}