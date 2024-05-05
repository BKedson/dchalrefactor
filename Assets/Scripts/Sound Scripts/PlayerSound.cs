using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footSound;



    public void PlaySound()
    {
        audioSource.PlayOneShot(footSound);
    }
}
