using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tileset {

    public Texture2D _texture;
    public int _tileResolution;
    public int _width {
        get { return _texture.width / _tileResolution; }
    }
    public int _height {
        get { return _texture.height / _tileResolution; }
    }
    public int _maxId {
        get { return _height * _width; }
    }
    
    public Color[] GetTilePixelsFromId(int id) {
        int width = _width;
        int maxId = width * _height;
        id = Mathf.Clamp(id, 0, maxId - 1);
        int y = id / width;
        int x = id - (y * width);
        return _texture.GetPixels(x * _tileResolution, y * _tileResolution, _tileResolution, _tileResolution);
    }

    public int ClampID(int id) {
        return Mathf.Clamp(id, 0, _maxId - 1);
    }

}
