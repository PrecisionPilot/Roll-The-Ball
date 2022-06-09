using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    public float Seconds;
	void Start () {
        Destroy(this.gameObject, Seconds);
	}
}
