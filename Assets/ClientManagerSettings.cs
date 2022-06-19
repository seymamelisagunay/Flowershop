using UnityEngine;

[CreateAssetMenu(fileName = "ClientManagerSettings",menuName = "Gnarly Team/ClientManagerSettings")]
public class ClientManagerSettings : ScriptableObject
{
    public IntVariable maxClientCount;
    public ClientController clientPrefab;
    public int clientMaxTradeCount;

}