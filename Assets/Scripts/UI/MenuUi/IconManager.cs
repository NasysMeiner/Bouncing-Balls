using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls
{
    public class IconManager : MonoBehaviour
    {
        [SerializeField] private List<Icon> _allIcon = new();

        private UIManager _uiManager;

        public Icon CurrentIcon { get; private set; }
        public int CurrentIdIcon => CurrentIcon.idIcon;

        public void Initialize(UIManager uIManager)
        {
            _uiManager = uIManager;

            if (CurrentIcon == null)
            {
                CurrentIcon = _allIcon[0];
                CurrentIcon.OnSelectView();
            }
        }

        public void SetIconFromId(int id)
        {
            if (id >= _allIcon.Count)
                id = 0;

            SetIcon(_allIcon[id]);
        }

        public void SetIcon(Icon icon)
        {
            if (CurrentIcon != null)
                CurrentIcon.OffSelectView();

            CurrentIcon = icon;
            CurrentIcon.OnSelectView();
        }
    }
}