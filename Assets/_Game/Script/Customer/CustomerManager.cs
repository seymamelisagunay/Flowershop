using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Script;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

//
public class CustomerManager : MonoBehaviour
{
    public CustomerManagerSettings settings;
    [ReadOnly] public CashTradeController cashTradeController;
    public List<CustomerController> clientList = new List<CustomerController>();
    public ItemTypeList itemTypes;
    public List<AreaPositionSelector> spawnPoint;
    public BoolVariable isClientCreate;
    public List<ItemType> selectItem;

    private int _maxCustomerCount;

    private void Start()
    {
        StartCoroutine(BotCreator());
    }

    private IEnumerator BotCreator()
    {
        while (true)
        {
            // Burada Game manager içerisinde Üretilmiş bir tane stand var mı diye bakacağız ve ona göre Client Üretim arasına gireceğiz
            yield return new WaitUntil(() => isClientCreate.Value);
            yield return new WaitUntil(() => clientList.Count < _maxCustomerCount);
            // var randomDuration = Random.Range(settings.botCreateDuration / 2, settings.botCreateDuration);
            CreateClient();
            yield return new WaitForSeconds(settings.botCreateDuration);
        }
    }

    [Button]
    private void CreateClient()
    {
        var randomShoppingCardCount = Random.Range(1, 5);
        var shoppingCard = new StackData();
        for (var i = 0; i < randomShoppingCardCount; i++)
        {
            var randomItemType = SelectRandomItemType();
            //Burada Random Verilecek aktif olan Ürünlere göre ;
            shoppingCard.ProductTypes.Add(randomItemType);
        }
        shoppingCard.ProductTypes.Sort();
        var selectCustomerPrefab = settings.customersPrefab.RandomSelectObject();
        var cloneCustomer = Instantiate(selectCustomerPrefab);
        var customerFirstPosition = spawnPoint.RandomSelectObject().GetPosition();
        cloneCustomer.Init(this, settings.clientMaxTradeCount, shoppingCard, customerFirstPosition);
        clientList.Add(cloneCustomer);
    }

    /// <summary>
    /// Koşullara göre Random item Type Döndürecek
    /// </summary>
    private ItemType SelectRandomItemType()
    {
        var randomItemType = itemTypes.value.RandomSelectObject();
        var factorys = SlotManager.instance.GetSlotController(SlotType.Factory);
        switch (randomItemType)
        {
            case ItemType.Water:
                // Factory Üretildimi diye bakacağız 
                var factory = factorys.Find(x => x.slot.itemType == randomItemType);
                if (factory == null)
                {
                    var itemType = selectItem.Find(x => x == randomItemType);
                    if (itemType == randomItemType)
                        randomItemType = ItemType.Rose;
                    else
                        selectItem.Add(randomItemType);
                }
                break;
            case ItemType.Delight:
                // bURAYA YAzıcağız 
                var delightFactory = factorys.Find(x => x.slot.itemType == randomItemType);
                if (delightFactory == null)
                {
                    var itemType = selectItem.Find(x => x == randomItemType);
                    if (itemType == randomItemType)
                        randomItemType = ItemType.Rose;
                    else
                        selectItem.Add(randomItemType);
                }
                break;
        }

        return randomItemType;
    }

    public void IncreaseCustomerLimit(int increaseValue)
    {
        _maxCustomerCount += increaseValue;
    }

    public void RemoveCustomer(CustomerController customer)
    {
        clientList.Remove(customer);
    }
}