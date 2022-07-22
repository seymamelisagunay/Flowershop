using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientManagerSettings",menuName = "Gnarly Team/ClientManagerSettings")]
[Serializable]
public class CustomerManagerSettings : ScriptableObject
{
    /// <summary>
    /// Sahnede max kaç tane client Olacak 
    /// </summary>
    public List<CustomerController> customersPrefab;
    public int clientMaxTradeCount;
    public float botCreateDuration;
}