using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SlotHud : MonoBehaviour
{
    public List<ISlot> hudHelpers = new List<ISlot>();
    public ISlot active;

    public void Init()
    {
        hudHelpers = GetComponentsInChildren<ISlot>().ToList();
        hudHelpers.ForEach(x => x.Close());
    }
    public void Open(string id)
    {
        active?.Close();
        var hud = hudHelpers.Find(x => x.GetId().Equals(id));
        hud.Open();
        active = hud;
    }

    public void SetPriceText(string price)
    {
        active.SetPrice(price);
    }



}
