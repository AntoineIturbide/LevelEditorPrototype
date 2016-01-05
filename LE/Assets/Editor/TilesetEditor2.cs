using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Tileset))]
[CanEditMultipleObjects]
public class TilesetEditor2 : Editor {

    public override void OnInspectorGUI() {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.LabelField("Test");

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

}
