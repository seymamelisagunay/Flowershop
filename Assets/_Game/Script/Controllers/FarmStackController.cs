using System.Collections;
using System.Collections.Generic;
using _Game.Script.Variable;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Script.Controllers
{
    //İtem Kontroller
    /// <summary>
    /// StackController kendi içinde bir save sistemi olacak
    /// Bir tane kendi altında scriptable object yapısı olacak 
    /// </summary>
    public class FarmStackController : MonoBehaviour, IStackController
    {
        private int _productCount;
        public List<Transform> finishSocketList = new List<Transform>();
        public List<Item> itemDataList = new List<Item>();
        private SlotController _slotController;
        public ItemList itemList;
        [ReadOnly] public StackData stackData;

        public void Init(SlotController slotController)
        {
            _slotController = slotController;
            stackData = _slotController.slot.stackData;
            StartCoroutine(StackDataLoad());
        }

        private IEnumerator StackDataLoad()
        {
            for (int i = 0; i < stackData.ProductTypes.Count; i++)
            {
                yield return new WaitForSeconds(0.5f);
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
            var farm = itemList.GetItemPrefab(itemType);
            var cloneObject = Instantiate(farm, transform);
            Debug.Log("cloneObject" + cloneObject.transform.position);
            cloneObject.Play(finishSocketList[_productCount]);
            itemDataList.Add(cloneObject);
        }

        /// <summary>
        /// Obje bizden çıkıp Bizden datayı isteyen Tarafa Gidiyor .
        /// </summary>
        public (ItemType, Item, bool) GetValue()
        {
            if (itemDataList.Count > 0)
            {
                _productCount--;
                var resultObject = itemDataList[0];
                itemDataList.Remove(resultObject);
                stackData.RemoveProduct(0);
                return (ItemType.Rose, resultObject, true);
            }

            return (ItemType.Rose, null, false);
        }
        
        public bool CheckMaxCount()
        {
            return stackData.ProductTypes.Count < stackData.MaxItemCount;
        }
    }

    public interface IStackController
    {
        void Init(SlotController slotController);
        (ItemType, Item, bool) GetValue();
        void SetValue(ItemType itemType);
    }
}