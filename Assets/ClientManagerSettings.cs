using UnityEngine;

[CreateAssetMenu(fileName = "ClientManagerSettings",menuName = "Gnarly Team/ClientManagerSettings")]
public class ClientManagerSettings : ScriptableObject
{
    /// <summary>
    /// Sahnede max kaç tane client Olacak 
    /// </summary>
    public IntVariable maxClientCount;
    public ClientController clientPrefab;
    public int clientMaxTradeCount;

}