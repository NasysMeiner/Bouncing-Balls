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
    private Game _game;
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
        _game.CristallChanged -= ChangeButton;
    }

    public void Render(Sprite image1, Sprite image2, Sprite image3, float price, ShopDistributor shopDistributor, int idProduct, float time, Game game, bool buffs, bool up, float upPrice, float value, int maxLevel)
    {
        _icon1.sprite = image1;
        _icon2.sprite = image2;
        _icon3.sprite = image3;
        _icon4.sprite = image3;
        _value = value;
        _text.text = price.ToString();
        _shopDistributor = shopDistributor;
        _game = game;
        _game.CristallChanged += ChangeButton;
        _price = price;
        _idProduct = idProduct;
        _buffsTime = time;
        _upPrice = upPrice;
        _maxLevel = maxLevel;
        ChangeButton(_game.Cristall);
        _productButton.InitButton(buffs, up, this, time);
    }

    public void ClickMouse()
    {
        if(_isPressed == false)
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
        ChangeButton(_game.Cristall);
    }

    private void ChangeButton(float cristall)
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
