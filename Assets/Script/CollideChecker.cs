using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideChecker : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == 8) { return; }
	//	Debug.Log($"{this.name}, {col.name}");
	}
}
