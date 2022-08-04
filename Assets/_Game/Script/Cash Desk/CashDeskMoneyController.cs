using System.Collections;
using _Game.Script.Character;
using NaughtyAttributes;
using UnityEngine;

public class CashDeskMoneyController : MonoBehaviour
{
    [Tag] public string playerTag;
    public GridSlotController moneyGrid;
    [SerializeField] private bool isInPlayer;
    public IntVariable cashDeskMoney;
    private Alarm _soundRepeater;
    private void Start()
    {
        _soundRepeater = new Alarm();
        _soundRepeater.Start(0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        isInPlayer = true;
        var playerController = other.GetComponent<PlayerController>();
        StartCoroutine(GetMoney(playerController));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        isInPlayer = false;
        var playerController = other.GetComponent<PlayerController>();
        StopCoroutine(GetMoney(playerController));
    }

    private IEnumerator GetMoney(PlayerController playerController)
    {
        yield return new WaitForSeconds(playerController.playerSettings.firstTriggerCooldown);
        while (isInPlayer)
        {
            yield return new WaitForSeconds(playerController.playerSettings.pickingSpeed);
            if (cashDeskMoney.Value <= 0)
                continue;

            var gridSlot = moneyGrid.GetSlotObject();
            //Para Artırılacak 
            if (gridSlot != null)
            {
                var isEnough = cashDeskMoney.Value - 10 >= 0;
                if (isEnough)
                {
                    cashDeskMoney.Value -= 10;
                    UserManager.Instance.IncrementMoney(10);
                    gridSlot.isFull = false;
                    var item = gridSlot.slotInObject;
                    item.transform.parent = playerController.transform;
                    item.moveDurationValue = 0.5f;
                    item.Play(Vector3.zero, true);
                    if (_soundRepeater.Check())
                    {
                        SoundManager.instance.Play("money_transfer");
                        _soundRepeater.Start(0.1f);
                    }
                }
                else
                {
                    moneyGrid.ClearAll();
                }
            }
            else
            {
                UserManager.Instance.IncrementMoney(cashDeskMoney.Value);
                cashDeskMoney.Value = 0;
            }
        }
    }
}