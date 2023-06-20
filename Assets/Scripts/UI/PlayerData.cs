using Agava.YandexGames;
using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private DeleteField _deleateField;

    private string _nameLeaderboard = "NewLeaders";

    public int score = 0;
    public int level = 1;
    public int cristall = 0;
    public bool isUnlockBascet = false;
    public int levelUp = 0;
    public int icon = 0;
    public int money = 0;
    public bool isShowGuide = false;
    public string name = "Anonymos";

    public void WriteDataPlayer(int score, int level, int cristall, bool isUnlockBascet, int levelUp, int icon, int money, bool isShowGuide, string name, bool isEnd = false)
    {
        if (isEnd)
        {
            if (!_levelLoader.isUnity)
                ResetData();
        }
        else
        {
            this.score = score;
            this.level = level;
            this.levelUp = levelUp;
            this.cristall = cristall;
            this.icon = icon;
            this.money = money;
            this.isShowGuide = isShowGuide;
            this.isUnlockBascet = isUnlockBascet;
            this.name = name;
        }

        OnSetCloudSaveData();
    }

    public void ResetData()
    {
        score = 0;
        level = 1;

        if (PlayerAccount.IsAuthorized && !_levelLoader.isUnity)
            name = _playerInfo.Name;
        else
            name = "Anonymous";

        isUnlockBascet = false;
        levelUp = 0;
        money = 0;
    }

    public void CheckPlayerName()
    {
        if (PlayerAccount.IsAuthorized)
        {
            Agava.YandexGames.Leaderboard.GetPlayerEntry(_nameLeaderboard, (result) =>
            {
                string name = result.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymos";

                _playerInfo.SetName(name);
            });
        }
    }

    public void LoadPlayerData(string loadedString)
    {
        if (!_levelLoader.isUnity)
        {
            if (loadedString != "{}")
            {
                try
                {
                    PlayerDataPack playerData = JsonUtility.FromJson<PlayerDataPack>(loadedString);
                    score = playerData.score;
                    level = playerData.level;
                    name = playerData.name;
                    CheckPlayerName();

                    if (level == 0)
                        level = 1;
                    
                    levelUp = playerData.levelUp;
                    cristall = playerData.cristall;
                    isUnlockBascet = playerData.isUnlockBasket;
                    icon = playerData.icon;
                    isShowGuide = playerData.isShowGuide;
                    money = playerData.money;
                    _playerInfo.LoadLevelData(this);
                }
                catch
                {
                    OnSetCloudSaveData();
                }
            }
        }
    }

    private void OnSetCloudSaveData()
    {
        if (!_levelLoader.isUnity)
        {
            PlayerDataPack playerDataPack = new PlayerDataPack(score, level, cristall, isUnlockBascet, levelUp, icon, money, isShowGuide, name);
            string jsonString = JsonUtility.ToJson(playerDataPack);
            PlayerAccount.SetCloudSaveData(jsonString);
        }
    }
}

[System.Serializable]
public class PlayerDataPack
{
    public int score = 0;
    public int level = 1;
    public int cristall = 0;
    public bool isUnlockBasket = false;
    public int levelUp = 0;
    public int icon = 0;
    public int money = 0;
    public bool isShowGuide = false;
    public string name = "Anonymos";

    public PlayerDataPack(int score, int level, int cristall, bool isUnlockBasket, int levelUp, int icon, int money, bool isShowGuide, string name)
    {
        this.score = score;
        this.level = level;
        this.cristall = cristall;
        this.isUnlockBasket = isUnlockBasket;
        this.levelUp = levelUp;
        this.icon = icon;
        this.money = money;
        this.isShowGuide = isShowGuide;
        this.name = name;
    }
}
