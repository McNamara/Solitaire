using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceContainers
{
    public class ResourcesContainer : MonoBehaviour
    {
        [Header("Playing Card Sprites")]
        public List<Sprite> PlayingCardValueSuitSprites;
        [Header("Playing Card Skins")]
        public List<Sprite> PlayingCardSkinSprites;
        [Header("Base Card Prefab")]
        public GameObject BasicCardPrefab;
        
        [Header("Board Canvas")]
        public Canvas BoardCanvas;
        public Transform BoardCanvasTransform;
        public GameObject ButtonRestartFromBoard;
        
        [Header("Ui Canvas")]
        public GameObject LobbyPanel;
        public GameObject ButtonPlay;
        public GameObject PlayPanel;
        public Text NextLevelText;
        public GameObject EndLevelPanel;
        public Text EndLevelText;
        public Button NextButton;
        
        public static ResourcesContainer Instance;
        private void Start()
        
        
        {
            Instance = this;
            BoardCanvasTransform = BoardCanvas.transform;
        }
    }
}