using UnityEngine;

[CreateAssetMenu(fileName = "MovementOptions", menuName = "Options/MovementOptions", order = 0)]
public class MovementOptions : ScriptableObject
{
    public enum MovementType
    {
        Cartesian,
        IsometricUpLeft,
        IsometricUpRight,
        Hybrid,
    }

    public MovementType movementType;
    
    public Vector2 GetTransformedMovementDirection(float x, float y)
    {
        switch (movementType)
        {
            case MovementType.Cartesian:
                return GetCartesianTransformation(x, y);
            case MovementType.IsometricUpLeft:
                return GetIsometricUpLeftTransformation(x, y);
            case MovementType.IsometricUpRight:
                return GetIsometricUpRightTransformation(x, y);
            case MovementType.Hybrid:
                return GetHybridTransformation(x, y);
            default:
                return GetCartesianTransformation(x, y);
        }
    }

    public static Vector2 GetCartesianTransformation(float x, float y)
    {
        return new Vector2(x, y);
    }

    public static Vector2 GetIsometricUpLeftTransformation(float x, float y)
    {
        return new Vector2
        {
            x = x - y,
            y = (x + y) / 2
        };
    }
    
    public static Vector2 GetIsometricUpRightTransformation(float x, float y)
    {
        return new Vector2
        {
            x = x + y,
            y = (-x + y) / 2
        };
    }
    
    public static Vector2 GetHybridTransformation(float x, float y)
    {
        return new Vector2(x, y / 2);
    }
}