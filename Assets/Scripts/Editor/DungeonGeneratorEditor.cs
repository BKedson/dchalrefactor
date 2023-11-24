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

        if (EditorApplication.isPlaying == true && GUILayout.Button("Generate Dungeon"))
        {
            targetGenerator.InitializeDungeon();
        }
    }
}
