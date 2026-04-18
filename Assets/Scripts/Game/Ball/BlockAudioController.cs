using BouncingBalls.Audio;
using UnityEngine;

namespace BouncingBalls.Ball
{
    public class BlockAudioController : MonoBehaviour
    {
        [SerializeField] private string _nameBounceSound = "Bounce";

        private Block.Block _block;

        private void OnDestroy()
        {
            if (_block != null)
            {
                _block.Bounced -= PlayBounceSound;
            }
        }

        private void Awake()
        {
            _block = GetComponent<Block.Block>();

            _block.Bounced += PlayBounceSound;
        }

        private void PlayBounceSound()
        {
            AudioManager.Instance.PlayEffectAudio(_nameBounceSound);
        }
    }
}