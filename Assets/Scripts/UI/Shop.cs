using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Shop : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private List<Product> _products = new List<Product>();
    [SerializeField] private ProductView _tamplate;
    [SerializeField] private ShopDistributor _shopDistributor;

    private Animator _shopAnimation;
    private bool _isPlay = false;
    private ProductView _currentProduct;

    public bool IsPlay => _isPlay;
    public ProductView Product => _currentProduct;

    public event UnityAction<bool> Open;
    public event UnityAction<bool> Close;

    private void Start()
    {
        _shopAnimation = GetComponent<Animator>();

        foreach (Product product in _products)
        {
            product.InitProduct(_shopDistributor, _game);
            Transform currentTransform = product.Place;

            var view = Instantiate(_tamplate, currentTransform);
            product.AddProduct(view);

            if(product.IdProduct == 5)
                _currentProduct = view;
        }
    }

    public void PlayAnimation()
    {
        if (_isPlay == false)
        {
            _shopAnimation.SetTrigger("Play");
            _isPlay = true;
            Open?.Invoke(false);
        }
        else
        {
            _shopAnimation.SetTrigger("Exit");
            _isPlay = false;
            Close?.Invoke(true);
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
    [SerializeField] private bool _upAnim;

    private ShopDistributor _shopDistributor;
    private Game _game;

    public Transform Place => _place;
    public int IdProduct => _idProduct;

    public void InitProduct(ShopDistributor shopDistributor, Game game)
    {
        _shopDistributor = shopDistributor;
        _game = game;
    }

    public void AddProduct(ProductView view)
    {
        view.Render(_icon1, _icon2, _icon3, _price, _shopDistributor, _idProduct, _timeBuffs, _game, _buffsAnim, _upAnim, _priceUp, _value, _maxLevel);
    }
}
