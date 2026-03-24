using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private Image _imageCristall;
    [SerializeField] private TMP_Text _text;

    private CanvasGroup _canvasGroup;
    private Camera _camera;

    private float _maxValue = 255f;
    private float _minValue = 1f;
    private int _leangthChange = 150;
    private float _positionChange = 0.002f;
    private float _startValue = 0.58f;
    private float _changeValue = 1.25f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _camera = Camera.main;
    }

    public void Initialize(string value, Vector3 startPosition, bool isImageOn = false)
    {
        if (isImageOn)
            _imageCristall.enabled = true;
        else
            _imageCristall.enabled = false;

        _text.text = value;

        transform.position = _camera.WorldToScreenPoint(startPosition);
        StartCoroutine(FadeIn(_canvasGroup.alpha));
    }

    private IEnumerator FadeIn(float alpha)
    {
        for (int i = 0; i < _leangthChange; i++)
        {
            alpha = _startValue - (_minValue / _maxValue * _changeValue * i);
            _canvasGroup.alpha = alpha;
            transform.position = new Vector3(transform.position.x, transform.position.y + _positionChange * i, transform.position.z);

            yield return null;
        }

        PoolManager.Instance.SetObject(this, BouncingBalls.ObjectType.ScorePopup);
    }
}
