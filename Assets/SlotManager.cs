using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slot Manager Amacý slotlarýn hangi sýra ile açýlacaðýný 
/// kontrol ettiðimiz yer oluyor
/// </summary>
public class SlotManager : MonoBehaviour
{
    public List<SlotState> slotControllers = new List<SlotState>();

}

[Serializable]
public class SlotState
{
    public int orderCount;
    public SlotController slotController;
}




