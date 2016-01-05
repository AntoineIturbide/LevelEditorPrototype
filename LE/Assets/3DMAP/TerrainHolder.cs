using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainHolder : MonoBehaviour {

    private static TerrainHolder instance = null;

    [System.Serializable]
    public class Terrain {
        [Header("Data")]
        public int id;
        public string name;
        public Material material;
        [Header("Meshes")]
        public Mesh fill;
        public Mesh lineA;
        public Mesh lineB;
        public Mesh angleConcave;
        public Mesh angleConvex;
        public Mesh lineAUnder;
        public Mesh lineBUnder;
        public Mesh angleConvexUnder;
    }

    public Terrain[] terrains;

    private Dictionary<int, Terrain> dictionaryById = new Dictionary<int, Terrain>();
    private Dictionary<string, Terrain> dictionaryByName = new Dictionary<string, Terrain>();

    private void Awake() {
        if (!LoadDictionaryById())
            return;
        if (!LoadDictionaryByName())
            return;
        instance = this;
    }

    public bool LoadDictionaryById () {
        foreach (Terrain terrain in terrains) {
            if (dictionaryById.ContainsKey(terrain.id)) {
                Debug.LogError(this + " contains multiple terrains with the same id.");
                return false;
            } else {
                dictionaryById.Add(terrain.id, terrain);
            }
        }
        return true;
    }

    public bool LoadDictionaryByName() {
        foreach (Terrain terrain in terrains) {
            if (dictionaryByName.ContainsKey(terrain.name)) {
                Debug.LogError(this + " contains multiple terrains with the same name.");
                return false;
            } else {
                dictionaryByName.Add(terrain.name, terrain);
            }
        }
        return true;
    }

    static public Terrain GetTerrainFromId (int id) {

        if (instance == null) {
            Debug.LogError("There isn't any TerrainHolder instance.");
            return null;
        }

        Terrain output = null;
        if (!instance.dictionaryById.TryGetValue(id, out output)) {
            Debug.LogError("Requested terrain is missing from " + instance + ".");
            return null;
        } else {
            return output;
        }

    }

    static public Terrain GetTerrainFromName(string name) {

        if (instance == null) {
            Debug.LogError("There isn't any TerrainHolder instance.");
            return null;
        }

        Terrain output = null;
        if (!instance.dictionaryByName.TryGetValue(name, out output)) {
            Debug.LogError("Requested terrain is missing from " + instance + ".");
            return null;
        } else {
            return output;
        }

    }

}