using _Game.Script.Character;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Gnarly Team/GameSettings")]
public class GameSettings : ScriptableObject
{
    public PlayerController playerControllerPrefab;
}