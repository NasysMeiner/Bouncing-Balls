using System.Collections.Generic;
using UnityEngine;

public class BufferTexts : MonoBehaviour
{
    [SerializeField] private int _numberTexts;
    [SerializeField] private TextBall _prefabTextMoney;
    [SerializeField] private TextBall _prefabTextCristall;

    private List<TextBall> _textListMoney = new List<TextBall>();
    private List<TextBall> _textListCristall = new List<TextBall>();

    private void Start()
    {
        CreateBufferTexts(_numberTexts);
    }

    public TextBall TryGetText(int value = 0)
    {
        List<TextBall> textList;

        if(value > 0)
            textList = _textListCristall;
        else
            textList = _textListMoney;

        foreach (TextBall text in textList)
        {
            if (text.gameObject.activeSelf == false)
            {
                return text;
            }
        }

        return null;
    }

    private void CreateBufferTexts(int value)
    {
        for(int i =0; i < _numberTexts; i++)
        {
            _textListMoney.Add(Instantiate(_prefabTextMoney, transform));
            _textListCristall.Add(Instantiate(_prefabTextCristall, transform));
        }
    }
}
