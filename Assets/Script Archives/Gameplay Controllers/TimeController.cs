using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void PauseOnLoad(bool paused)
    {
        Time.timeScale = paused ? 0.0f : 1.0f;
    }
}
