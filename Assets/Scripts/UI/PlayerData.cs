using Agava.YandexGames;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private ShopDistributor _shopDistributor;
    [SerializeField] private DeleateField _deleateField;

    public int score = 0;
    public int level = 1;
    public float cristall = 0f;
    public bool isUnlockBascket = false;
    public int levelUp = 0;
    public int icon = 0;
    public float money = 0;
    public bool isShowGuide = false;

    public void LoadDataPlayer()
    {
        _game.LoadLevel(level, score, cristall, name, money);
        _shopDistributor.LoadUp(levelUp);

        if(isUnlockBascket)
            _deleateField.UnlockLoad();
    }

    public void ResetData()
    {
        score= 0;
        level = 1;

        if (PlayerAccount.IsAuthorized)
            name = _game.Name;
        else
            name = "Anonymous";

        isUnlockBascket = false;
        levelUp = 0;
        money = 0;
    }
}
