using UnityEngine;
using System.Collections;

public class EnemyCharge : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, .99f);
	}
}
