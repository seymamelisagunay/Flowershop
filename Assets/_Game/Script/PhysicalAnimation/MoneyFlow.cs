using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyFlow : MonoBehaviour
{
    public Vector3 target;
    public Vector3 restart;
    public GameObject bankroll;
    public bool moneyFlow;
    public float moneyFlowTime;
    private GameObject _money;
    void Start()
    {
        if (moneyFlow)
        {
            _money = Instantiate(bankroll, restart, Quaternion.identity);
            _money.transform.DOMove(target, moneyFlowTime).SetLoops(-1, LoopType.Restart);
        }
        else
        {
            Destroy(_money);
        }
    }
    
}
