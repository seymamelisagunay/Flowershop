using System;
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
        private void Awake()
        {
            instance = this;
        }
        /// <summary>
        /// Game is start
        /// </summary>
        public void Init()
        {
            PlayerCreater();
        }

        private void PlayerCreater()
        {
            activePlayer = Instantiate(gameSettings.playerControllerPrefab);
            activePlayer.Init(playerSpawnPoint);
        }
    }
}