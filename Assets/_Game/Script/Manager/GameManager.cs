using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Script.Character;
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
            PlayerCreater();
            slotManager.slotStates.ForEach(x =>
            {
                x.slotController.Init();
            });
            slotManager.SlotOpen();
        }

        private void PlayerCreater()
        {
            activePlayer = Instantiate(gameSettings.playerControllerPrefab);
            activePlayer.Init(playerSpawnPoint);
        }
    }
}