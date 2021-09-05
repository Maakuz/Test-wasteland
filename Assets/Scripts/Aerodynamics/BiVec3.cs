using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiVec3
{
    public Vector3 force;
    public Vector3 torque;

    public BiVec3() 
    {
       
    }

    public BiVec3(Vector3 force, Vector3 torque)
    {
        this.force = force;
        this.torque = torque;
    }

    public static BiVec3 operator+(BiVec3 a, BiVec3 b)
    {
        return new BiVec3(a.force + b.force, a.torque + b.torque);
    }

    public static BiVec3 operator*(float a, BiVec3 b)
    {
        return new BiVec3(a * b.force, a * b.torque);
    }

    public static BiVec3 operator*(BiVec3 a, float b)
    {
        return b * a;
    }
}
