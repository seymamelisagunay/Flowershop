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
        public List<Item> objectList = new List<Item>();
        private SlotController _slotController;
        public Transform startPoint;

        public ItemList itemList;
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
        /// <param name="itemType"></param>
        public void SetValue(ItemType itemType)
        {
            _productCount++;
            _productCount = _productCount > finishSocketList.Count - 1 ? finishSocketList.Count - 1 : _productCount;
            stackData.AddProduct(itemType);
            PlayEffect(itemType);
        }

        private void PlayEffect(ItemType itemType)
        {
            Debug.Log(name);
            var farm = itemList.GetStackObject(itemType);
            var cloneObject = Instantiate(farm);
            objectList.Add(cloneObject);
            var cloneMoveObject = cloneObject.GetComponent<MoveObject>();
            cloneMoveObject.Play(startPoint, finishSocketList[_productCount], duration);
        }

        /// <summary>
        /// Obje bizden çıkıp Bizden datayı isteyen Tarafa Gidiyor .
        /// </summary>
        public (ItemType, Item, bool) GetValue()
        {
            if (objectList.Count > 0)
            {
                _productCount--;
                var resultObject = objectList[0];
                objectList.Remove(resultObject);
                stackData.RemoveProduct(0);
                return (ItemType.Rose, resultObject, true);
            }

            return (ItemType.Rose, null, false);
        }
    }

    public interface IStackController
    {
        (ItemType, Item, bool) GetValue();
        void SetValue(ItemType itemType);
    }
}