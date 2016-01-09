using UnityEngine;
using System.Collections;

namespace Level {

    public class Map {

        public string name = "UntitledMap";

        public Terrain terrain = new Terrain(256, 256);

        public MapSaveData Export() {
            return new MapSaveData(terrain);
        }

        public bool Import(MapSaveData _msd) {
            if (_msd == null)
                return false;

            terrain = _msd.terrain;
            return true;
        }

        public int[,,] GenerateChunkData(Vector3 position) {

            int x = (int)position.x;
            int z = (int)position.z;
            int chunkSize = 18;
            int chunkHeight = 5;
            int[,,] output = new int[chunkSize, chunkHeight, chunkSize];
            for (int _z = Mathf.Max(0, z); _z < Mathf.Min(terrain.length, z + chunkSize); _z++) {
                for (int _x = Mathf.Max(0, x); _x < Mathf.Min(terrain.width, x + chunkSize); _x++) {
                    for (int _y = Mathf.Min(terrain.heightMap[_x, _z], chunkHeight -1); _y >= 0; _y--) {
                        output[_x - x, _y, _z - z] = terrain.idMap[_x, _z];
                    }
                }
            }
            return output;
        }
    }

}