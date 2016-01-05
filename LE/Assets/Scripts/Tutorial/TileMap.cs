using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {

    public MeshFilter _meshFilter;
    public MeshRenderer _meshRenderer;
    public MeshCollider _meshCollider;

    public ushort _mapSizeX = 1, _mapSizeZ = 1;
    public float _tileSize = 1f;
    
    [HideInInspector]
    public Tileset _tileset;

    public Tile[,] _tiles;

    public void Awake() {
        Rebuild();
    }
    
    public void Build() {
        if (_meshFilter == null)
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (_meshCollider == null)
            _meshCollider = gameObject.GetComponent<MeshCollider>();

        _tiles = BuildTiles(false);
        ApplyMesh(BuildMesh());
        ApplyTexture(BuildTexture(_tiles));
    }

    public void Rebuild() {
        if (_meshFilter == null)
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        if (_meshRenderer == null)
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (_meshCollider == null)
            _meshCollider = gameObject.GetComponent<MeshCollider>();

        _tiles = BuildTiles(true);
        ApplyMesh(BuildMesh());
        ApplyTexture(BuildTexture(_tiles));
    }

    public Tile[,] BuildTiles(bool conservation) {
        Tile[,] tiles;
        if (!conservation || _tiles == null || _tiles.Length <= 0) {
            tiles = new Tile[_mapSizeX, _mapSizeZ];
        } else {
            tiles = ResizeArray<Tile>(_tiles, _mapSizeX, _mapSizeZ);
        }
        for (int y = 0; y < _mapSizeZ; y++) {
            for (int x = 0; x < _mapSizeX; x++) {
                if(tiles[x, y] == null) {
                    tiles[x, y] = new Tile();
                }
            }
        }

        return tiles;
    }

    public Mesh BuildMesh() {
        // Calculate important values
        int xVerticles = _mapSizeX + 1;
        int zVerticles = _mapSizeZ + 1;
        int totalVerticles = xVerticles * zVerticles;
        int totalTriangles = _mapSizeX * _mapSizeZ * 2 * 3;
        
        // Generate mesh data
        Vector3[] verticles = new Vector3[totalVerticles];
        Vector3[] normals = new Vector3[totalVerticles];
        Vector2[] uv = new Vector2[totalVerticles];
        int[] triangles = new int[totalTriangles];
        
        for (int z = 0; z < zVerticles; z++) {
            for (int x = 0; x < xVerticles; x++) {
                verticles[z * xVerticles + x] = new Vector3(x * _tileSize, 0, z * _tileSize);
                normals[z * xVerticles + x] = Vector3.up;
                uv[z * xVerticles + x] = new Vector2((float)x / xVerticles, (float)z / zVerticles);
            }
        }

        for (int z = 0; z < _mapSizeZ; z++) {
            for (int x = 0; x < _mapSizeX; x++) {
                int index = z * _mapSizeX + x;
                int triIndex = index * 6;
                int verIndex = z * xVerticles + x;
                // 1st triangle
                triangles[triIndex + 0] = verIndex + 0;
                triangles[triIndex + 1] = verIndex + xVerticles + 0;
                triangles[triIndex + 2] = verIndex + xVerticles + 1;
                // 2nd triangle
                triangles[triIndex + 3] = verIndex + 0;
                triangles[triIndex + 4] = verIndex + xVerticles + 1;
                triangles[triIndex + 5] = verIndex + 1;
            }
        }

        // Create new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        return mesh;
    }

    public Texture2D BuildTexture(Tile[,] tiles) {
        if (_tileset == null) {
            // Error
            return null;
        }

        Texture2D texture = new Texture2D((_mapSizeX+1) * _tileset._tileResolution, (_mapSizeZ+1) * _tileset._tileResolution);
        texture.filterMode = FilterMode.Point;
        // For each tile
        for (int y = 0; y < _mapSizeZ; y++) {
            for (int x = 0; x < _mapSizeX; x++) {
                // Gen texture id
                // int textureId = Random.RandomRange(0, 3);
                texture.SetPixels(
                    x * _tileset._tileResolution,
                    y * _tileset._tileResolution,
                    _tileset._tileResolution,
                    _tileset._tileResolution,
                    _tileset.GetTilePixelsFromId(_tiles[x,y]._bgId)
                    );

            }
        }
        texture.Apply();
        return texture;
    }

    public void ApplyMesh(Mesh mesh) {
        // Assign
        if (_meshFilter == null)
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        if (_meshFilter == null) {
            // Error : _meshFilter not found
            return;
        }
        
        if (_meshCollider == null)
            _meshCollider = gameObject.GetComponent<MeshCollider>();
        if (_meshCollider == null) {
            // Error : _meshCollider not found
            return;
        }

        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    public bool ApplyTexture(Texture2D texture) {
        if(texture == null) {
            // Error : texture empty
            goto Error;
        }

        // Assign
        if (_meshRenderer == null)
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (_meshRenderer == null) {
            // Error : _meshFilter not found
            goto Error;
        }
        _meshRenderer.sharedMaterials[0].SetTexture(0, texture);
        return true;

        Error:
        return false;
    }

    public bool GetTilePositionOnGridFromWorldPoint(Vector3 worldPoint, out int x, out int y) {
        // Calculate relative position
        Vector3 relativePosition = worldPoint - transform.position;

        // Check if the point is within the bounds
        if (relativePosition.x < 0 || relativePosition.x > _mapSizeX * _tileSize || relativePosition.z < 0 || relativePosition.z > _mapSizeZ * _tileSize) {
            goto Error;
        }

        x = (int)(relativePosition.x / _tileSize);
        y = (int)(relativePosition.z / _tileSize);

        Debug.Log(relativePosition + " | (" + x + "," + y + ")");
        return true;

        Error:
        x = 0;
        y = 0;
        return false;
    }

    public Vector3 GetTileWorldPositionFromPoint(Vector3 worldPoint) {
        // Calculate relative position
        Vector3 relativePosition = worldPoint - transform.position;

        // Check if the point is within the bounds
        if (relativePosition.x < 0 || relativePosition.x > _mapSizeX * _tileSize || relativePosition.z < 0 || relativePosition.z > _mapSizeZ * _tileSize) {
            return Vector3.zero;
        }
        
        return new Vector3(
            transform.position.x + ((int)(relativePosition.x / _tileSize)+0.5f) * _tileSize,
            0,
            transform.position.z + ((int)(relativePosition.z / _tileSize) + 0.5f) * _tileSize
            );
    }

    public Vector3 GetTileWorldPositionFromPositionOnGrid(int x, int y) {
        return new Vector3(
            transform.position.x + (x + 0.5f) * _tileSize,
            0,
            transform.position.z + (y + 0.5f) * _tileSize
            );
    }

    T[,] ResizeArray<T>(T[,] original, int rows, int cols) {
        var newArray = new T[rows, cols];
        int minRows = Mathf.Min(rows, original.GetLength(0));
        int minCols = Mathf.Min(cols, original.GetLength(1));
        for (int i = 0; i < minRows; i++)
            for (int j = 0; j < minCols; j++)
                newArray[i, j] = original[i, j];
        return newArray;
    }

    void Reset() {
        if (_meshFilter != null)
            _meshFilter.mesh.Clear();
        if (_meshCollider != null)
            _meshCollider.sharedMesh.Clear();
    }
}
