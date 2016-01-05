using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tile {

    public ushort _bgId = 1;

    public Tile(int bgId = -1) {
        if (bgId >= 0) {
            _bgId = (ushort)bgId;
        } else {
            _bgId = 0;
        }
    }
    
}
