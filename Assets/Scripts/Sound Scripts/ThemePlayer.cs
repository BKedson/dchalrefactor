using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ThemePlayer : MonoBehaviour
{
    public static ThemePlayer instance;

    public AudioClip[] audioClips;

    public AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        SceneManager.sceneLoaded += SceneLoaded;
    }

     private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu" && (!ThemePlayer.instance.audioSource.isPlaying || audioSource.clip == audioClips[1]))
        {
            audioSource.clip = audioClips[0];
            ThemePlayer.instance.audioSource.Play();
        }
        if (scene.name == "Room Generation Test Scene" && audioSource.clip == audioClips[0])
        {
            audioSource.clip = audioClips[1];
            ThemePlayer.instance.audioSource.Play();
        }
    }
}
