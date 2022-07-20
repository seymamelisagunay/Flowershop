using System.Collections.Generic;
using _Game.Script.Character;
using Assets._Game.Script.Variable;
using Sources.Utility;
using UnityEngine;

namespace _Game.Script.Manager
{
    [DefaultExecutionOrder(-101)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameSettings gameSettings;
        public Transform playerSpawnPoint;
        [HideInInspector] public PlayerController activePlayer;
        public SlotManager slotManager;
        public BoolVariable isClientCreate;
        public CustomCameraFollow customCamera;
        public ItemTypeList ItemTypeList;
        public ItemList itemList;
        public NavMeshUtility NavMesh;
        public List<SpriteVariable> itemIcons;
        public List<SpriteVariable> emojiIcons;
        public SpriteVariable cashDeskIcon;

        private void Awake()
        {
            Application.targetFrameRate = gameSettings.fpsCount;
            ItemTypeList.value.Clear();
            instance = this;
            slotManager = FindObjectOfType<SlotManager>();
        }

        /// <summary>
        /// Game is start
        /// </summary>
        public void Init()
        {
            NavMesh = new NavMeshUtility(null, Vector3.zero);
            PlayerCreator();
            slotManager.slotStates.ForEach(x => { x.slotController.Init(); });
            slotManager.SlotOpen();
        }

        private void PlayerCreator()
        {
            activePlayer = Instantiate(gameSettings.playerControllerPrefab);
            activePlayer.Init(playerSpawnPoint);
        }
    }
}