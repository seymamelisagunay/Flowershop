using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Slot", menuName = "Gnarly Team/Slot")]
public class Slot : ScriptableObject
{
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
    public SlotEmptyController slotEmptyPrefab;
    /// <summary>
    /// 
    /// </summary>
    public GameObject factoryController;
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

    private void OnValidate()
    {
        emptyData.OnValidate();
    }
}

public enum SlotType
{
    farm = 0,
    factory = 1,
    stand = 2,
    checkout = 3
}



