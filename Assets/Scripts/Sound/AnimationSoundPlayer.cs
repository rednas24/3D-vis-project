using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

        public void PlaySound(AudioClip clip)
    {
        Debug.Log($"PlaySound on {gameObject.name} frame {Time.frameCount}");

        audioSource.PlayOneShot(clip);
    }
}