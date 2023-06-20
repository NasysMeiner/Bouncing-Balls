using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductView : MonoBehaviour
{
    [SerializeField] private Image _icon1;
    [SerializeField] private Image _icon2;
    [SerializeField] private Image _icon3;
    [SerializeField] private Image _icon4;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;
    [SerializeField] private ProductButton _productButton;

    private ShopDistributor _shopDistributor;
    private PlayerInfo _playerInfo;
    private int _idProduct;
    private float _buffsTime;
    private float _price;
    private float _upPrice;
    private float _value;
    private int _maxLevel;
    private int _curentLevel = 1;
    private bool _isPressed = false;

    public float Price => _price;
    public ProductButton ProductButton => _productButton;

    private void OnDisable()
    {
        _playerInfo.CristallChanged -= ChangeButton;
    }

    public void Render(Sprite image1, Sprite image2, Sprite image3, float price, ShopDistributor shopDistributor, int idProduct, float time, bool buffs, float upPrice, float value, int maxLevel, PlayerInfo playerInfo)
    {
        _icon1.sprite = image1;
        _icon2.sprite = image2;
        _icon3.sprite = image3;
        _icon4.sprite = image3;
        _playerInfo = playerInfo;
        _value = value;
        _text.text = price.ToString();
        _shopDistributor = shopDistributor;
        _playerInfo.CristallChanged += ChangeButton;
        _price = price;
        _idProduct = idProduct;
        _buffsTime = time;
        _upPrice = upPrice;
        _maxLevel = maxLevel;
        ChangeButton(_playerInfo.Cristall);
        _productButton.InitButton(buffs, this, time);
    }

    public void ClickMouse()
    {
        Debug.Log(_buffsTime + "buffs");

        if (_isPressed == false)
            _shopDistributor.Buff(_idProduct, _price, this, _value, _buffsTime);
    }

    public void PriceUp()
    {
        _price += _upPrice + (int)(_price / 2);
        _text.text = _price.ToString();
    }

    public void LevelUp()
    {
        _curentLevel += 1;
    }

    public void EndAnimation()
    {
        ChangeButton(_playerInfo.Cristall);
    }

    private void ChangeButton(int cristall)
    {
        if (cristall >= _price && _productButton.IsBuff == false && _curentLevel < _maxLevel)
        {
            _button.interactable = true;
            _isPressed = false;
        }
        else
        {
            _button.interactable = false;
        }
    }
}
