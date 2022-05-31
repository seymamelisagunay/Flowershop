using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Gnarly Team/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Player")] [Space] public float playerSpeed = 2.5f;
    public float rotateLerpFactor = 10;
    public int MatchTime;
}