using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    void OnEnable()
    {
        target = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(target.position);
    }
}

