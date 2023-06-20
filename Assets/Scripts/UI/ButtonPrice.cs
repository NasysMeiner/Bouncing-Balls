using UnityEngine;
using UnityEngine.UI;

public class ButtonPrice : MonoBehaviour
{
    [SerializeField] private StockBalls _stockBalls;
    [SerializeField] private Button _button;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private bool _isBallButton;

    private void Update()
    {
        if (_isBallButton)
            CheckMoneyBalls();
        else
            CheckMoneyBlocks();
    }

    private void CheckMoneyBalls()
    {
        if (_playerInfo.Money >= _stockBalls.CurrentPriceBall)
            _button.interactable = true;
        else
            _button.interactable = false;
    }

    private void CheckMoneyBlocks()
    {
        if (_playerInfo.Money >= _stockBlocks.CurrentPriceBlock)
            _button.interactable = true;
        else
            _button.interactable = false;
    }
}
