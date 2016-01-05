using UnityEngine;
using System.Collections;

public class TerrainMap : MonoBehaviour {

    public int[,,] map = new int[16,16,16];

    void Awake() {
        map = new int[16, 8, 16];
        bool fill = false;
        for (int z = 0; z < map.GetLength(2); z++) {
            for (int x = 0; x < map.GetLength(0); x++) {
                fill = false;
                for (int y = 3; y >= 0; y--) {
                    if (!fill) {
                        map[x, y, z] = Mathf.Max(0, Random.Range((-2 * (y)) + 1, 2));
                        if (map[x, y, z] != 0) fill = true;
                    } else {
                        map[x, y, z] = map[x, y + 1, z];
                    }
                }
            }
        }
    }

    void OnDrawGizmos () {
        /*
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        GizmosDrawCell(Vector3.zero, new Color(1f, 1f, 1f, 0.75f));
        */ 

        // Colors
        Color defaultC = new Color(1f, 1f, 1f, 0.75f);
        Color validC = new Color(0f, 1f, 0f, 0.75f);

        // Draw Cells
        for (int z = 0; z < map.GetLength(2); z++) {
            for (int x = 0; x < map.GetLength(0); x++) {
                for (int y = map.GetLength(1) - 1; y >= 0; y--) {
                    if (map[x, y, z] != 0) {
                        GizmosDrawCell(new Vector3(x, y + 1.05f, z), validC);
                        break;
                    } else if (y == 0) {
                        GizmosDrawCell(new Vector3(x, y + 0.05f, z), defaultC);
                        break;
                    }
                }
            }
        }
    }

    void GizmosDrawCell(Vector3 position, Color color) {
        Gizmos.color = color;
        float delta = -0.1f;
        Gizmos.DrawLine(
            position - new Vector3(-(0.5f + delta), 0, -(0.5f + delta)),
            position - new Vector3(-(0.5f + delta), 0, (0.5f + delta))
        );
        Gizmos.DrawLine(
            position - new Vector3(-(0.5f + delta), 0, (0.5f + delta)),
            position - new Vector3((0.5f + delta), 0, (0.5f + delta))
        );
        Gizmos.DrawLine(
            position - new Vector3((0.5f + delta), 0, (0.5f + delta)),
            position - new Vector3((0.5f + delta), 0, -(0.5f + delta))
        );
        Gizmos.DrawLine(
            position - new Vector3((0.5f + delta), 0, -(0.5f + delta)),
            position - new Vector3(-(0.5f + delta), 0, -(0.5f + delta))
        );

        /*
        Gizmos.DrawCube(
            new Vector3(position.x, position.y / 2, position.z),
            new Vector3(1, position.y, 1)
        );
        */
    }

}
