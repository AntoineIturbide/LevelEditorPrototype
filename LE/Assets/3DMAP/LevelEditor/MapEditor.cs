using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Level {

    public class MapEditor : MonoBehaviour {

        public class Chunk {
            
            public int x = 0;
            public int z = 0;

            public int size = 4;
            
            BoxCollider[,] colliders;

            public Chunk () {
                colliders = new BoxCollider[size, size];
            }

            public bool Load(Map map, GameObject go) {
                LoadTerrain(map.terrain, go);
                return true;
            }

            public bool LoadTerrain (Terrain terrain, GameObject go) {

                if(go == null) {
                    Debug.Log("ERROR");
                    return false;
                }

                // Reset
                Collider[] coll = go.GetComponents<Collider>();
                Debug.Log(coll.Length);
                for (int i = 0; i < coll.Length; i++) {
                    if(coll[i] != null)
                        DestroyImmediate(coll[i]);
                }

                colliders = new BoxCollider[size, size];
                for (int _z = z; _z < Mathf.Min(terrain.length, z + size); _z++) {
                    for (int _x = x; _x < Mathf.Min(terrain.width, x + size); _x++) {
                        Debug.Log("x : " + _x + " - z : " + _z);
                        colliders[_x - x, _z - z] = go.AddComponent<BoxCollider>();
                        colliders[_x - x, _z - z].center = new Vector3(_x, terrain.heightMap[_x, _z] / 2f, _z);
                        colliders[_x - x, _z - z].size = new Vector3(1, terrain.heightMap[_x, _z], 1);
                        Debug.Log(terrain.heightMap[_x, _z]);
                    }
                }
                return true;
            }

        }

        Chunk chunk = new Chunk();

        public Map loadedMap;
        
        [Header("Data")]
        public string saveFileName = "";
        
        // System

        public bool Initialise() {

            //

            return true;
        }

        public bool CreateMap() {

            loadedMap = new Map();
            Chunk chunk = new Chunk();
            chunk.Load(loadedMap, gameObject);

            return true;
        }

        public bool CloseMap() {

            loadedMap = null;

            return true;
        }

        public bool LoadMap(string path = "") {

            if (path == "") {
                path = EditorUtility.OpenFilePanel(
                    "Load Map",
                    Application.dataPath,
                    "map");
            }
            
            if (path.Length != 0 && File.Exists(path)) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                MapSaveData msd = (MapSaveData)bf.Deserialize(file);
                if(loadedMap == null)
                    loadedMap = new Map();
                loadedMap.Import(msd);
                file.Close();

                loadedMap.name = Path.GetFileNameWithoutExtension(path);

                //EditorUtility.SetDirty(this);

                Chunk chunk = new Chunk();
                chunk.Load(loadedMap, gameObject);
            }

            return true;
        }

        public bool SaveMap(string path = "") {

            if(loadedMap == null) {
                Debug.LogError("No map is currently loaded");
                return false;
            }

            if(path == "") {
                path = EditorUtility.SaveFilePanel(
                        "Save Map",
                        Application.dataPath,
                        loadedMap.name + ".map",
                        "map");
            }

            if (path.Length != 0) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(path);
                bf.Serialize(file, loadedMap.Export());
                file.Close();

                loadedMap.name = Path.GetFileNameWithoutExtension(path);
            }

            return true;
        }

        // Rendering

        private void OnDrawGizmos () {

            if(loadedMap == null) {
                return;
            }

            if (loadedMap.terrain != null) {
                DrawTerrain(loadedMap.terrain);
            }
            
        }

        void DrawTerrain(Terrain terrain) {


            // Colors
            Color defaultC = new Color(1f, 1f, 1f, 0.75f);
            Color validC = new Color(0f, 1f, 0f, 0.75f);
            float yOffset = 1.05f;

            // Draw Cells
            for (int z = 0; z < terrain.length; z++) {
                for (int x = 0; x < terrain.width; x++) {
                    
                    if (terrain.idMap[x, z] != 0) {
                        DrawCell(new Vector3(x, terrain.heightMap[x, z] + yOffset, z), validC);
                    } else {
                        DrawCell(new Vector3(x, terrain.heightMap[x, z], z), defaultC);
                    }
                }
            }
        }

        void DrawCell(Vector3 position, Color color) {
            Gizmos.color = color;
            float offset = -0.05f;
            Gizmos.DrawLine(
                position - new Vector3(-(0.5f + offset), 0, -(0.5f + offset)),
                position - new Vector3(-(0.5f + offset), 0, (0.5f + offset))
            );
            Gizmos.DrawLine(
                position - new Vector3(-(0.5f + offset), 0, (0.5f + offset)),
                position - new Vector3((0.5f + offset), 0, (0.5f + offset))
            );
            Gizmos.DrawLine(
                position - new Vector3((0.5f + offset), 0, (0.5f + offset)),
                position - new Vector3((0.5f + offset), 0, -(0.5f + offset))
            );
            Gizmos.DrawLine(
                position - new Vector3((0.5f + offset), 0, -(0.5f + offset)),
                position - new Vector3(-(0.5f + offset), 0, -(0.5f + offset))
            );

        }

    }

}