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
    private const string _textStartAnimation = "Play";

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
        _animator.SetTrigger(_textStartAnimation);
    }

    public void ChangeActiveOn()
    {
        _button.interactable = true;
    }

    public void ChangeActiveOff()
    {
        _button.interactable = false;
    }

    public void Init(float value)
    {
        _text.text = value.ToString();
    }
}
