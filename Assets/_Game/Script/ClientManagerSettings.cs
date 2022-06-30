using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientManagerSettings",menuName = "Gnarly Team/ClientManagerSettings")]
public class ClientManagerSettings : ScriptableObject
{
    /// <summary>
    /// Sahnede max kaç tane client Olacak 
    /// </summary>
    public IntVariable maxClientCount;
    public List<CustomerController> customersPrefab;
    public int clientMaxTradeCount;

}