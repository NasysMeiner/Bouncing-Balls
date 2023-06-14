using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ShopDistributor : MonoBehaviour
{
    //[SerializeField] private float _buffsForceFactore = 2;
    //[SerializeField] private int _upNumberBalls = 5;
    [SerializeField] private Game _game;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Shop _shop;

    public event UnityAction<int, float> ChangeBuffs;

    public void Buff(int id, float price, ProductView productView, float value, float timeBuffs = 0)
    {
        if (_game.Cristall >= price)
        {
            if(id == 4 && _stockBlocks.TryEmptyCell() == null)
            {
                return;
            }

            if(timeBuffs> 0)
            {
                StartCoroutine(WaitTime(id, timeBuffs, productView.ProductButton, value));
            }
            else
            {
                _playerData.levelUp += 1;
                ChangeBuffs?.Invoke(id, value);
                productView.LevelUp();
            }

            productView.PriceUp();
            _game.ChangeCristall(-price);
        }
    }

    public void LoadUp(int value)
    {
        for (int i = 0; i < value; i++)
        {
            Buff(5, 0, _shop.Product, 0.2f);
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
