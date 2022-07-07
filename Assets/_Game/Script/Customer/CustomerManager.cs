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
    public int maxClientCount = 4;
    public int firstCustomer = 0;
    [ReadOnly] public CashTradeController cashTradeController;
    public List<CustomerController> clientList = new List<CustomerController>();
    public ItemTypeList itemTypes;
    public List<AreaPositionSelector> spawnPoint;
    public BoolVariable isClientCreate;
    public List<ItemType> selectItem;

    private void Start()
    {
        firstCustomer = PlayerPrefs.GetInt("CustomerCount", 0);
        if (firstCustomer >= settings.firstCustomer)
        {
            settings.maxClientCount = maxClientCount;
        }

        StartCoroutine(BotCreator());
    }

    public void SaveFirstCustomerCount()
    {
        PlayerPrefs.SetInt("CustomerCount", firstCustomer);
    }

    private IEnumerator BotCreator()
    {
        while (true)
        {
            // Burada Game manager içerisinde Üretilmiş bir tane stand var mı diye bakacağız ve ona göre Client Üretim arasına gireceğiz
            yield return new WaitUntil(() => isClientCreate.Value);
            yield return new WaitUntil(() => clientList.Count < settings.maxClientCount);
            var randomDuration = Random.Range(settings.botCreateDuration / 2, settings.botCreateDuration);
            yield return new WaitForSeconds(randomDuration);
            CreateClient();
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
        switch (randomItemType)
        {
            case ItemType.Perfume:
                // Factory Üretildimi diye bakacağız 
                var factorys = SlotManager.instance.GetSlotController(SlotType.Factory);
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
            case ItemType.Candy:
                break;
        }

        return randomItemType;
    }

    public void RemoveCustomer(CustomerController customer)
    {
        clientList.Remove(customer);
    }
}