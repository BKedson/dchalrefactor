using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;



    public void PlayerWalk()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void PlayerJump()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void PlayerAttack()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }
}
