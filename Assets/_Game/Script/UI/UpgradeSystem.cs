using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public GameObject moneyBox;
    public IntVariable money;
    public TMP_Text playerUpgradeLevelName;
    public TMP_Text playerUpgradeMoney;
    public TMP_Text shelverUpgradeLevelName;
    public TMP_Text shelverUpgradeMoney;
    public TMP_Text carryUpgradeLevelName;
    public TMP_Text carryUpgradeMoney;
    public TMP_Text perfumeUpgradeLevelName;
    public TMP_Text perfumeUpgradeMoney;
    public TMP_Text delightUpgradeLevelName;
    public TMP_Text delightUpgradeMoney;

    //upgrade ucretleri
    public int[] PlayerUpgradeMoneyArray = {100, 300, 500, 1000, 10000};
    public int[] ShelverUpgradeMoneyArray = {200, 600, 1500, 3000};
    public int[] CarryUpgradeMoneyArray = {1500, 3000, 6000, 12000};
    public int[] PerfumeUpgradeMoneyArray = {200, 600, 1500};
    public int[] DelightUpgradeMoneyArray = {300, 900, 2100};
    public Tweener ShakeTweener;
    private string _upgradeLevelName = "Upgrade Lvl. ";

    //bu kismmi kaydetmem lazim***. upgrade seviyelerimizi tutucak
    private int _playerUpgradeLevel = 1;
    private int _carryUpgradeLevel = 1;
    private int _shelverUpgradeLevel = 1;
    private int _perfumeUpgradeLevel = 1;
    private int _delightUpgradeLevel = 1;

    public void Start() //baslangicta level ismimizi ve upgrade paralarini atiyoruz 
    {
        money.OnChangeVariable.AddListener(MoneyShake);
        // playerUpgradeLevelName.text = _upgradeLevelName + _playerUpgradeLevel;
        // playerUpgradeMoney.text = PlayerUpgradeMoneyArray[_playerUpgradeLevel - 1].ToString();
        // shelverUpgradeLevelName.text = _upgradeLevelName + _shelverUpgradeLevel;
        // shelverUpgradeMoney.text = ShelverUpgradeMoneyArray[_shelverUpgradeLevel - 1].ToString();
        // carryUpgradeLevelName.text = _upgradeLevelName + _carryUpgradeLevel;
        // carryUpgradeMoney.text = CarryUpgradeMoneyArray[_carryUpgradeLevel - 1].ToString();
        // perfumeUpgradeLevelName.text = _upgradeLevelName + _perfumeUpgradeLevel;
        // perfumeUpgradeMoney.text = PerfumeUpgradeMoneyArray[_perfumeUpgradeLevel - 1].ToString();
        // delightUpgradeLevelName.text = _upgradeLevelName + _delightUpgradeLevel;
        // delightUpgradeMoney.text = DelightUpgradeMoneyArray[_delightUpgradeLevel - 1].ToString();
    }

    public int UpgradeButton(int[] UpgradeMoneyArray, int upgradeLevel, TMP_Text upgradeLevelName,
        TMP_Text upgradeMoney)
    {
        int money = UpgradeMoneyArray[upgradeLevel - 1];
        if (UserManager.Instance.CheckedMoney(money)) // paramiz varsa upgrade islemlerini yapicaz
        {
            if (upgradeLevel >= UpgradeMoneyArray.Length)
            {
                UserManager.Instance.DecreasingMoney(money);
                upgradeMoney.GetComponentInParent(typeof(Button)).gameObject
                    .SetActive(false); //eger tum upgradeler yapilirsa butonu gorunmez yapiyoruz
                upgradeLevelName.text = "Upgrade Lvl. Max";
            }
            else
            {
                UserManager.Instance.DecreasingMoney(money); //odeme yaptik
                upgradeLevelName.text = "Upgrade Lvl. " + (upgradeLevel + 1); //level ismimizi guncelledik
                upgradeMoney.text = UpgradeMoneyArray[upgradeLevel].ToString(); //paramizi guncelledik
                Debug.Log(upgradeLevel);
                upgradeLevel += 1; //bu kismmi kaydetmem lazim***.
            }
        }
        else
        {
            MoneyShake(); //paramiz yoksa para kutucugunu titrestiriyoruz
        }

        return upgradeLevel;
    }

    public void MoneyShake()
    {
        if (ShakeTweener != null )
        {
            if (ShakeTweener.IsActive())
            {
                return;
            }
        }
        ShakeTweener = moneyBox.transform.DOShakeScale(0.5f, 0.2f, 10, 90f, false);
    }

    public void PlayerUpgrade()
    {
        _playerUpgradeLevel = UpgradeButton(PlayerUpgradeMoneyArray, _playerUpgradeLevel, playerUpgradeLevelName,
            playerUpgradeMoney);
    }

    public void ShelverUpgrade()
    {
        _shelverUpgradeLevel = UpgradeButton(ShelverUpgradeMoneyArray, _shelverUpgradeLevel, shelverUpgradeLevelName,
            shelverUpgradeMoney);
    }

    public void CarryUpgrade()
    {
        _carryUpgradeLevel = UpgradeButton(CarryUpgradeMoneyArray, _carryUpgradeLevel, carryUpgradeLevelName,
            carryUpgradeMoney);
    }

    public void PerfumeUpgrade()
    {
        _perfumeUpgradeLevel = UpgradeButton(PerfumeUpgradeMoneyArray, _perfumeUpgradeLevel, perfumeUpgradeLevelName,
            perfumeUpgradeMoney);
    }

    public void DelightUpgrade()
    {
        _delightUpgradeLevel = UpgradeButton(DelightUpgradeMoneyArray, _delightUpgradeLevel, delightUpgradeLevelName,
            delightUpgradeMoney);
    }
}