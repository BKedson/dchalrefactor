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
        audioSource.clip = audioClips[0];
        audioSource.time = 0.10f;
        audioSource.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.clip = audioClips[1];
        audioSource.time = 0.2f;
        audioSource.Play();
    }
}
