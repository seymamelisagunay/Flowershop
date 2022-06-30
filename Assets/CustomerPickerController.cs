using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CustomerPickerController : MonoBehaviour,IPickerController
{
    [Tag]
    public string standTag;
    public IEnumerator GetItem(IItemController itemController)
    {
       yield break;
    }

    public void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
