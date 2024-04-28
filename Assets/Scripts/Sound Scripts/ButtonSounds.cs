using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hoverSound;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.time = 0.10f;
        audioSource.Play();
    }
}
