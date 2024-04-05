using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RebindManager : MonoBehaviour
{
    public InputActionAsset actions;

    public void LoadRebinds()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
        Debug.Log("rebinds loaded");
    }

    public void SaveRebinds()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        Debug.Log("rebinds saved");

        SceneManager.LoadScene("Controls");
    }
}
