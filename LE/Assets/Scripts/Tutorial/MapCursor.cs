using UnityEngine;
using System.Collections;

public class MapCursor : MonoBehaviour {

    public TileMap _tileMap;
    public Vector3 _targetPosition;
    public float speed = 5f;

    void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), hit.point - Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _targetPosition = _tileMap.GetTileWorldPositionFromPoint(hit.point);
            }
        }

        Vector3 newPosition = transform.position;
        newPosition = Vector3.MoveTowards(newPosition, _targetPosition, (speed + Vector3.Distance(newPosition, _targetPosition) * speed) * Time.deltaTime);
        transform.position = newPosition;

    }
}
