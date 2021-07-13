using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(-5f, -2.5f, -10f);
    public float dampingTime = 0.3f;
    public  Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        MoveCamara(true);
    }

    public void ResetCamaraPosition()
    {
        MoveCamara(false);
    }

    public void MoveCamara (bool Smooth)
    {
        Vector3 destination = new Vector3(target.position.x - offset.x, offset.y, offset.z);
        if (Smooth)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position,
                destination, ref velocity, dampingTime);
        }
        else
        {
            this.transform.position = destination;
        }
    }
    
}
