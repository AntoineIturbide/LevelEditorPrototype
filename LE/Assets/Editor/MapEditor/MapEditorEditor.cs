using UnityEngine;
using UnityEditor;
using System.Collections;

using Level;

[CustomEditor(typeof(Level.MapEditor))]
public class MapEditorEditor : Editor {

    public GUIContent buttonText = new GUIContent("Button Load/Save");
    public GUIStyle buttonStyle = GUIStyle.none;

    public bool drag = false;

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
            EditorGUILayout.LabelField("Click on the \"New\" button to create a new map.");
        }

        // Load / Save
        r = EditorGUILayout.GetControlRect();
        space = 2;

        if (GUI.Button(new Rect(r.x, r.y, (r.width - space) / 2, r.height), "Load")) {
            t.LoadMap();
            EditorGUIUtility.ExitGUI();
        }
        if (GUI.Button(new Rect(r.x + (r.width - space) / 2 + space, r.y, (r.width - space) / 2, r.height), "Save")) {
            t.SaveMap();
        }

        // Reload / Reset
        r = EditorGUILayout.GetControlRect();
        space = 2;

        if (GUI.Button(new Rect(r.x, r.y, (r.width - space) / 2, r.height), "New")) {
            t.CreateMap();
            EditorGUIUtility.ExitGUI();
        }
        if (GUI.Button(new Rect(r.x + (r.width - space) / 2 + space, r.y, (r.width - space) / 2, r.height), "Reset")) {
            t.CreateMap();
        }

    }

    public void OnSceneGUI() {

        MapEditor t = (MapEditor)target;
        if (t == null) return;


        EditorGUI.BeginChangeCheck();
        Vector3 chunkPos = Handles.PositionHandle(new Vector3(t.chunk.x, 1, t.chunk.z), Quaternion.identity);
        if (EditorGUI.EndChangeCheck()) {
            bool change = false;
            if(t.chunk.x != (int)chunkPos.x) {
                t.chunk.x = (int)chunkPos.x;
                change = true;
            }
            if (t.chunk.z != (int)chunkPos.z) {
                t.chunk.z = (int)chunkPos.z;
                change = true;
            }
            if (change) {
                t.UpdateChunk();
            }
        }

        /*
        Ray ray = HandleUtility.GUIPointToWorldRay((GUIUtility.GUIToScreenPoint(Event.current.mousePosition)));
        //Debug.DrawRay(ray.origin, ray.direction);
        Handles.CubeCap(0, ray.origin + ray.direction * 5, Quaternion.identity, 1f);
        */

        Ray ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000.0f)) {
            Color color = new Color(0, 0, 1, 0.5f);
            Handles.color = color;
            Handles.CubeCap(0, t.GetBlockPositionFromWorldPoint(hit.point), Quaternion.identity, 1f);

            if((Event.current.type == EventType.MouseDown && Event.current.button == 0)) {
                Event.current.type = EventType.used;
            }
        }

    }

}
