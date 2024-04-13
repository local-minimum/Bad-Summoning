using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public static Dictionary<Vector3Int, GridCell> Map;

    [SerializeField]
    GameObject Roof;
    [SerializeField]
    GameObject Floor;
    [SerializeField]
    GameObject North;
    [SerializeField]
    GameObject South;
    [SerializeField]
    GameObject West;
    [SerializeField]
    GameObject East;

    public bool HasWall(Direction direction) {
        switch (direction)
        {
            case Direction.North:
                return North.activeInHierarchy;
            case Direction.South:
                return South.activeInHierarchy;
            case Direction.West:
                return West.activeInHierarchy;                
            case Direction.East:
                return East.activeInHierarchy;
            default:
                return false;
        }
    }
}
