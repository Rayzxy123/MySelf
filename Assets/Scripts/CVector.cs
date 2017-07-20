using UnityEngine;
using System.Collections;

public class CVector {

    public static Vector3 vec3X;
    public static Vector3 vec3Y;
    public static Vector3 vec3Z;
    
    public static void InitVector3()
    {

        vec3X = new Vector3(1.0f, 0.0f, 0.0f);
        vec3Y = new Vector3(0.0f, 1.0f, 0.0f);
        vec3Z = new Vector3(0.0f, 0.0f, 1.0f);
    }
}
