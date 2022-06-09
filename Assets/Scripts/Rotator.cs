using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public Vector3 RotatePosition;
    void Update()
    { transform.Rotate(RotatePosition * Time.deltaTime); }
}