using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

    //public TerrainMap _debug_TerrainMap;
    public Level.MapEditor mapEditor;

    public List<Node> nodes;

    Mesh mesh;

    private void Start() {
        /*
        nodes = CalculateNodes(mapEditor.loadedMap.GenerateChunkData(transform.position));
        gameObject.GetComponent<MeshFilter>().mesh = GenerateMesh();
        */
    }

    public class Node {
        public int terrainId;
        public Vector3 position;
        public bool[,] configuration;

        public Node(int _terrainId, Vector3 _position, bool[,] _configuration) {
            terrainId = _terrainId;
            position = _position;
            configuration = _configuration;
        }

        public Mesh GenerateMesh() {
            if (terrainId == 0) {
                return null;
            }

            // Retrieve terrain
            TerrainHolder.Terrain terrain = TerrainHolder.GetTerrainFromId(terrainId);
            if (terrain == null) {
                return null;
            }
            
            List<CombineInstance> meshCombiner = new List<CombineInstance>();
            CombineInstance tempCombiner;

            // Top
            if (configuration[1, 1]) {
                // Bottom left
                #region Bottom left

                Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 180, 0), Vector3.one);

                // Angle
                tempCombiner = new CombineInstance();
                tempCombiner.mesh = terrain.angleConvexUnder;
                tempCombiner.transform = transformMatrix;
                meshCombiner.Add(tempCombiner);
                #endregion

                // Bottom right
                #region Bottom right

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 90, 0), Vector3.one);

                // Angle
                tempCombiner = new CombineInstance();
                tempCombiner.mesh = terrain.angleConvexUnder;
                tempCombiner.transform = transformMatrix;
                meshCombiner.Add(tempCombiner);
                #endregion

                // Top left
                #region Top left

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, -90, 0), Vector3.one);

                // Angle
                tempCombiner = new CombineInstance();
                tempCombiner.mesh = terrain.angleConvexUnder;
                tempCombiner.transform = transformMatrix;
                meshCombiner.Add(tempCombiner);
                #endregion

                // Top right
                #region Top left

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 0, 0), Vector3.one);

                // Angle
                tempCombiner = new CombineInstance();
                tempCombiner.mesh = terrain.angleConvexUnder;
                tempCombiner.transform = transformMatrix;
                meshCombiner.Add(tempCombiner);
                #endregion
            } else {
                // Bottom left
                #region Bottom left

                Matrix4x4 transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 180, 0), Vector3.one);

                if (configuration[0, 1]) {
                    if (configuration[1, 0]) {
                        if (configuration[0, 0]) {
                            // Fill
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.fill;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        } else {
                            // Angle concave
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.angleConcave;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        }
                    } else {
                        // Line B
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineB;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);

                    }
                } else {
                    if (configuration[1, 0]) {
                        // Line A
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineA;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    } else {
                        // Angle convex
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.angleConvex;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    }
                }
                #endregion
                
                // Bottom right
                #region Bottom right

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 90, 0), Vector3.one);

                if (configuration[1, 0]) {
                    if (configuration[2, 1]) {
                        if (configuration[2, 0]) {
                            // Fill
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.fill;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        } else {
                            // Angle concave
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.angleConcave;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        }
                    } else {
                        // Line B
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineB;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);

                    }
                } else {
                    if (configuration[2, 1]) {
                        // Line A
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineA;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    } else {
                        // Angle convex
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.angleConvex;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    }
                }
                #endregion

                // Top left
                #region Top left

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, -90, 0), Vector3.one);

                if (configuration[1, 2]) {
                    if (configuration[0, 1]) {
                        if (configuration[0, 2]) {
                            // Fill
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.fill;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        } else {
                            // Angle concave
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.angleConcave;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        }
                    } else {
                        // Line B
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineB;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);

                    }
                } else {
                    if (configuration[0, 1]) {
                        // Line A
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineA;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    } else {
                        // Angle convex
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.angleConvex;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    }
                }
                #endregion

                // Top right
                #region Top left

                transformMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 0, 0), Vector3.one);

                if (configuration[2, 1]) {
                    if (configuration[1, 2]) {
                        if (configuration[2, 2]) {
                            // Fill
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.fill;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        } else {
                            // Angle concave
                            tempCombiner = new CombineInstance();
                            tempCombiner.mesh = terrain.angleConcave;
                            tempCombiner.transform = transformMatrix;
                            meshCombiner.Add(tempCombiner);
                        }
                    } else {
                        // Line B
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineB;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);

                    }
                } else {
                    if (configuration[1, 2]) {
                        // Line A
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.lineA;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    } else {
                        // Angle convex
                        tempCombiner = new CombineInstance();
                        tempCombiner.mesh = terrain.angleConvex;
                        tempCombiner.transform = transformMatrix;
                        meshCombiner.Add(tempCombiner);
                    }
                }
                #endregion
            }

            Mesh output = new Mesh();
            output.CombineMeshes(meshCombiner.ToArray());
            return output;
        }
    }

    private List<Node> CalculateNodes(int[,,] chunk) {

        if (
            chunk.GetLength(2) <= 2
            || chunk.GetLength(1) <= 2
            || chunk.GetLength(0) <= 2
        ) {
            return null;
        }

        List<Node> output = new List<Node>();

        // Data
        int id;
        Vector3 position;
        bool[,] configuration;

        for (int y = 0; y < chunk.GetLength(1) - 1; y++) {
            for (int z = 1; z < chunk.GetLength(2) - 1; z++) {
                for (int x = 1; x < chunk.GetLength(0) - 1; x++) {
                    // Get id
                    id = chunk[x, y, z];

                    // Create node if not air
                    if (id != 0) {
                        // Calculate position
                        position = new Vector3(x, y, z);

                        // Calculate configuration
                        configuration = new bool[3, 3];

                        // [Tl][T ][Tr]
                        // [L ][O ][R ]
                        // [Bl][B ][Br]

                        // Over
                        configuration[1, 1] = chunk[x, y + 1, z] == id;

                        // Botom
                        configuration[1, 0] = chunk[x, y, z - 1] == id;
                        // Left
                        configuration[0, 1] = chunk[x - 1, y, z] == id;
                        // Right
                        configuration[2, 1] = chunk[x + 1, y, z] == id;
                        // Top
                        configuration[1, 2] = chunk[x, y, z + 1] == id;


                        // Botom left
                        configuration[0, 0] =
                            chunk[x - 1, y, z - 1] == id
                            || (
                                chunk[x, y + 1, z - 1] == id
                                && chunk[x - 1, y + 1, z] == id
                                )
                        ;

                        // Botom right
                        configuration[2, 0] =
                            chunk[x + 1, y, z - 1] == id
                            || (
                                chunk[x, y + 1, z - 1] == id
                                && chunk[x + 1, y + 1, z] == id
                                )
                        ;

                        // Top left
                        configuration[0, 2] =
                            chunk[x - 1, y, z + 1] == id
                            || (
                                chunk[x, y + 1, z + 1] == id
                                && chunk[x - 1, y + 1, z] == id
                                )
                        ;

                        // Top right
                        configuration[2, 2] = chunk[x + 1, y, z + 1] == id
                            || (
                                chunk[x, y + 1, z + 1] == id
                                && chunk[x + 1, y + 1, z] == id
                                )
                        ;


                        output.Add(new Node(id, position, configuration));
                    }
                }
            }
        }

        return output;
    }

    // Mesh
    private Mesh GenerateMesh() {

        CombineInstance[] combine = new CombineInstance[nodes.Count];
        for (int i = 0; i < nodes.Count; i++) {
            Mesh nodeMesh = nodes[i].GenerateMesh();
            combine[i].mesh = nodes[i].GenerateMesh();
            combine[i].transform = Matrix4x4.TRS(nodes[i].position, Quaternion.identity, Vector3.one);
        }

        mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }

    // Render debug
    private void OnDrawGizmosSelected() {
        /*
        if (nodes != null) {
            foreach (Node node in nodes) {
                if (node != null) Gizmos.DrawCube(node.position, Vector3.one * 0.5f);
            }
        }
        */
    }
}
