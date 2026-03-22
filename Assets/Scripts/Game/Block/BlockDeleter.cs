using System.Collections;
using UnityEngine;

public class BlockDeleter : MonoBehaviour
{
    [SerializeField] private float _coefficientCell = 0.5f;
    [SerializeField] private StockBlocks _stockBlocks;
    [SerializeField] private ButtonAnimation _buttonAnimation;
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private ShopDistributor _shopDistributor;

    private bool _isUnlock = false;

    public bool IsUnlock => _isUnlock;

    private void OnEnable()
    {
        _shopDistributor.ChangeBuffs += UpgrateSell;
        _playerInfo.CristallChanged += OnChangeCristall;
        _buttonAnimation.Init(_buttonAnimation.Price);
    }

    private void OnDisable()
    {
        _shopDistributor.ChangeBuffs -= UpgrateSell;
        _playerInfo.CristallChanged -= OnChangeCristall;
    }

    public void Unlock()
    {
        if (_playerInfo.Cristall >= _buttonAnimation.Price && _isUnlock == false)
        {
            _playerInfo.ChangeCristall(-(int)_buttonAnimation.Price);
            _isUnlock = true;
            _playerInfo.UnlockBascet(); ;
            _buttonAnimation.PlayAnimation();
            StartCoroutine(UnlockAnimation());
        }
    }

    public void UnlockLoad()
    {
        _isUnlock = true;
        _buttonAnimation.PlayAnimation();
        StartCoroutine(UnlockAnimation());
    }

    public void CellBlock(Block block)
    {
        _playerInfo.ChangeMoney((int)(block.Price * _coefficientCell));

        PoolManager.Instance.SetObject(block, block.ObjectType);
    }

    private void OnChangeCristall(int value)
    {
        if (value >= _buttonAnimation.Price)
            _buttonAnimation.ChangeActiveOn();
        else
            _buttonAnimation.ChangeActiveOff();
    }

    private void UpgrateSell(int id, float value)
    {
        if (id == 5)
        {
            if (_coefficientCell == 0.7f)
                _coefficientCell = 1;
            else
                _coefficientCell += value;
        }
    }

    private IEnumerator UnlockAnimation()
    {
        Vector3 target = new(transform.position.x, transform.position.y, transform.position.z - 0.5f);
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