using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Script.Character;
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
        [HideInInspector]
        public PlayerController activePlayer;
        public SlotManager slotManager;
        public BoolVariable isClientCreate;
        public CustomCameraFollow customCamera;

        public NavMeshUtility NavMesh;

        private void Awake()
        {
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
            slotManager.slotStates.ForEach(x =>
            {
                x.slotController.Init();
            });
            slotManager.SlotOpen();
        }

        private void PlayerCreator()
        {
            activePlayer = Instantiate(gameSettings.playerControllerPrefab);
            activePlayer.Init(playerSpawnPoint);
        }
    }
}