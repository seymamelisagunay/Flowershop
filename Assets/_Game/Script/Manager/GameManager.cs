using System;
using _Game.Script.Character;
using UnityEngine;

namespace _Game.Script.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameSettings gameSettings;

        public PlayerController playerPrefab;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
        }
    }
}