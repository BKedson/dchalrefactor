using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    private DungeonGenerator targetGenerator;

    void OnEnable()
    {
        targetGenerator = (DungeonGenerator)target;
    }

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
