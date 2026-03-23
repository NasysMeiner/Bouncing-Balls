using UnityEngine;

public class BlockAudioController : MonoBehaviour
{
    [SerializeField] private string _nameBounceSound = "Bounce";

    private Block _block;

    private void OnDestroy()
    {
        if (_block != null )
        {
            _block.Bounced -= PlayBounceSound;
        }
    }

    private void Awake()
    {
        _block = GetComponent<Block>();

        _block.Bounced += PlayBounceSound;
    }

    private void PlayBounceSound()
    {
        AudioManager.Instance.PlayEffectAudio(_nameBounceSound);
    }
}
