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
    public int money = 50;
    public int level = 1;
    public int exp = 0;
}

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }
    public UserModel UserModel { get; private set; }
    private const int ModelVersion = 4;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GetUserData();
    }

    private void GetUserData()
    {
        if (PlayerPrefs.HasKey("model_version"))
        {
            var modelVersion = PlayerPrefs.GetInt("model_version");
            if (modelVersion == ModelVersion)
            {
                UserModel = JsonConvert.DeserializeObject<UserModel>(PlayerPrefs.GetString("user"));
                if (UserModel != null) return;
                Debug.LogError("Json convert fail");
                CreateUserData();
            }
            else
            {
                CreateUserData();
            }
        }
        else
        {
            CreateUserData();
        }
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