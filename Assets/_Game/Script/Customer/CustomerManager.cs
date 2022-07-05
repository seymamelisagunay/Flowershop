using System.Collections;
using System.Collections.Generic;
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
    public List<ItemType> itemTypes = new List<ItemType>();
    public List<AreaPositionSelector> spawnPoint;
    public BoolVariable isClientCreate;
    public List<SlotController> standList = new List<SlotController>();

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
        // var standList 
        var randomShoppingCardCount = Random.Range(1, 5);
        var shoppingCard = new StackData();
        for (var i = 0; i < randomShoppingCardCount; i++)
        {
            //Burada Random Verilecek aktif olan Ürünlere göre ;
            shoppingCard.ProductTypes.Add(ItemType.Rose);
        }

        var selectCustomerPrefab = settings.customersPrefab.RandomSelectObject();
        var cloneCustomer = Instantiate(selectCustomerPrefab);
        var customerFirstPosition = spawnPoint.RandomSelectObject().GetPosition();
        cloneCustomer.Init(this, settings.clientMaxTradeCount, shoppingCard, customerFirstPosition);
        clientList.Add(cloneCustomer);
    }

    public void RemoveCustomer(CustomerController customer)
    {
        clientList.Remove(customer);
    }
}