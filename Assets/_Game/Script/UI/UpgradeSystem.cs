using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UpgradeSystem : MonoBehaviour
{
    public TMP_Text playerUpgrade;
    public TMP_Text carryUpgrade;
    public TMP_Text shelverUpgrade;
    public TMP_Text perfumeUpgrade;
    public TMP_Text delightUpgrade;

    enum PlayerUpgradeMoney { Lvl1 = 100, Lvl2 = 300, Lvl3 = 500, Lvl4 = 1000, Lvl5 = 10000 };
    enum CarryUpgradeMoney { Lvl1 = 200, Lvl2 = 600, Lvl3 = 1500, Lvl4 = 3000 };
    enum ShelverUpgradeMoney { Lvl1 = 1500, Lvl2 = 3000, Lvl3 = 6000, Lvl4 = 12000};
    enum PerfumeUpgradeMoney { Lvl1 = 200, Lvl2 = 600, Lvl3 = 1500};
    enum DelightUpgradeMoney { Lvl1 = 300, Lvl2 = 900, Lvl3 = 2100};
    
    private string _upgradeLevelName = "Upgrade Lvl. ";

    private int playerUpgradeLevel = 0;
    private int _carryUpgradeLevel = 0;
    private int _shelverUpgradeLevel = 0;
    private int _perfumeUpgradeLevel = 0;
    private int _delightUpgradeLevel = 0;
    
    public void PlayerUpgrade()
    {
        
        UserManager.Instance.DecreasingMoney(20);
        playerUpgrade.text = _upgradeLevelName ;
    }

    public void CarryUpgrade()
    {
        UserManager.Instance.DecreasingMoney(20);
        carryUpgrade.text = "sdgsdg";
    }
    
    public void ShelverUpgrade()
    {
        
    }

    public void PerfumeUpgrade()
    {
        
    }

    public void DelightUpgrade()
    {
        
    }
}
