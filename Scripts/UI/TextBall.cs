using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBall : MonoBehaviour
{
    [SerializeField] private bool _isText = true;

    private TMP_Text _text;
    private Image _image;
    private float _maxValue = 255f;
    private float _minValue = 1f;
    private int _leangthChange = 150;
    private float _positionChange = 0.002f;
    private float _startValue = 0.58f;
    private float _changeValue = 1.25f;

    private void OnEnable()
    {
        _text = transform.GetComponent<TMP_Text>();

        if (_isText)
        {
            _text = transform.GetComponent<TMP_Text>();
            StartCoroutine(FadeIn(_text.color));
        }
        else
        {
            _image = transform.GetComponent<Image>();
            StartCoroutine(FadeIn(_image.color));
        }
    }

    public void ChangeText(float profitability, float force)
    {
        if (_text != null)
        {
            _text.text = $"${profitability * force}";
            transform.gameObject.SetActive(true);
        }
    }

    public void ChangeActiveText(bool value)
    {
        gameObject.SetActive(value);
    }

    private IEnumerator FadeIn(Color color)
    {
        for (int i = 0; i < _leangthChange; i++)
        {
            color.a = _startValue - (_minValue / _maxValue * _changeValue * i);

            if (_isText)
                _text.color = color;
            else
                _image.color = color;

            transform.position = new Vector3(transform.position.x, transform.position.y + _positionChange * i, transform.position.z);

            yield return null;
        }

        color.a = _startValue;

        if (_isText)
        {
            transform.gameObject.SetActive(false);
            _text.color = color;
        }
        else
        {
            transform.gameObject.SetActive(false);
            _image.color = color;
        }
    }
}
