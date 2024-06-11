using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CompletionSound : MonoBehaviour
{
    public AudioClip completeSound;
    private AudioSource audioSource;
    private bool enemiesGenerated;
    private bool updated;

    public delegate void CompletionInput();
    public event CompletionInput OnCompletion;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemiesGenerated = false;
        updated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("enemy").Length >= 1 && updated == false)
        {
            enemiesGenerated = true;
            updated = true;
        }

        if (GameObject.FindGameObjectsWithTag("enemy").Length < 1 && enemiesGenerated == true)
		{
            if (!audioSource.isPlaying)
            {
                Sounder();
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
        Sounder();
    }

    public void Sounder() {
        audioSource.PlayOneShot(completeSound);
        enemiesGenerated = false;
        updated = false;
        OnCompletion?.Invoke();
    }
}
