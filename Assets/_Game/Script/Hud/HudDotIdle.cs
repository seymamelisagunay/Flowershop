using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class HudDotIdle : MonoBehaviour
{
    public GameObject[] closeObjects;
    public TMP_Text threeDot;

    [Button()]
    public void ThreeDotAnim()
    {
        threeDot.text = "";
        foreach (var closeObject in closeObjects)
        {
            closeObject.SetActive(false);
            
        }
        StartCoroutine(Enumerator());
    }
    IEnumerator Enumerator()
    {
        for (int i = 1; i < 10; i++)
        {
            threeDot.text += '.';
            yield return new WaitForSeconds(0.3f);
            if (i % 3 == 0)
            {
                threeDot.text = "";
                yield return new WaitForSeconds(0.3f);
            }
                
        }
    }
}
