using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private CooldownButton _profabilityButton;
    [SerializeField] private CooldownButton _forceBounceButton;
    [SerializeField] private CooldownButton _cristallChanceButton;
    [SerializeField] private CooldownButton _timeBlockButton;
    [SerializeField] private CooldownButton _unlockBlockDeleterButton;
    [SerializeField] private CooldownButton _levelUpBlocDeleterButton;

    private const string _textStartAnimation = "Play";
    private const string _textExitAnimation = "Exit";

    private BuffController _buffController;
    private GameManager _gameManager;

    private Animator _shopAnimation;

    private bool _isPlay;
    private List<IInitializeButton> _buttons = new();

    private void OnDestroy()
    {
        if(_gameManager != null)
        {
            _gameManager.OnEndLevel -= OnEndLevel;
        }
    }

    public void Initialize(BuffController buffController, GameManager gameManager)
    {
        _buffController = buffController;
        _gameManager = gameManager;

        _shopAnimation = GetComponent<Animator>();

        _profabilityButton.Initialize(_buffController.ProfitabilityBuffCrystalPrice);
        _forceBounceButton.Initialize(_buffController.ForceBounceBuffCrystalPrice);
        _cristallChanceButton.Initialize(_buffController.CristallChanceBuffCrystalPrice);
        _timeBlockButton.Initialize(_buffController.TimeBlockCrystalPrice);
        _unlockBlockDeleterButton.Initialize(_buffController.BlockDeleterPrice);
        _levelUpBlocDeleterButton.Initialize(_buffController.CurrentPriceLevelUpBlockDeleter);

        _buttons.Add(_profabilityButton);
        _buttons.Add(_forceBounceButton);
        _buttons.Add(_cristallChanceButton);
        _buttons.Add(_timeBlockButton);

        _gameManager.OnEndLevel += OnEndLevel;
    }

    public void TimeUpdateProfability(CooldownButton uIButton)
    {
        if(_buffController.TryProfitabilityBuff(out float coolDown))
            uIButton.SetTimeInactive(coolDown);
    }

    public void TimeUpdateForceBounce(CooldownButton uIButton)
    {
        if (_buffController.TryForceBounceBuff(out float coolDown))
            uIButton.SetTimeInactive(coolDown);
    }

    public void TimeUpdateCristallChance(CooldownButton uIButton)
    {
        if (_buffController.TryCristallChanceBuff(out float coolDown))
            uIButton.SetTimeInactive(coolDown);
    }

    public void TimeBlockCreate(CooldownButton uIButton)
    {
        if (_buffController.TryTimeBlockCreate(out float coolDown))
            uIButton.SetTimeInactive(coolDown);
    }

    public void UnlockBlockDeleter(CooldownButton uIButton)
    {
        if (_buffController.TryBlockDeleterUnlock())
            uIButton.OffButton();
    } 

    public void BlockDeleterLevelUp(CooldownButton uIButton)
    {
        if (_buffController.TryBlockDeleterUp())
        {
            if(_buffController.IsMaxLevelBlockDeleter)
            {
                uIButton.OffButton();
                return;
            }

            uIButton.Initialize(_buffController.CurrentPriceLevelUpBlockDeleter);
            uIButton.SetTimeInactive();
        }
    }

    public void PlayAnimation()
    {
        if (_isPlay == false)
        {
            _shopAnimation.SetTrigger(_textStartAnimation);
            _isPlay = true;
        }
        else
        {
            _shopAnimation.SetTrigger(_textExitAnimation);
            _isPlay = false;
        }
    }

    private void OnEndLevel()
    {
        if (_isPlay)
            PlayAnimation();

        foreach (var button in _buttons)
            button.ResetButton();
    }
}