using System.Collections;
using System.Collections.Generic;
using _Game.Script.Character;
using _Game.Script.Manager;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using NaughtyAttributes;
using UnityEngine;

public class SlotEmptyController : MonoBehaviour, ISlotController
{
    [Header("Unlock Duration Related")]
    [SerializeField]
    private float
        baseUnlockDuration =
            0.01f; // for every unit of cost that smaller than threshold, use this value for duration calculation. 

    [SerializeField]
    private float
        additionalUnlockDuration =
            0.01f; // for every unit of cost that bigger than threshold, use this value for duration calculation.

    [SerializeField]
    private int slowerUnlockDurationThreshold = 100; // every unity bigger than this one will be marked ass additional.

    [Space]
    [Header("Money Transfer Related")]
    [SerializeField]
    private float moneyTransferSpeed;

    [SerializeField] private float moneyTransferInterval;
    [SerializeField] private AnimationCurve moneyMovementCurve;

    [SerializeField] private int moneyStackSize;

    private const float firstTimeCooldown = 0.1f;

    private SlotController _slotController;
    private bool _isInsidePlayer;
    [ReadOnly] public SlotEmptyData emptyData;

    private PlayerController _playerController;
    private List<GameObject> _moneyStack = new();
    private Tweener _moneyTweener;
    private Coroutine _oldCouroutine;

    private Alarm _soundRepeater;
    public GameObject shelverIcon;
    public GameObject cashierIcon;


    public void Init(SlotController slotController)
    {
        _slotController = slotController;
        emptyData = _slotController.slot.emptyData;
        SlotOpenEffect();
        _slotController.slotHud.Open("empty");
        var remaining = _slotController.slot.emptyData.Price - _slotController.slot.emptyData.CurrenctPrice;
        _slotController.slotHud.SetPriceText(remaining.ToString());
        _slotController.slot.emptyData.OnChangeVariable.AddListener((data) =>
        {
            var remaingCount = data.Price - data.CurrenctPrice;
            _slotController.slotHud.SetPriceText(remaingCount.ToString());
        });

        // Create money pool
        var moneyPrefab = GameManager.instance.itemList.GetItemPrefab(ItemType.Money);
        for (var i = 0; i < moneyStackSize; i++)
        {
            var money = Instantiate(moneyPrefab).gameObject;
            money.SetActive(false);
            _moneyStack.Add(money);
        }

        switch (_slotController.slot.slotType)
        {
            case SlotType.Shelver:
                shelverIcon.SetActive(true);
                cashierIcon.SetActive(false);
                break;
            case SlotType.Cashier:
                shelverIcon.SetActive(false);
                cashierIcon.SetActive(true);
                break;
            default:
                shelverIcon.SetActive(false);
                cashierIcon.SetActive(false);
                break;
        }
        _soundRepeater = new Alarm();
        _soundRepeater.Start(0.2f);
    }

    private void SlotOpenEffect()
    {
        var parent = transform.parent;
        parent.localScale = Vector3.zero;
        parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_slotController.slot.emptyData.IsOpen)
        {
            var clonePlayer = other.GetComponent<PlayerController>();

            if (clonePlayer.playerSettings.isBot) return;
            _playerController = clonePlayer;
            _isInsidePlayer = true;
            _oldCouroutine = StartCoroutine(StayInPlayer(_playerController));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var clonePlayer = other.GetComponent<PlayerController>();
            if (clonePlayer.playerSettings.isBot) return;
            _isInsidePlayer = false;
        }
    }

    private IEnumerator StayInPlayer(PlayerController player)
    {
        yield return new WaitUntil(() => !player.characterController.inMotion && _isInsidePlayer);
        Debug.Log("Test Geçtik !");
        yield return new WaitForSeconds(firstTimeCooldown);

        // Calculate unlock duration
        var remainingCost = emptyData.Price - emptyData.CurrenctPrice;
        var duration = remainingCost * baseUnlockDuration;
        if (remainingCost > slowerUnlockDurationThreshold)
        {
            var additionalCost = remainingCost - 100;
            duration += additionalCost * additionalUnlockDuration;
        }

        _moneyTweener = DOVirtual.Int(remainingCost, 0, duration, value =>
        {
            if (!_isInsidePlayer) return;
            if (UserManager.Instance.money.Value == 0)
            {
                _isInsidePlayer = false;
                return;
            }

            var moneyAmountToDecrease = GetDecreaseAmount(remainingCost, value);

            var result = UserManager.Instance.DecreasingMoney(moneyAmountToDecrease);
            if (!result)
            {
                _isInsidePlayer = false;
                return;
            }
            if (_soundRepeater.Check())
            {
                SoundManager.instance.Play("money_transfer");
                _soundRepeater.Start(0.1f);
                MMVibrationManager.Haptic(HapticTypes.Selection, false, true, this);
            }
            emptyData.CurrenctPrice += moneyAmountToDecrease;
            // _slotHud.emptyPrice.SetText(moneyCounter.ToString());
            if (emptyData.CurrenctPrice < emptyData.Price) return;
            ActivateSlot();
        });

        while (_isInsidePlayer)
        {
            if (!_isInsidePlayer) break;
            if (emptyData.CurrenctPrice == emptyData.Price) break;

            yield return new WaitForSeconds(moneyTransferInterval);
            TransferMoney();
        }

        _moneyTweener.Kill();
        //
    }

    private void TransferMoney()
    {
        var money = GetMoney();
        money.SetActive(true);
        money.transform.position = _playerController.transform.position;

        DOVirtual.Float(0, 1, moneyTransferSpeed, value =>
            {
                money.transform.position = Vector3.Lerp(money.transform.position, transform.position, value)
                                           + new Vector3(0, moneyMovementCurve.Evaluate(value), 0f);
            }).OnComplete(() => money.SetActive(false))
            .SetLink(money, LinkBehaviour.KillOnDestroy);
    }

    private void ActivateSlot()
    {
        _isInsidePlayer = false;
        emptyData.IsOpen = true;
        SlotManager.instance.NextSlot();
        SlotClose();
        _slotController.OpenSlot();
        _slotController.slotHud.active.Close();

        // Clear created money stack
        foreach (var money in _moneyStack)
        {
            Destroy(money);
        }

        _moneyStack.Clear();
    }

    private int GetDecreaseAmount(int previousLeftCost, int value)
    {
        var moneyAmountToDecrease = previousLeftCost - value;
        var userMoney = UserManager.Instance.money.Value;

        if (moneyAmountToDecrease > userMoney) moneyAmountToDecrease = userMoney;

        var paidAmountAfterCalculation = emptyData.CurrenctPrice + moneyAmountToDecrease;
        var remainingCost = emptyData.Price - emptyData.CurrenctPrice;

        if (paidAmountAfterCalculation > emptyData.Price) moneyAmountToDecrease = remainingCost;

        return moneyAmountToDecrease;
    }

    private GameObject GetMoney()
    {
        foreach (var money in _moneyStack)
        {
            if (!money.activeSelf) return money;
        }
        var newMoney = Instantiate(GameManager.instance.itemList.GetItemPrefab(ItemType.Money)).gameObject;
        _moneyStack.Add(newMoney);
        return newMoney;
    }

    private void SlotClose()
    {
        transform.GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
    }
}

public interface ISlotController
{
    public void Init(SlotController slotController);
}