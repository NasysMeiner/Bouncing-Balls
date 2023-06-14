using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _price;

    private Animator _animator;
    private Button _button;

    public float Price => _price;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    private void Start()
    {
        Init(_price);
    }

    public void PlayAnimation()
    {
        _animator.SetTrigger("Play");
    }

    public void ChangeActive(bool isState)
    {
        _button.interactable = isState;
    }

    public void Init(float value)
    {
        _text.text = value.ToString();
    }
}
