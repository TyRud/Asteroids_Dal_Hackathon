﻿using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}


}
