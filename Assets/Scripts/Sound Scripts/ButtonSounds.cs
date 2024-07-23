using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!audioSource.isPlaying) {
            audioSource.clip = audioClips[0];
            audioSource.time = 0.20f;
            audioSource.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!audioSource.isPlaying) {
            audioSource.clip = audioClips[1];
            audioSource.time = 0.25f;
            audioSource.Play();
        }
    }
}
