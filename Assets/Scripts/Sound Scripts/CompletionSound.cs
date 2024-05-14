using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionSound : MonoBehaviour
{
    public AudioClip completeSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("enemy").Length < 1)
		{
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(completeSound);
            }
            else
            {
                StartCoroutine(WaitForSound());
            }
		}
    }

    IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.PlayOneShot(completeSound);
    }
}
