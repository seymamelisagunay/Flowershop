using UnityEngine;

namespace _Game.Script.Bot
{
    public interface IInput
    {
        public Vector3 Direction { get; }
        public void SetDirection(Vector3 direction);
        public void ClearDirection();
        void StartListen();
        void StopListen();
    }
}