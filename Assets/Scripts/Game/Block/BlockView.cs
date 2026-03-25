using TMPro;
using UnityEngine;

namespace BouncingBalls
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private Block _block;

        private void OnDestroy()
        {
            if (_block != null)
            {
                _block.OnInitialize -= OnInitialize;
            }
        }

        private void Awake()
        {
            _block = GetComponent<Block>();

            _block.OnInitialize += OnInitialize;
        }

        private void OnInitialize(int factor)
        {
            _text.text = factor.ToString();
        }
    }
}