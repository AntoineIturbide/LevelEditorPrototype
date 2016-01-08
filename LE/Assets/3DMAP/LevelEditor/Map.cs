using UnityEngine;
using System.Collections;

namespace Level {

    public class Map {

        public string name = "UntitledMap";

        public Terrain terrain = new Terrain(6, 6);

        public MapSaveData Export() {
            return new MapSaveData(terrain);
        }

        public bool Import(MapSaveData _msd) {
            if (_msd == null)
                return false;

            terrain = _msd.terrain;
            return true;
        }
    }

}