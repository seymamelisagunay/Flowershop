using UnityEngine;

[CreateAssetMenu(fileName = "SlotData", menuName = "Gnarly Team/SlotData")]
public class SlotData : ScriptableObject
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
    /// Slot satın alındımı alınmadımı onu anladımığımız yer oluyor 
    /// </summary>
    public bool isOpen;
    /// <summary>
    /// Slotun ne olduğunu belirtiğimiz yer oluyor .
    /// </summary>
    public SlotType slotType;
    /// <summary>
    /// Slot satın alma fiatı değişken adı ileride değişmesi daha açıklayıcı olması gerek 
    /// </summary>
    public int price = 10;

}

public enum SlotType{

}



