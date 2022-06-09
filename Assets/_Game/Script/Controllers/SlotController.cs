using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Slot Yapısı bunun üzerine kurulacak
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SlotController : MonoBehaviour, ISlot
{
    public SlotData slotData;
    public void OpenSlot()
    {
    }
    public void OnTriggerEnter(Collider collider)
    {
    }
    public void OnTriggerExit(Collider collider)
    {
    }
    [Button]
    private void SlotNameSet()
    {
        name = slotData.slotName;
    }
}
public interface ISlot
{
    /// <summary>
    ///  Slot eğer kapalı ise satın alınması gerekir
    /// </summary>
    void OpenSlot();
    void OnTriggerEnter(Collider collider);
    void OnTriggerExit(Collider other);
}

