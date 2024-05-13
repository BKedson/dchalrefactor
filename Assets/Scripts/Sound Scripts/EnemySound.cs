using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float maxVol = 1f;
    public float minVol = 0f;
    public float proximity = 8000f;
    private Transform obj;

    void Start() 
    {
        obj = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() 
    {
        if(obj == null) return;

        float distance = Vector3.Distance(transform.position, obj.position);
        float volume = Mathf.Lerp(maxVol, minVol, distance/proximity);
        audioSource.volume = volume;
    }

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
