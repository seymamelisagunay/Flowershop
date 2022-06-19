using System;
using UnityEngine;

namespace _Game.Script
{
    [Serializable]
    public class StackObject
    {
        public ProductType productType;
        public int price;
        public GameObject prefab;
    }
}