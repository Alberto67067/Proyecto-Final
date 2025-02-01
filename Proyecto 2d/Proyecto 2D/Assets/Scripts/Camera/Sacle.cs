using UnityEngine;
using System;

public class Sacle : MonoBehaviour
{
    [SerializeField] float Size = 15f;
    [SerializeField] Camera camera;
    void Update()
    {
        if(camera.orthographicSize >= 0)
        {
            camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Size;
        }
        else
        {
            camera.orthographicSize = 0;
        }
    }

}

