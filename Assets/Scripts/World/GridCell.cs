using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public static Dictionary<Vector2Int, GridCell> Map = new Dictionary<Vector2Int, GridCell>();

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

    public GridCell Neighbour(Direction direction)
    {
        var neighbourCoords = Coords.Translate(direction);
        if (!Map.ContainsKey(neighbourCoords))
        {
            Debug.Log($"{name}/{Coords} has no {direction} neighbour at {neighbourCoords}");
            return null;
        }

        if (HasWall(direction))
        {
            Debug.Log($"{name}/{Coords} has a {direction} wall");
            return null;
        }

        return Map[neighbourCoords];
    }

    public bool Occupied => HasPlayer || HasEnemy;

    public bool HasPlayer { get; set; } = false;
    public bool HasEnemy { get; set; } = false;

    #region Coords
    bool coordsInited = false;
    Vector2Int _coords;
    public Vector2Int Coords
    {
        get { 
            if (!coordsInited)
            {
                coordsInited = true;
                _coords = transform.position.ToVector2Int();
            }
            return _coords; 
        }
    }
    #endregion

    private void Start()
    {
        
        if (Map.ContainsKey(Coords))
        {
            throw new System.ArgumentException($"Duplicate grid cells at {Coords}: {name} & {Map[Coords].name}");
        } else
        {
            Map.Add(Coords, this);
        }
    }

    private void OnDestroy()
    {
        Map.Remove(Coords);
    }    
}
