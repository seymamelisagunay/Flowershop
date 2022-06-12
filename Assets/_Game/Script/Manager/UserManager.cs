using System;
using Gnarlyteam.Leaderboard;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;
//TODO Currency Sistem için Evirile bilir .
[Serializable]
public class UserModel
{
    public string id;
    public string name;
    public int level = 1;
    public int exp = 0;
}

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }
    public UserModel UserModel;
    public IntVariable money;
    private const int ModelVersion = 4;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetUserData();
    }

    private void GetUserData()
    {
        if (PlayerPrefs.HasKey("user"))
        {
            UserModel = JsonConvert.DeserializeObject<UserModel>(PlayerPrefs.GetString("user"));
            if (UserModel == null)
                CreateUserData();
        }
        else
            CreateUserData();

        GetMoney();
        money.OnChangeVariable.RemoveListener(SaveMoney);
        money.OnChangeVariable.AddListener(SaveMoney);
    }


    public void SaveUser()
    {
        if (UserModel != null)
        {
            PlayerPrefs.SetString("user", JsonConvert.SerializeObject(UserModel));
            PlayerPrefs.Save();
        }
    }

    private void CreateUserData()
    {
        UserModel = new UserModel
        {
            name = $"Player{1000 + Random.Range(15, 500)}",
            id = SystemInfo.deviceUniqueIdentifier,
        };
        SaveUser();
    }
    public bool CheckedMoney(int money)
    {
        return money <= this.money.Value;
    }
    public void IncrementMoney(int count)
    {
        this.money.Value += count;
    }
    public bool DecreasingMoney(int count)
    {
        this.money.Value -= count;
        if (this.money.Value < 0)
        {
            this.money.Value += count;
            return false;
        }
        return true;
    }
    public void SaveMoney()
    {
        PlayerPrefs.SetInt("UserMoney", money.Value);
    }
    public void GetMoney()
    {
        money.Value = PlayerPrefs.GetInt("UserMoney", money.Value);
    }

    public void IncrementExp(int exp)
    {
        UserModel.exp += exp;
        while (UserModel.exp >= UserModel.level * 100)
        {
            UserModel.exp -= UserModel.level * 100;
            UserModel.level++;
        }
        SaveUser();
    }
}