using UnityEngine;
using UnityEngine.UI;

namespace BouncingBalls.View
{
    public class Language : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private string _name;

        public string Name => _name;

        public void ChangeActiveOn()
        {
            _image.enabled = true;
        }

        public void ChangeActiveOff()
        {
            _image.enabled = false;
        }
    }
}