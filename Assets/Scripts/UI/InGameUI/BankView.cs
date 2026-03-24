using TMPro;
using UnityEngine;

public class BankView : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private TMP_Text _textCristall;
    [SerializeField] private TMP_Text _textPriceBlock;
    [SerializeField] private TMP_Text _textPriceBall;

    private Bank _bank;
    private PurchaseManager _purchaseManager;

    private void OnDestroy()
    {
        if(_bank != null)
        {
            _bank.OnChangedMoney -= ChangeMoney;
            _bank.OnChangedCristall -= ChangeCristall;
        }

        if(_purchaseManager != null)
        {
            _purchaseManager.OnBlockPriceIncreased -= ChangePriceBlock;
            _purchaseManager.OnBallPriceIncreased -= ChangePriceBall;
        }
    }

    public void Initialize(Bank bank, PurchaseManager purchaseManager)
    {
        _bank = bank;
        _purchaseManager = purchaseManager;

        _bank.OnChangedMoney += ChangeMoney;
        _bank.OnChangedCristall += ChangeCristall;
        _purchaseManager.OnBlockPriceIncreased += ChangePriceBlock;
        _purchaseManager.OnBallPriceIncreased += ChangePriceBall;
    }

    private void ChangeMoney(int newMoney)
    {
        ChangeValue(newMoney, _textMoney);
    }

    private void ChangeCristall(int newCristall)
    {
        ChangeValue(newCristall, _textCristall);
    }

    private void ChangePriceBlock(int newPrice)
    {
        ChangeValue(newPrice, _textPriceBlock);
    }

    private void ChangePriceBall(int newPrice)
    {
        ChangeValue(newPrice, _textPriceBall);
    }

    private void ChangeValue(int newValue, TMP_Text text)
    {
        text.text = newValue.ToString();
    }
}
