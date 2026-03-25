using UnityEngine;

namespace BouncingBalls
{
    public class ScorePopupManager : MonoBehaviour
    {
        private BlockManager _blockManager;

        private float _maxErorrPosition = 0.2f;

        private void OnDestroy()
        {
            if (_blockManager != null)
            {
                _blockManager.OnBlockAdded -= OnBlockAdded;
                _blockManager.OnBlockRemoved -= OnBlockRemoved;
            }
        }

        public void Initialize(BlockManager blockManager)
        {
            _blockManager = blockManager;

            _blockManager.OnBlockAdded += OnBlockAdded;
            _blockManager.OnBlockRemoved += OnBlockRemoved;
        }

        public void OnScoreEarned(BounceScoreData bounceScoreData)
        {
            ScorePopup scorePopup = PoolManager.Instance.GetObject<ScorePopup>(ObjectType.ScorePopup);

            if (scorePopup == null)
                return;

            Vector3 errorPosition = new(Random.Range(-_maxErorrPosition, _maxErorrPosition), Random.Range(-_maxErorrPosition, _maxErorrPosition), 0);
            string scoreText = bounceScoreData.Score > 0 ? bounceScoreData.Score.ToString() : "";
            scorePopup.gameObject.SetActive(true);
            scorePopup.Initialize(scoreText, bounceScoreData.BouncePosition + errorPosition, bounceScoreData.Cristall > 0);
        }

        private void OnBlockAdded(Block block)
        {
            block.OnScoreEarned += OnScoreEarned;
        }

        private void OnBlockRemoved(Block block)
        {
            block.OnScoreEarned -= OnScoreEarned;
        }
    }
}