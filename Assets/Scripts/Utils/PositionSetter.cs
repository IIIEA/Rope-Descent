using UnityEngine;

public static class PositionSetter
{
    public static Vector3 SetPositionX(Vector3 position, float newPosition)
    {
        return new Vector3(newPosition, position.y, position.z);
    }

    public static Vector3 SetPositionY(Vector3 position, float newPosition)
    {
        return new Vector3(position.x, newPosition, position.z);
    }

    public static Vector3 SetPositionZ(Vector3 position, float newPosition)
    {
        return new Vector3(position.x, position.y, newPosition);
    }
}
