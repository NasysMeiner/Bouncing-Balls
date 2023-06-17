using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlock : MonoBehaviour
{
    [SerializeField] private BufferTexts _bufferText;

    private string _nameTextMoney = "Money";
    private string _nameTextCristall = "Cristall";

    public void ShowMoneyTextBlock(int ballProfitability, int blockProfitabiliy, Vector3 textPosition)
    {
        TextBall textMoney = _bufferText.TryGetText(_nameTextMoney);

        if (textMoney != null)
        {
            Vector3 Change = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0);
            textMoney.transform.position = textPosition;
            textMoney.transform.position += Change;
            textMoney.ChangeText(ballProfitability, blockProfitabiliy);
            textMoney.ChangeActiveText(true);
        }
    }

    public void ShowCristallTextBlock(Vector3 textPosition)
    {
        TextBall textCristall = _bufferText.TryGetText(_nameTextCristall);

        if (textCristall != null)
        {
            Vector3 Change = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
            textCristall.transform.position = textPosition;
            textCristall.transform.position += Change;
            textCristall.ChangeActiveText(true);
        }
    }
}
