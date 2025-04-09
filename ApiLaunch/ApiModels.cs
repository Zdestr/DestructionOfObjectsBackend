using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;
    
    public Vector3Data(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    
    public Vector3 ToUnityVector()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class CollisionRequest
{
    public string objectId;
    public float impactForce;
    public Vector3Data impactPoint;
    public Vector3Data impactDirection;
    public string objectType;
    public float objectMass;
    public Vector3Data objectDimensions;
}

[Serializable]
public class FragmentData
{
    public Vector3Data initialVelocity;
    public Vector3Data angularVelocity;
    public float mass;
}

[Serializable]
public class CollisionResponse
{
    public bool canBeDestroyed;
    public float destructionForce;
    public List<FragmentData> fragments;
    public float disappearTime;
    public float fadeTime;
}
