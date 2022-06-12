using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Script.Character;
using UnityEngine;

namespace _Game.Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameSettings gameSettings;
        public Transform playerSpawnPoint;
        [HideInInspector]
        public PlayerController activePlayer;

        public List<SlotController> slotList = new List<SlotController>();
        private void Awake()
        {
            instance = this;
            GetAllSlotController();
        }
        /// <summary>
        /// Game is start
        /// </summary>
        public void Init()
        {
            PlayerCreater();
            slotList.ForEach(x=>{
                x.Init();
            });
        }

        private void PlayerCreater()
        {
            activePlayer = Instantiate(gameSettings.playerControllerPrefab);
            activePlayer.Init(playerSpawnPoint);
        }


        public void GetAllSlotController()
        {
            var slots = FindObjectsOfType<SlotController>();
            slotList = slots.ToList();
        }
    }
}