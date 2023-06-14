using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DeleateField : MonoBehaviour
{
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private ButtonAnimation _buttonAnimation;
    [SerializeField] private Game _game;
    [SerializeField] private PlayerData _playerData;

    private bool _isUnlock = false;
    private bool _isPlayAnimation = false;

    public bool IsUnlock => _isUnlock;
    public bool IsPlayAnimation => _isPlayAnimation;

    private void OnEnable()
    {
        _game.CristallChanged += OnChangeCristall;
        _buttonAnimation.Init(_buttonAnimation.Price);
    }

    private void OnDisable()
    {
        _game.CristallChanged -= OnChangeCristall;
    }

    public void Unlock()
    {
        if(_game.Cristall >= _buttonAnimation.Price && _isUnlock == false)
        {
            _game.ChangeCristall(-_buttonAnimation.Price);
            _isUnlock = true;
            _isPlayAnimation = true;
            _playerData.isUnlockBascket = true;
            _buttonAnimation.PlayAnimation();
        }
    }

    public void UnlockLoad()
    {
        _isUnlock = true;
        _isPlayAnimation = true;
        _buttonAnimation.PlayAnimation();
    }

    private void OnChangeCristall(float value)
    {
        if (value >= _buttonAnimation.Price)
            _buttonAnimation.ChangeActive(true);
        else
            _buttonAnimation.ChangeActive(false);
    }
}