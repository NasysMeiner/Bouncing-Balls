using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeleter : MonoBehaviour
{
    [SerializeField] private float _zAddPosition = 0.5f;
    [SerializeField] private float _startCoeffSell = 0.5f;
    [SerializeField] private List<float> _coeffSellPerLevel = new();

    private Bank _bank;
    private PurchaseManager _purchaseManager;

    private float _currentCoeffSell;
    private bool _isUnlock = false;

    public bool IsUnlock => _isUnlock;

    public void Initialize(Bank bank, PurchaseManager purchaseManager)
    {
        _bank = bank;
        _purchaseManager = purchaseManager;

        _currentCoeffSell = _startCoeffSell;
    }

    public void Unlock()
    {
        if (_isUnlock)
            return;

        _isUnlock = true;
        StartCoroutine(UnlockAnimation());
    }

    public void CellBlock(Block block)
    {
        block.ChangeCell(null);
        _bank.AddMoney((int)(_purchaseManager.CurrentPriceBlock * _currentCoeffSell));
        PoolManager.Instance.SetObject(block, block.ObjectType);
    }

    public void SetNewCoeffSell(int newLevel)
    {
        if (newLevel >= _coeffSellPerLevel.Count)
            return;

        _currentCoeffSell = _coeffSellPerLevel[newLevel];
    }

    private IEnumerator UnlockAnimation()
    {
        Vector3 target = new(transform.position.x, transform.position.y, transform.position.z - _zAddPosition);
        bool isWork = true;
        int speedAnimation = 10;

        while (isWork)
        {
            if(transform.position == target)
                isWork = false;

            transform.position = Vector3.MoveTowards(transform.position, target, speedAnimation * Time.deltaTime);

            yield return null;
        }
    }
}