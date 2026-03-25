using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls
{
    public class Icon : MonoBehaviour
    {
        [SerializeField] private GameObject _selectView;
        [SerializeField] private Image _imageIcon;
        [SerializeField] private int _idIcon;

        public Sprite SpriteImage => _imageIcon.sprite;
        public int idIcon => _idIcon;

        public void OffSelectView()
        {
            _selectView.SetActive(false);
        }

        public void OnSelectView()
        {
            _selectView.SetActive(true);
        }
    }
}