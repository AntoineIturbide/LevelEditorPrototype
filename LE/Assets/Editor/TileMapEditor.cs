using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

    bool _drawHandle = false;
    Tile _selectedTile = null;
    Texture2D _texture;
    Texture2D[] _tilesetTextures;

    float _row;
    int _selectedPainId;

    void UpdateTileset (TileMap tileMap) {
        if (tileMap._meshRenderer == null) {
            tileMap._meshRenderer = tileMap.gameObject.GetComponent<MeshRenderer>();
        }
        tileMap.ApplyTexture(tileMap.BuildTexture(tileMap._tiles));
        _tilesetTextures = LoadTexturesFromTileset(tileMap._tileset);
    }

    Texture2D[] LoadTexturesFromTileset(Tileset tilset) {
        // Check for errors
        if (tilset == null) {
            goto Error;
        }

        // Retrive values
        int width = tilset._width;
        int maxId = width * tilset._height;

        // Create output
        Texture2D[] output = new Texture2D[tilset._maxId];
        for (int i = 0; i < maxId; i++) {
            output[i] = new Texture2D(tilset._tileResolution, tilset._tileResolution);
            output[i].SetPixels(tilset.GetTilePixelsFromId(i));
            output[i].Apply();
        }
        return output;

        // Error case
        Error:
        return null;
    }



    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        TileMap tileMap = target as TileMap;

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        tileMap._tileset._texture = (Texture2D)EditorGUILayout.ObjectField("Tileset", tileMap._tileset._texture, typeof(Texture2D), true);
        if (tileMap._tileset._texture != null) {
            tileMap._tileset._tileResolution = Mathf.Clamp(
                EditorGUILayout.IntField("Tile Resolution", tileMap._tileset._tileResolution),
                1,
                Mathf.Min(tileMap._tileset._texture.width, tileMap._tileset._texture.height)
                );
        }
        if (EditorGUI.EndChangeCheck() && tileMap._tileset._texture != null) {
            EditorUtility.SetDirty(target);

            // Clamp _tileResolution
            tileMap._tileset._tileResolution = Mathf.Clamp(
                tileMap._tileset._tileResolution,
                1,
                Mathf.Min(tileMap._tileset._texture.width, tileMap._tileset._texture.height)
                );

            // Apply changes to texture if playing
            if (Application.isPlaying) {
                UpdateTileset(tileMap);
            }
        }

        GUI.enabled = false;
        if (Application.isPlaying) {
            GUI.enabled = true;
        }

        if (GUILayout.Button("Resize")) {
            EditorUtility.SetDirty(target);
            tileMap.Rebuild();
        }
        if (GUILayout.Button("Regenerate")) {
            EditorUtility.SetDirty(target);
            tileMap.Build();
        }

        // Draw Handle Button
        DrawHandlesButton();
        
        // Draw tile editor
        if(_selectedTile != null && tileMap != null) {
            DrawTileEditor(tileMap, _selectedTile);
        }

        GUI.enabled = true;

        // Draw tool editor
        if (Application.isPlaying) {
            DrawToolEditor(tileMap);
        }


        serializedObject.ApplyModifiedProperties();
    }

    void DrawHandlesButton () {
        if (_drawHandle) {
            if (GUILayout.Button("Hide Handle")) {
                _drawHandle = false;
            }
        } else {
            if (GUILayout.Button("Show Handle")) {
                _drawHandle = true;
            }
        }
    }

    void DrawTileEditor(TileMap tileMap, Tile tile) {
        EditorGUI.BeginChangeCheck();
        tile._bgId = (ushort)tileMap._tileset.ClampID(EditorGUILayout.IntField("BG ID", tile._bgId));
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
            // Reaply texture
            UpdateTileset(tileMap);
        }
    }

    void DrawToolEditor(TileMap tileMap) {

        if (_tilesetTextures == null || _tilesetTextures.Length <= 0) {
            goto Error;
        }

        Rect textureRect;
        int textureDisplayResolution = 32;
        Rect baseRect = EditorGUILayout.GetControlRect(false, textureDisplayResolution);
        float col = baseRect.width / (textureDisplayResolution + 1);
        if ((int) col > 0) {
            _row = (_tilesetTextures.Length-1) / (int)col + 1;
            int x, y;
            for (int i = 0; i < _tilesetTextures.Length; i++) {
                y = i / (int)col;
                x = i - (y * (int)col);
                textureRect = new Rect(baseRect.x + x * (textureDisplayResolution + 1), baseRect.y + y * (textureDisplayResolution + 1), textureDisplayResolution, textureDisplayResolution);
                if(GUI.Button(textureRect, _tilesetTextures[i])) {
                    Debug.Log("Selected ID : " +i);
                    _selectedPainId = i;
                };
            }
        }
        EditorGUILayout.GetControlRect(false, ((int)_row - 1)  * (textureDisplayResolution + 1));

        //GUI.Button()

        Error:
        return;
    }

    /* Scene GUI */

    void OnSceneGUI() {
        TileMap tileMap = target as TileMap;
        DrawHandles(tileMap);
        ExecutePaintMode(tileMap);
    }

    void DrawHandles(TileMap tileMap) {

        
        if (_drawHandle) {
            // Draw handle
            Handles.color = Color.green;
            float handleSize = 0.25f;
            if (tileMap != null) {
                for (int y = 0; y < tileMap._mapSizeZ; y++) {
                    for (int x = 0; x < tileMap._mapSizeX; x++) {
                        Vector3 position = tileMap.GetTileWorldPositionFromPositionOnGrid(x, y);
                        bool selected = Handles.Button(
                            position,
                            Quaternion.identity,
                            handleSize,
                            handleSize,
                            Handles.CubeCap
                            );
                        if (selected) {
                            EditorUtility.SetDirty(target);
                            _selectedTile = tileMap._tiles[x, y];
                        }
                    }
                }
            }
        }
    }

    /* Pain editor */

    void ExecutePaintMode (TileMap tileMap) {
        if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) {
            if (Event.current.button == 0) {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(worldRay, out hitInfo)) {
                    int x, y;
                    if (tileMap.GetTilePositionOnGridFromWorldPoint(hitInfo.point,out x, out y)) {
                        tileMap._tiles[x, y]._bgId = (ushort)_selectedPainId;
                        tileMap.ApplyTexture(tileMap.BuildTexture(tileMap._tiles));
                    }
                }

                Event.current.Use();
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

}
