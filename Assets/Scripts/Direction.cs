using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, South, West, East };

public static class DirectionExtensions
{
    public static Direction RotateCCW(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.West;
            case Direction.West:
                return Direction.South;
            case Direction.South:
                return Direction.East;
            case Direction.East:
                return Direction.North;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    public static Direction RotateCW(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    public static Direction Inverse(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;
            case Direction.East:
                return Direction.West;
            case Direction.South:
                return Direction.North;
            case Direction.West:
                return Direction.East;
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }


    public static Vector2Int Translate(this Vector2Int coords, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(coords.x, coords.y + 1);
            case Direction.South:
                return new Vector2Int(coords.x, coords.y - 1);
            case Direction.West:
                return new Vector2Int(coords.x - 1, coords.y);
            case Direction.East:
                return new Vector2Int(coords.x + 1, coords.y);
            default:
                return coords;
        }
    }

    public static Vector2Int ToLookVector(this Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(0, 1);
            case Direction.South:
                return new Vector2Int(0, -1);
            case Direction.West:
                return new Vector2Int(-1, 0);
            case Direction.East:
                return new Vector2Int(1, 0);
            default:
                return Vector2Int.zero;
        }
    }    
}