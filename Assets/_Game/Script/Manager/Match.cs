using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Systems.MatchResult.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Script.Manager
{
    public class MatchData
    {
        public class PlayerData
        {
            public string UserID;
            public string Name;
            public bool IsBot;
        }

        public int MatchID;
        public List<PlayerData> players;
    }

    public enum MatchState
    {
        Created,
        Loaded,
        Begin,
        End,
    }

    public class Match : MonoBehaviour
    {
        public static Match Instance { get; private set; }
        public MatchState State { get; private set; }
        public Action OnBegin { get; set; }
        public Action OnEnd { get; set; }
        public MatchData Data { get; private set; }
        public GameSettings Settings { get; private set; }

        public void Init(MatchData data)
        {
            Instance = this;
            Data = data;
            DontDestroyOnLoad(gameObject);
            Settings = Resources.Load<GameSettings>("GameSettings");
            TinySauce.OnGameStarted(UserManager.Instance.MatchCount.ToString());

            SceneManager.LoadSceneAsync("Game").completed +=
                operation => SetState(MatchState.Loaded);
        }


        private void SetState(MatchState state)
        {
            if (state == State) return;
            State = state;
            switch (state)
            {
                case MatchState.Begin:
                    OnBegin?.Invoke();
                    break;
                case MatchState.End:
                    OnEnd?.Invoke();
                    break;
            }
        }

        public void Begin()
        {
            SetState(MatchState.Begin);
        }

        public void End(MatchResultType result)
        {
            SetState(MatchState.End);
            SceneManager.LoadSceneAsync("Lobby").completed +=
                operation =>
                {
                    FindObjectOfType<MatchRewardPanel>().Show(result);
                    Instance = null;
                    Destroy(gameObject);
                };
        }
    }
}