using TMPro;
using UnityEngine;

namespace BouncingBalls
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextPriceBlock : MonoBehaviour
    {
        private TMP_Text _text;

        private void OnEnable()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void ChangeText(int value)
        {
            _text.text = value.ToString() + "$";
        }
    }
}