using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

namespace BouncingBalls
{
    public class SaveManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private BlockDeleter _blockDeleter;
        private UIManager _uiManager;
        private Bank _bank;
        private IconManager _iconManager;

        private string _dataIsEmpty = "{}";
        private string _loadString = "None";
        private string _loadingString = "None";

        public event Action<LoadType, PlayerProgressData> OnLoadData;

        private void OnDestroy()
        {
            if (_gameManager != null)
                _gameManager.OnEndLevel -= SavePlayerData;
        }

        public void Initialize(GameManager gameManager, BlockDeleter blockDeleter, UIManager uIManager, Bank bank, IconManager iconManager)
        {
            _gameManager = gameManager;
            _blockDeleter = blockDeleter;
            _uiManager = uIManager;
            _bank = bank;
            _iconManager = iconManager;

            _gameManager.OnEndLevel += SavePlayerData;
        }

        public void SavePlayerData()
        {
            if (!_gameManager.IsUnity)
            {
                PlayerProgressData playerProgress = new();
                playerProgress.Level = _gameManager.CurrentLevel;
                playerProgress.Cristall = _bank.Cristall;
                playerProgress.IsUnlockBlockDeleter = _blockDeleter.IsUnlock;
                playerProgress.LevelBlockDeleter = _blockDeleter.CurrentLevelBlockDeleter;
                playerProgress.IconId = _iconManager.CurrentIdIcon;
                playerProgress.IsShowGuide = _uiManager.IsShowGuide;
                playerProgress.Name = _uiManager.PlayerName;

                string jsonString = JsonUtility.ToJson(playerProgress);
                PlayerAccount.SetCloudSaveData(jsonString);
            }
        }

        public void GetPlayerData()
        {
            StartCoroutine(OnGetCloudSaveDataButtonClick());
        }

        private IEnumerator OnGetCloudSaveDataButtonClick()
        {
            if (_gameManager.IsUnity)
            {
                OnLoadData?.Invoke(LoadType.Failed, null);
                yield break;
            }

            PlayerAccount.GetCloudSaveData((data) => _loadString = data);

            while (_loadString == _loadingString)
            {
                yield return null;
            }

            if (_loadingString == _dataIsEmpty)
            {
                OnLoadData?.Invoke(LoadType.Failed, null);
                yield break;
            }

            PlayerProgressData playerData = JsonUtility.FromJson<PlayerProgressData>(_loadingString);

            OnLoadData?.Invoke(LoadType.Success, playerData);
        }
    }
}