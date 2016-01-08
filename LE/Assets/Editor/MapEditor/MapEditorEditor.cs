using UnityEngine;
using UnityEditor;
using System.Collections;

using Level;

[CustomEditor(typeof(Level.MapEditor))]
public class MapEditorEditor : Editor {

    public GUIContent buttonText = new GUIContent("Button Load/Save");
    public GUIStyle buttonStyle = GUIStyle.none;

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        
        MapEditor t = (MapEditor)target;
        if (t == null) return;
        
        Rect r;
        float space;

        if(t.loadedMap != null) {
            EditorGUILayout.LabelField("Loaded map :");
            EditorGUILayout.LabelField(t.loadedMap.name);
        } else {
            EditorGUILayout.LabelField("No map is currently loaded");
            if(GUILayout.Button("New Map")) {
                t.CreateMap();
            }
        }
        r = EditorGUILayout.GetControlRect();
        space = 2;

        if (GUI.Button(new Rect(r.x, r.y, (r.width - space) / 2, r.height), "Load")) {
            t.LoadMap();
        }
        if (GUI.Button(new Rect(r.x + (r.width - space) / 2 + space, r.y, (r.width - space) / 2, r.height), "Save")) {
            t.SaveMap();
        }


    }

}
