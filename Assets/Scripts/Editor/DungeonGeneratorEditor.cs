using UnityEditor;
using UnityEngine;

// Editor class for DungeonGenerator
[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    private DungeonGenerator targetGenerator;

    void OnEnable()
    {
        targetGenerator = (DungeonGenerator)target;
    }

    // Inspector UI elements
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying && GUILayout.Button("Generate Rom"))
        {
            targetGenerator.GenRoom();
        }

        if (EditorApplication.isPlaying && GUILayout.Button("Destroy Room"))
        {
            targetGenerator.DestroyRoom();
        }
    }
}
