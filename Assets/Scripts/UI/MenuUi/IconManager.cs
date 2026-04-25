using System.Collections.Generic;
using UnityEngine;

namespace BouncingBalls.View
{
    public class IconManager : MonoBehaviour
    {
        [SerializeField] private List<Icon> _allIcon = new();

        public Icon CurrentIcon { get; private set; }
        public int CurrentIdIcon => CurrentIcon.IdIcon;

        public void Initialize()
        {
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