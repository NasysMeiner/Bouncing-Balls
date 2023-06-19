using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Shop : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private List<Product> _products = new List<Product>();
    [SerializeField] private ProductView _tamplate;
    [SerializeField] private ShopDistributor _shopDistributor;

    private Animator _shopAnimation;
    private bool _isPlay = false;
    private ProductView _currentProduct;
    private int _upgrateBasketId = 5;
    private const string _textStartAnimation = "Play";
    private const string _textExitAnimation = "Exit";

    public bool IsPlay => _isPlay;
    public ProductView Product => _currentProduct;

    public event UnityAction Open;
    public event UnityAction Close;

    private void Start()
    {
        _shopAnimation = GetComponent<Animator>();

        foreach (Product product in _products)
        {
            product.InitProduct(_shopDistributor, _playerInfo);
            Transform currentTransform = product.Place;

            var view = Instantiate(_tamplate, currentTransform);
            product.AddProduct(view);

            if (product.IdProduct == _upgrateBasketId)
                _currentProduct = view;
        }
    }

    public void PlayAnimation()
    {
        if (_isPlay == false)
        {
            _shopAnimation.SetTrigger(_textStartAnimation);
            _isPlay = true;
            Open?.Invoke();
        }
        else
        {
            _shopAnimation.SetTrigger(_textExitAnimation);
            _isPlay = false;
            Close?.Invoke();
        }
    }
}

[System.Serializable]

public class Product
{
    [SerializeField] private Transform _place;
    [SerializeField] private Sprite _icon1;
    [SerializeField] private Sprite _icon2;
    [SerializeField] private Sprite _icon3;
    [SerializeField] private float _price;
    [SerializeField] private float _timeBuffs;
    [SerializeField] private int _idProduct;
    [SerializeField] private float _priceUp;
    [SerializeField] private float _value;
    [SerializeField] private int _maxLevel;
    [SerializeField] private bool _buffsAnim;

    private ShopDistributor _shopDistributor;
    private PlayerInfo _playerInfo;

    public Transform Place => _place;
    public int IdProduct => _idProduct;

    public void InitProduct(ShopDistributor shopDistributor, PlayerInfo playerInfo)
    {
        _shopDistributor = shopDistributor;
        _playerInfo = playerInfo;
    }

    public void AddProduct(ProductView view)
    {
        view.Render(_icon1, _icon2, _icon3, _price, _shopDistributor, _idProduct, _timeBuffs, _buffsAnim, _priceUp, _value, _maxLevel, _playerInfo);
    }
}
