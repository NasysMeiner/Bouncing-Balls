using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ShopDistributor : MonoBehaviour
{
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private DeleteField _deleteField;
    [SerializeField] private Shop _shop;

    public event UnityAction<int, float> ChangeBuffs;

    public void Buff(int id, float price, ProductView productView, float value, float timeBuffs = 0)
    {
        int premiumBlockId = 4;

        if (_playerInfo.Cristall >= price)
        {
            if (id == premiumBlockId && _stockBlocks.TryEmptyCell() == null)
            {
                return;
            }

            if (timeBuffs > 0)
            {
                StartCoroutine(WaitTime(id, timeBuffs, productView.ProductButton, value));
            }
            else
            {
                _playerInfo.LevelUpBascet();
                ChangeBuffs?.Invoke(id, value);
                productView.LevelUp();
            }

            productView.PriceUp();
            _playerInfo.ChangeCristall(-(int)price);
        }
    }

    public void LoadUp(int value, bool isOpenBasket)
    {
        if(isOpenBasket)
        {
            _deleteField.UnlockLoad();
        }

        int upgrateBasketId = 5;
        int price = 0;
        float changeUpgrate = 0.2f;

        for (int i = 0; i < value; i++)
        {
            Buff(upgrateBasketId, price, _shop.Product, changeUpgrate);
        }
    }

    private IEnumerator WaitTime(int id, float time, ProductButton productButton, float value)
    {
        productButton.PlayAnimation();
        ChangeBuffs?.Invoke(id, value);

        yield return new WaitForSeconds(time);

        productButton.PlayAnimation();
        ChangeBuffs?.Invoke(id, -value);
    }
}
