using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Land {

    public List<Building> _buildings;

    public LandSquare[] _landSquares;

    bool GenerateLandSquares(byte size) {
        _landSquares = new LandSquare[((int)size * (int)size) - (int)size];
        return true;
    }

    void _debug_Feedback_DrawLand () {

    }
    
    void GetSquare(int x, int y) {

    }

}
