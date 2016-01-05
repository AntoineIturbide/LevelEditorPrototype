using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TerrainMap))]
public class TerrainMapEditor : Editor {

    private TerrainMap t;
    private float test = 5f;
    bool selectNewNode = false;

    private void OnEnable() {
        t = (TerrainMap)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Button("test");

    }

    public void OnSceneGUI() {

        bool guiChanged = GUI.changed;
        GUI.changed = false;

        if (t.map != null) {
            for (int z = 0; z < t.map.GetLength(2); z++) {
                for (int x = 0; x < t.map.GetLength(0); x++) {
                    for (int y = t.map.GetLength(1) - 1; y >= 0; y--) {
                        if (t.map[x, y, z] != 0) {
                            // test = Mathf.Clamp(Handles.ScaleValueHandle(y, new Vector3(x, y + 1, z), Quaternion.Euler(-90,0,0), 15, Handles.ArrowCap, 5f), 0, t.map.GetLength(1) - 1);
                            test = Handles.ScaleValueHandle(y, new Vector3(x, y + 1, z), Quaternion.Euler(-90, 0, 0), 15, Handles.ArrowCap, 1f);
                            if (GUI.changed) {
                                int id = t.map[x, y, z];
                                for (int i = 0; i <= Mathf.Min((int)(test), t.map.GetLength(1) - 1); i++) {
                                    t.map[x, i, z] = id;
                                }
                                for (int i = Mathf.Max((int)(test) + 1, 0); i < t.map.GetLength(1); i++) {
                                    t.map[x, i, z] = 0;
                                }
                                EditorUtility.SetDirty(target);
                                GUI.changed = false;

                                Debug.Log(test);
                            }
                            break;
                        }
                    }
                }
            }
        }

        GUI.changed = guiChanged;

    }

}
