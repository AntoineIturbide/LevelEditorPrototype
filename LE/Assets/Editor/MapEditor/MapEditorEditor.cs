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

        // Set Targer
        MapEditor t = (MapEditor)target;
        if (t == null) return;

        // Chunk Handle
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
        
        // Edit mode
        if (!drag) {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1000.0f)) {
                Color color = new Color(0, 0, 1, 0.5f);
                Handles.color = color;
                dragGUIPos = t.GetBlockPositionFromWorldPoint(hit.point);
                Handles.CubeCap(0, dragGUIPos, Quaternion.identity, 1f);
                
                if ((Event.current.type == EventType.MouseDown && Event.current.button == 0)) {
                    drag = true;
                    dragGUIPos = t.SelectCell(hit.point);
                    mousePos.x = Event.current.mousePosition.x;
                    mousePos.y = Event.current.mousePosition.y;
                    dragValue.y = dragGUIPos.y;
                    Event.current.type = EventType.used;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }
        } else {


            //Handles
            Color color = new Color(0, 0, 1, 0.5f);
            Handles.color = color;
            Handles.CubeCap(0, dragGUIPos, Quaternion.identity, 1f);

            // GUI
            Handles.BeginGUI();

            Vector2 guiPoint = HandleUtility.WorldToGUIPoint(dragGUIPos);
            Rect r = new Rect(guiPoint.x - 50, guiPoint.y - 50, 100, 100);
            dragValue.x = GUI.HorizontalSlider(r, dragValue.x, 0.0F, 10.0F);
            dragValue.y = GUI.VerticalSlider(r, dragValue.y, 0.0F, 10.0F);
            Handles.EndGUI();
            

            // Drag
            if ((Event.current.type == EventType.MouseDrag && Event.current.button == 0)) {
                float deltaX = (Event.current.mousePosition.x - mousePos.x) * 0.02f;
                float deltaY = (Event.current.mousePosition.y - mousePos.y) * 0.02f;
                dragValue.x += deltaX;
                dragValue.y -= deltaY;
                dragValue = t.EditCell(dragValue);
                mousePos.x = Event.current.mousePosition.x;
                mousePos.y = Event.current.mousePosition.y;
            }

            // Stop
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0)) {
                drag = false;
                Event.current.type = EventType.used;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if ((Event.current.type == EventType.MouseUp && Event.current.button == 0)) {
            drag = false;
            Cursor.lockState = CursorLockMode.None;
        }
        
        if ((Event.current.type == EventType.MouseMove)) {
            SceneView.RepaintAll();
        }
    }

    // Edit mode
    bool drag = false;
    Vector3 dragGUIPos = Vector3.zero;
    Vector2 dragValue = Vector2.zero;
    Vector2 mousePos = Vector2.zero;    

}
