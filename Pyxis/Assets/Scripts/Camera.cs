using System;
using UnityEngine;

public class Camera : MonoBehaviour
{

    Transform Target;
    public Vector3 Offset;
    Vector3 Velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] Objects = GameObject.FindGameObjectsWithTag("Player");
        if(Objects.Length == 0)
        {
            throw new Exception("No object with Player tag");
        }
        if(Objects.Length > 1)
        {
            throw new Exception("Too many objects with Player tag");
        }
        Target = Objects[0].transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = Target.position + Offset;
        Vector3 smoothTransform = Vector3.SmoothDamp(transform.position, targetPosition, ref Velocity, 1.0f);
        transform.position = smoothTransform;
    }
}
