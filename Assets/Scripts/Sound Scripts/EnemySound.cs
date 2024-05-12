using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;



    public void EnemyWalk()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void EnemyRun()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void EnemyAttack()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }
}
