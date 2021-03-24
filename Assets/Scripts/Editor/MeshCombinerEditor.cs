using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshCombiner meshCombiner = target as MeshCombiner;
        base.OnInspectorGUI();
        if (GUILayout.Button("Combine Meshes"))
        {
            meshCombiner.CombineMeshes();
        }
    }
}
