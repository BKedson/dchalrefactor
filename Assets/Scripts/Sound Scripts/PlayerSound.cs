using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip walkSound;



    public void PlaySound()
    {
        audioSource.PlayOneShot(walkSound);
    }
}
