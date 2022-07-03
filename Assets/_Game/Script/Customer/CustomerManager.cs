using System.Collections;
using System.Collections.Generic;
using _Game.Script;
using _Game.Script.Controllers;
using _Game.Script.Manager;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

//
public class CustomerManager : MonoBehaviour
{
    public ClientManagerSettings settings;
    [ReadOnly] public CashTradeController cashTradeController;
    public List<CustomerController> clientList = new List<CustomerController>();

    public List<AreaPositionSelector> spawnPoint;
    public BoolVariable isClientCreate;
    public List<GameObject> standList = new List<GameObject>();

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
            yield return new WaitUntil(() => clientList.Count < settings.maxClientCount.Value);
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