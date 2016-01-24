using UnityEngine;
using System.Collections;

namespace Level {

    [System.Serializable]
    public class Terrain {

        public byte[,] heightMap;
        public byte[,] idMap;
        
        public int width {
            get { return Mathf.Min(heightMap.GetLength(0), idMap.GetLength(0)); }
        }

        public int length {
            get { return Mathf.Min(heightMap.GetLength(1), idMap.GetLength(1)); }
        }

        public Terrain(int width, int length) {
            heightMap = new byte[width, length];
            for (int y = 0; y < length; y++) {
                for (int x = 0; x < width; x++) {
                    heightMap[x, y] = 0;
                        //(byte)Random.Range(0, 4);
                }
            }
            idMap = new byte[width, length];
            for (int y = 0; y < length; y++) {
                for (int x = 0; x < width; x++) {
                    idMap[x, y] = (byte)Random.Range(1, 3);
                }
            }
        }

    }

}