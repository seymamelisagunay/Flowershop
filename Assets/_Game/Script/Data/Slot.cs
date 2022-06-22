using _Game.Script.Controllers;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Slot", menuName = "Gnarly Team/Slot")]
public class Slot : ScriptableObject
{
    [ShowAssetPreview]
    public Sprite logo;
    /// <summary>
    /// Slot Adı bizim tanımamzı için kullanılır.
    /// </summary>
    public string slotName;
    /// <summary>
    /// Slot Id Tanıma ve Bu slotu player prefs içersinde kaytı eder iken 
    /// key olarak kullanmaya yarar
    /// </summary>
    public string Id;
    /// <summary>
    /// Slotun ne olduğunu belirtiğimiz yer oluyor .
    /// </summary>
    public SlotType slotType;
    /// <summary>
    /// boş slot Yapısı
    /// </summary>
    public GameObject slotEmptyPrefab;
    /// <summary>
    /// 
    /// </summary>
    public FarmController farmControllerPrefab;
    /// <summary>
    /// 
    /// </summary>
    public StandController standControllerPrefab;
    /// <summary>
    /// 
    /// </summary>
    public CashDeskController cashDeskControllerPrefab;
    /// <summary>
    /// slot UI HUD
    /// </summary>
    public SlotHud slotHudPrefab;
    /// <summary>
    /// Slot içerisine girildiğinde ne sürede işleme başlayacağız
    /// </summary>
    public float firstTriggerCooldown;
    /// <summary>
    /// 
    /// </summary>
    public float triggerCooldown = 0.05f;
    /// <summary>
    /// Bunun Empty İçinbe alınması lazım ismi değiştirilip
    /// </summary>
    [InfoBox("Boş Slot Bilgilerinin Tutulduğu yer!")]
    [Space]
    public SlotEmptyData emptyData;
    /// <summary>
    /// 
    /// </summary>
    public StackData stackData = new StackData();

    private void OnValidate()
    {
        emptyData.OnValidate();
        stackData.OnValidate();
    }
}

public enum SlotType
{
    Farm = 0,
    Factory = 1,
    Stand = 2,
    CashDesk = 3,
    Money = 4
}

public enum ItemType
{
    Rose,
    Perfume,
    Candy,
    Money
}



