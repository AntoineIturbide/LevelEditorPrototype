using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMap : MonoBehaviour {

    /* DEBUG */
    public GameObject _debug_hexaPrefab;

    /* GLOBAL */
    static double g_hexa = .8660254037844386d;

    /* SYSTEM */
    public byte _mapSize = 1;
    public byte _mapScale = 1;

    public CellScript[,] _cellArray;

    public List<Ray> _debug_rayArray = new List<Ray>();

    void Awake() {
        LoadMap();
        DisplayMap();
    }

    bool LoadMap() {
        // Check for errors
        if (_mapSize <= 0) {
            Debug.Log("[Error] " + this + ".LoadMap() : _mapSize must be supperior to one for this script to work.");
            return false;
        }

        _cellArray = new CellScript[_mapSize, _mapSize];

        return true;
    }


    bool DisplayMap() {
        // Check for errors
        if (_cellArray == null && _cellArray.Length <= 0) {
            Debug.Log("[Error] " + this + ".DisplayMap() : _cellArray is corrupted.");
            return false;
        }

        // Log

        // Prefab
        bool decalHexa = false;
        for (int x = 0; x < _cellArray.GetLength(0); x++) {
            for (int y = 0; y < _cellArray.GetLength(1); y++) {
                GameObject newHexa = Instantiate(_debug_hexaPrefab);
                newHexa.transform.position =
                    decalHexa ?
                    new Vector3(
                    x * (float)g_hexa * (float)_mapScale,
                    Random.Range(-.25f, .25f),
                    (y + 0.5f) * (float)_mapScale
                    ) :
                    new Vector3(
                    x * (float)g_hexa * (float)_mapScale,
                    Random.Range(-.25f, .25f),
                    (y) * (float)_mapScale
                    );
                newHexa.transform.localScale = ( (Vector3.right + Vector3.up) * (float)_mapScale ) + -Vector3.forward;

            }
            decalHexa = !decalHexa;
        }

        return true;
    }

}
