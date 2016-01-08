using UnityEngine;
using System.Collections;

namespace Level {

    [System.Serializable]
    public class MapSaveData {

        public Terrain terrain;

        public MapSaveData(Terrain _terrain) {
            terrain = _terrain;
        }

    }

}