using _Game.Script.Controllers;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Slot", menuName = "Gnarly Team/Slot")]
public class Slot : ScriptableObject
{
    [ShowAssetPreview] public Sprite logo;

    /// <summary>
    /// Slot Adı bizim tanımamzı için kullanılır.
    /// </summary>
    public string slotName;

    /// <summary>
    /// Slot Id Tanıma ve Bu slotu player prefs içersinde kaytı eder iken 
    /// key olarak kullanmaya yarar
    /// </summary>
    public string Id;

    public ItemType itemType;

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
    public GameObject itemControllerPrefab;

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

    public float focusWaiting;

    /// <summary>
    /// Bunun Empty İçinbe alınması lazım ismi değiştirilip
    /// </summary>
    [InfoBox("Boş Slot Bilgilerinin Tutulduğu yer!")] [Space]
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

    [Button]
    public void RestartData()
    {
        stackData.ProductTypes.Clear();
        emptyData.IsOpen = false;
        emptyData.CurrenctPrice = 0;
        PlayerPrefs.DeleteKey(Id + "-Empty");
        PlayerPrefs.DeleteKey(Id + "-StackData");
    }
}

public enum SlotType
{
    Farm = 0,
    Factory = 1,
    Stand = 2,
    CashDesk = 3,
    Money = 4,
    Shelver = 5,
    Cashier = 6
}

public enum ItemType
{
    none = 0,
    Rose = 1,
    Water = 2,
    Delight = 3,
    Money = 4
}