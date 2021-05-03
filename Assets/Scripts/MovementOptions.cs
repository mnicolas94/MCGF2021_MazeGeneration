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
                return new Vector2(x, y);
            case MovementType.IsometricUpLeft:
                var dirLeft = new Vector2();
                dirLeft.x = x - y;
                dirLeft.y = (x + y) / 2;
                return dirLeft;
            case MovementType.IsometricUpRight:
                var dirRight = new Vector2();
                dirRight.x = x + y;
                dirRight.y = (-x + y) / 2;
                return dirRight;
            case MovementType.Hybrid:
                return new Vector2(x, y / 2);
            default:
                return new Vector2(x, y);
        }
    }
}