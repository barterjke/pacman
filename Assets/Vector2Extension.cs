using System;
using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 Sign(this Vector2 vec)
    {
        return new Vector2(Math.Sign(vec.x), Math.Sign(vec.y));
    }
    
    public static Vector3 Sign(this Vector3 vec)
    {
        return new Vector3(Math.Sign(vec.x), Math.Sign(vec.y), Math.Sign(vec.z));
    }
}