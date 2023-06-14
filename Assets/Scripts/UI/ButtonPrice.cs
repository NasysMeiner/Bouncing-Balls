using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrice : MonoBehaviour
{
    [SerializeField] private StockBalls _stockBalls;
    [SerializeField] private Button _button;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private Game _game;
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
        if (_game.Money >= _stockBalls.CurrentPriceBall)
            _button.interactable = true;
        else
            _button.interactable = false;
    }

    private void CheckMoneyBlocks()
    {
        if (_game.Money >= _stockBlocks.CurrentPriceBlock)
            _button.interactable = true;
        else
            _button.interactable = false;
    }
}
