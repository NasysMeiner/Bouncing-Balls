using System.Collections;
using UnityEngine;

namespace BouncingBalls
{
    public class RewardedAnnouncement : MonoBehaviour
    {
        [SerializeField] private GameObject _announcementView;
        [SerializeField] private float _timeRepeat;
        [SerializeField] private float _timeShow;
        [SerializeField] private int _repeats = 3;

        private GameManager _gameManager;

        private int _countRepeat = 0;

        private void OnEnable()
        {
            _gameManager.OnEndLevel += ResetRepeats;
            _gameManager.OnStartLevel += ResetRepeats;
        }

        private void OnDisable()
        {
            _gameManager.OnEndLevel -= ResetRepeats;
            _gameManager.OnStartLevel -= ResetRepeats;
        }

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private IEnumerator Announcement()
        {
            if (_countRepeat < _repeats)
            {
                _countRepeat++;
                _announcementView.SetActive(true);

                yield return new WaitForSeconds(_timeShow);

                _announcementView.SetActive(false);

                yield return new WaitForSeconds(_timeRepeat);

                StartCoroutine(Announcement());
            }
        }

        private void ResetRepeats()
        {
            _countRepeat = 0;
            _announcementView.SetActive(false);
            StartCoroutine(Announcement());
        }
    }
}