using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TrashPickerController : MonoBehaviour, IPickerController
{
    [Tag] public string playerTag;
    private bool _isStayPlayer;
    public Transform targetPosition;
    [SerializeField] private float firstTriggerCooldown;
    [SerializeField] private float triggerCooldown;


    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isStayPlayer = true;
        var playerItemController = other.GetComponent<IItemController>();
        StartCoroutine(GetItem(playerItemController));
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        _isStayPlayer = false;
    }

    public IEnumerator GetItem(IItemController itemController)
    {
        var playerItemController = itemController;
        yield return new WaitForSeconds(firstTriggerCooldown);
        while (_isStayPlayer)
        {
            yield return new WaitForSeconds(triggerCooldown);
            var (productType, item, isItemFinish) = playerItemController.GetValue();
            if (!isItemFinish) continue;
            item.transform.parent = null;
            Debug.Log(targetPosition.localPosition + " Global : " + targetPosition.position);
            item.Play(targetPosition.position, true);
        }
    }
}