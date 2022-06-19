using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

//
public class ClientManager : MonoBehaviour
{
    public ClientManagerSettings settings;
    [ReadOnly]
    public CashTradeController cashTradeController;


    private void Start()
    {
        cashTradeController = FindObjectOfType<CashTradeController>();
    }
    [Button]
    public void TestCreateClient()
    {
        var randomShoppingCardCount = Random.Range(1, 5);
        StackData shoppingCard = new StackData();
        for (int i = 0; i < randomShoppingCardCount; i++)
        {
            shoppingCard.ProductTypes.Add(ProductType.Rose);
        }
        var cloneClient = Instantiate(settings.clientPrefab);
        cloneClient.Init(this,settings.clientMaxTradeCount,shoppingCard);

    }
}