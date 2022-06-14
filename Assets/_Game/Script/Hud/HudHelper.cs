using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudHelper : MonoBehaviour, ISlot
{
    public string id;
    public TMP_Text text;
    public string GetId() => id;

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetPrice(string currenctPrice)
    {
        text.SetText(currenctPrice);
    }
}

public interface ISlot
{
    string GetId();
    void Open();
    void Close();
    void SetPrice(string price);
}