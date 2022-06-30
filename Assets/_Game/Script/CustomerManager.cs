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

    private void Start()
    {
        cashTradeController = FindObjectOfType<CashTradeController>();
        StartCoroutine(BotCreator());
    }

    private IEnumerator BotCreator()
    {
        while (true)
        {
            // Burada Game manager içerisinde Üretilmiş bir tane stand var mı diye bakacağız ve ona göre Client Üretim arasına gireceğiz
            yield return new WaitUntil(() => isClientCreate.Value);
            yield return new WaitUntil(() => clientList.Count < settings.maxClientCount.Value);
            yield return new WaitForSeconds(0.5f);
            // Bot Oluşturulacak 
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
        var cloneClient = Instantiate(selectCustomerPrefab);
        cloneClient.Init(this, settings.clientMaxTradeCount, shoppingCard);
        cloneClient.transform.position = spawnPoint.RandomSelectObject().GetPosition();
        clientList.Add(cloneClient);
    }
}