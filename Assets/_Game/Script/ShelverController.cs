using System;
using System.Collections;
using _Game.Script.Bot;
using _Game.Script.Core.Character;
using _Game.Script.Manager;
using UnityEngine;
using UnityEngine.AI;

public class ShelverController : MonoBehaviour
{
    public PlayerSettings shelverSettings;

    public PlayerPickerController pickerController;
    public PlayerItemController itemController;

    private SlotController _activeSlot;
    private StandController _activeStand;
    private IInput _input;

    private NavMeshPath _path;
    private int _pathIndex;


    private void Start()
    {
        _input = GetComponent<IInput>();
        _input.StartListen();
        pickerController = GetComponent<PlayerPickerController>();
        itemController = GetComponent<PlayerItemController>();
        itemController.stackData = pickerController.playerStackData;

        StartCoroutine(BotProgress());
    }

    public IEnumerator BotProgress()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //select Active Slots
            var slots = SlotManager.instance.GetActiveFarmAndFactory();
            if (slots.Count == 0) continue;
            var selectedSlot = slots.RandomSelectObject();
            if (selectedSlot.slot.stackData.ProductTypes.Count <= 0) continue;
            var gridSlot = new GridSlot();
            _activeSlot = selectedSlot;
            switch (selectedSlot.slot.slotType)
            {
                case SlotType.Farm:
                    var farmController = _activeSlot.GetComponentInChildren<FarmController>();
                    gridSlot = farmController.GetCustomerSlot();
                    break;
                case SlotType.Factory:
                    var factoryController = _activeSlot.GetComponentInChildren<FactoryController>();
                    gridSlot = factoryController.GetCustomerSlot();
                    break;
            }

            // Calculate Path
            _path = new NavMeshPath();
            _pathIndex = 1;
            GameManager.instance.NavMesh.CalculatePath(transform.position, gridSlot.transform.position, _path);
            yield return MoveToPoint(_path);
            Debug.Log("Test !");
            yield return new WaitForSeconds(1);

            yield return PickItem();
            // 
            Debug.Log("Burda İtem Alındı ve itemi ");

            yield return new WaitForSeconds(1);
            var itemType = pickerController.playerStackData.ProductTypes[0];
            var slotController = SlotManager.instance.GetActiveStand(itemType);
            _activeStand = slotController.GetComponentInChildren<StandController>();
            var standGridSlot = _activeStand.GetCustomerSlot();
            _path = new NavMeshPath();
            _pathIndex = 1;
            GameManager.instance.NavMesh.CalculatePath(transform.position, standGridSlot.transform.position, _path);
            yield return MoveToPoint(_path);
            IPickerController picker = null;
            switch (itemType)
            {
                case ItemType.Rose:
                    picker = _activeStand.GetComponent<RoseStandPickerController>();
                    break;
                default:
                    picker = _activeStand.GetComponent<RosePerfumeStandPickerController>();
                    break;
            }
            yield return picker.GetItem(itemController);
        }
    }

    private IEnumerator PickItem()
    {
        IItemController itemController = null;
        switch (_activeSlot.slot.slotType)
        {
            case SlotType.Farm:
                var farmController = _activeSlot.GetComponentInChildren<FarmController>();
                itemController = (IItemController) farmController;
                break;
            case SlotType.Factory:
                var factoryController = _activeSlot.GetComponentInChildren<FactoryController>();
                itemController = factoryController.itemController;
                break;
        }

        pickerController.SetStay(true);
        yield return pickerController.GetItem(itemController);
        Debug.Log("Test !  itemler Alındı !");
    }


    private IEnumerator MoveToPoint(NavMeshPath path)
    {
        while (path.corners.Length != _pathIndex)
        {
            yield return new WaitForSeconds(0.1f);
            if (_pathIndex >= path.corners.Length) continue;

            if (path.corners.Length > 0)
            {
                var direction = path.corners[_pathIndex] - transform.position;
                _input.SetDirection(direction.normalized);
                if (direction.magnitude < 0.5f)
                    _pathIndex++;
            }
        }

        _input.ClearDirection();
    }
}