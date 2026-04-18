using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls.View
{
    public class SubLevelView : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private Image _addImage;
        [SerializeField] private Color _inactiveColor;
        [SerializeField] private Color _activeColor;

        public void ActiveView()
        {
            _mainImage.color = _activeColor;
            _addImage.color = _activeColor;
        }

        public void InactiveView()
        {
            _mainImage.color = _inactiveColor;
            _addImage.color = _inactiveColor;
        }
    }
}