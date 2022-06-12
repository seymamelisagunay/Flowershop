using UnityEngine;
using NaughtyAttributes;
using Newtonsoft.Json;

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
    public SlotEmpty slotEmptyPrefab;
    /// <summary>
    /// slot UI HUD
    /// </summary>
    public SlotHud slotHudPrefab;
    /// <summary>
    /// Slot içerisine girildiğinde ne sürede işleme başlayacağız
    /// </summary>
    public float firstTriggerTime;
    /// <summary>
    /// Bunun Empty İçinbe alınması lazım ismi değiştirilip
    /// </summary>
    [InfoBox("Boş Slot Bilgilerinin Tutulduğu yer!")]
    [Space]
    public SlotEmptyData emptyData;

    public SlotFullData slotData;
    private void OnValidate()
    {
        emptyData.OnValidate();
    }
}

public class SlotFullData{

}
public enum SlotType
{

}



