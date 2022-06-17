using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Script.Controllers
{
    [Serializable]
    public class StackData
    {
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private int maxProductCount;

        public int MaxProductCount
        {
            get => maxProductCount;
            set
            {
                maxProductCount = value;
                OnChangeVariable?.Invoke(this);
            }
        }

        [SerializeField] private float productionRate;

        public float ProductionRate
        {
            get => productionRate;
            set
            {
                productionRate = value;
                OnChangeVariable?.Invoke(this);
            }
        }

        [SerializeField] private List<ProductType> _productTypes;

        public List<ProductType> ProductTypes
        {
            get => _productTypes;
            set
            {
                _productTypes = value;
                OnChangeVariable?.Invoke(this);
            }
        }

        public void AddProduct(ProductType productType)
        {
            _productTypes.Add(productType);
            OnChangeVariable?.Invoke(this);
        }

        public void RemoveProduct(int index)
        {
            _productTypes.RemoveAt(index);
            OnChangeVariable?.Invoke(this);
        }
        public bool CheckMaxCount()
        {
            return _productTypes.Count < maxProductCount;
        }
        /// <summary>
        /// 
        /// </summary>
        public UnityEvent<StackData> OnChangeVariable;

        public void OnValidate()
        {
            MaxProductCount = maxProductCount;
            ProductionRate = productionRate;
        }
    }
}