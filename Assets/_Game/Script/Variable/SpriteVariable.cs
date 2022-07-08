using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Game.Script.Variable
{
    [CreateAssetMenu(fileName = "SpriteVariable", menuName = "Voli Games/Variable/SpriteVariable")]
    [Serializable]
    public class SpriteVariable : ScriptableObject
    {
        public int id;
        [SerializeField]
        private Sprite _value;
        public Sprite Value
        {
            get => _value;
            set
            {
                _value = value;

            }
        }
        public UnityEvent OnChangeVariable;

        private void OnValidate()
        {
            Value = _value;
        }
    }
}