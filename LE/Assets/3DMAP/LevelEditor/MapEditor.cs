using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Level {

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MapEditor : MonoBehaviour {

        public class Chunk {

            public int x = 0;
            public int z = 0;

            public int size = 10;

            BoxCollider[,] colliders;

            public Chunk() {
                colliders = new BoxCollider[size, size];
            }

            public bool Load(Map map, GameObject go) {
                LoadTerrain(map.terrain, go);
                return true;
            }

            public bool LoadTerrain(Terrain terrain, GameObject go) {

                if (go == null) {
                    Debug.Log("ERROR");
                    return false;
                }

                // Reset

                Collider[] coll = go.GetComponents<Collider>();
                for (int i = 0; i < coll.Length; i++) {
                    if (coll[i] != null)
                        DestroyImmediate(coll[i]);
                }

                colliders = new BoxCollider[size, size];
                for (int _z = z; _z < Mathf.Min(terrain.length, z + size); _z++) {
                    for (int _x = x; _x < Mathf.Min(terrain.width, x + size); _x++) {
                        colliders[_x - x, _z - z] = go.AddComponent<BoxCollider>();
                        colliders[_x - x, _z - z].center = new Vector3(_x, terrain.heightMap[_x, _z] / 2f, _z);
                        colliders[_x - x, _z - z].size = new Vector3(1, terrain.heightMap[_x, _z], 1);
                        Debug.Log(terrain.heightMap[_x, _z]);
                    }
                }
                return true;
            }

        }

        public class Cell {
            int x = 0;
            int z = 0;
            Map map;

            public byte height {
                get { return map.terrain.heightMap[x, z]; }
                set { map.terrain.heightMap[x, z] = value; }
            }

            public byte id {
                get { return map.terrain.idMap[x, z]; }
                set { map.terrain.idMap[x, z] = value; }
            }

            public Cell(int _x, int _z, Map _map) {
                x = _x;
                z = _z;
                map = _map;
            }
        }

        public Chunk chunk = new Chunk();

        public Cell selectedCell = null;

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
            //chunk.Load(loadedMap, gameObject);

            DrawTerrainMesh();

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
                if (loadedMap == null)
                    loadedMap = new Map();
                loadedMap.Import(msd);
                file.Close();

                loadedMap.name = Path.GetFileNameWithoutExtension(path);

                //EditorUtility.SetDirty(this);

                // Chunk
                Chunk chunk = new Chunk();
                //chunk.Load(loadedMap, gameObject);

                // Mesh
                DrawTerrainMesh();
            }

            return true;
        }

        public bool SaveMap(string path = "") {

            if (loadedMap == null) {
                Debug.LogError("No map is currently loaded");
                return false;
            }

            if (path == "") {
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

        public bool UpdateChunk() {

            if (loadedMap == null)
                return false;

            //chunk.Load(loadedMap, gameObject);
            DrawTerrainMesh();

            return true;
        }

        // Edition

        public Vector3 SelectCell(Vector3 position) {
            int x = (int)(position.x + 0.5f);
            int z = (int)(position.z + 0.5f);

            selectedCell = new Cell(x, z, loadedMap);

            // 3DPos
            Vector3 output = new Vector3(
                x,
                (int)(position.y + 0.5f) - 0.5f,
                z
                );
            return output;
        }

        public void DeselectCell() {
            selectedCell = null;
        }

        public Vector2 EditCell(Vector2 input) {
            if(selectedCell != null){
                byte old = selectedCell.height;
                selectedCell.height = (byte)Mathf.Clamp(input.y, 0, 8);
                if(old != selectedCell.height) {
                    DrawTerrainMesh();
                }
                return new Vector2(input.x, selectedCell.height + input.y % 1 );
            }
            return Vector2.zero;
        }

        // Rendering

        public void DrawTerrainMesh() {

            if (loadedMap == null)
                return;

            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null) {
                Vector3 position = new Vector3(chunk.x - 1, 0, chunk.z - 1);
                int[,,] chunkData = loadedMap.GenerateChunkData(position);
                List<MeshGenerator.Node> nodes = MeshGenerator.CalculateNodes(chunkData);
                // Mesh
                Mesh mesh;
                Material[] materials;
                if (mesh = MeshGenerator.GenerateMesh(nodes, position, out materials)){
                    mf.sharedMesh.subMeshCount = materials.Length;
                    // Materials
                    MeshRenderer mr = GetComponent<MeshRenderer>();
                    if (mr != null) {
                        mr.materials = materials;
                    }
                }
                mf.sharedMesh = mesh;
                MeshCollider mc = GetComponent<MeshCollider>();
                if(mc != null) {
                    mc.sharedMesh = mf.sharedMesh;
                }
            }

        }

        public Vector3 GetBlockPositionFromWorldPoint(Vector3 input) {
            Vector3 output = new Vector3(
                (int)(input.x + 0.5f),
                (int)(input.y + 0.5f) - 0.5f,
                (int)(input.z + 0.5f)
                );
            return output;
        }

        // Gizmos

        private void OnDrawGizmos () {

            if(loadedMap == null) {
                return;
            }

            if (loadedMap.terrain != null) {
                DrawTerrain(loadedMap.terrain);
            }
            
        }

        void DrawTerrain(Terrain terrain) {

            if (chunk == null)
                return;

            // Colors
            Color defaultC = new Color(1f, 1f, 1f, 0.75f);
            Color validC = new Color(0f, 1f, 0f, 0.75f);
            float yOffset = 1.05f;



            // Draw Cells
            /*
            for (int z = 0; z < terrain.length; z++) {
                for (int x = 0; x < terrain.width; x++) {
                    
                    if (terrain.idMap[x, z] != 0) {
                        DrawCell(new Vector3(x, terrain.heightMap[x, z] + yOffset, z), validC);
                    } else {
                        DrawCell(new Vector3(x, terrain.heightMap[x, z], z), defaultC);
                    }
                }
            }*/
            
            for (int _z = Mathf.Max(0, chunk.z - 1); _z < Mathf.Min(terrain.length, chunk.z - 1 + chunk.size); _z++) {
                for (int _x = Mathf.Max(0, chunk.x - 1); _x < Mathf.Min(terrain.width, chunk.x - 1 + chunk.size); _x++) {
                    if (terrain.idMap[_x, _z] != 0) {
                        DrawCell(new Vector3(_x, terrain.heightMap[_x, _z] + yOffset, _z), validC);
                    } else {
                        DrawCell(new Vector3(_x, terrain.heightMap[_x, _z], _z), defaultC);
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