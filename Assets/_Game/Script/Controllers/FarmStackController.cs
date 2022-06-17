using System.Collections.Generic;
using _Game.Script.Variable;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Script.Controllers
{
    /// <summary>
    /// StackController kendi içinde bir save sistemi olacak
    /// Bir tane kendi altında scriptable object yapısı olacak 
    /// </summary>
    public class FarmStackController : MonoBehaviour, IStackController
    {
        private int _productCount;
        public float duration;
        public List<Transform> finishSocketList = new List<Transform>();
        public List<GameObject> objectList = new List<GameObject>();
        private SlotController _slotController;
        public Transform startPoint;

        public StackObjectList stackObjectList;
        [ReadOnly] public StackData stackData;

        public void Init(SlotController slotController)
        {
            _slotController = slotController;
            stackData = _slotController.slot.stackData;

            for (int i = 0; i < stackData.ProductTypes.Count; i++)
            {
                _productCount = i;
                PlayEffect(stackData.ProductTypes[i]);
            }
        }

        /// <summary>
        /// Değer Ekleniyor ve Bize Doğru bir Obje Geliyor 
        /// </summary>
        /// <param name="productType"></param>
        public void SetValue(ProductType productType)
        {
            _productCount++;
            _productCount = _productCount > finishSocketList.Count - 1 ? finishSocketList.Count - 1 : _productCount;
            stackData.AddProduct(productType);
            PlayEffect(productType);
        }

        private void PlayEffect(ProductType productType)
        {
            Debug.Log(name);
            var farm = stackObjectList.GetStackObject(productType);
            var cloneObject = Instantiate(farm.prefab);
            objectList.Add(cloneObject);
            var cloneMoveObject = cloneObject.GetComponent<MoveObject>();
            cloneMoveObject.Play(startPoint, finishSocketList[_productCount], duration);
        }

        /// <summary>
        /// Obje bizden çıkıp Bizden datayı isteyen Tarafa Gidiyor .
        /// </summary>
        public (ProductType, GameObject, bool) GetValue()
        {
            if (objectList.Count > 0)
            {
                _productCount--;
                var resultObject = objectList[0];
                objectList.Remove(resultObject);
                stackData.RemoveProduct(0);
                return (ProductType.rose, resultObject, true);
            }

            return (ProductType.rose, gameObject, false);
        }
    }

    public interface IStackController
    {
        (ProductType, GameObject, bool) GetValue();
        void SetValue(ProductType productType);
    }
}