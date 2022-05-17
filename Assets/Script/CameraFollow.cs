using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	Transform target;
	public float leftLimit = 0.68f;
	public float rightLimit = 5000;


	private void Start() {
		target = FindObjectOfType<MyMoveCharacterAction>().transform;
	}

	private void Update() {
		transform.position = new Vector3(Mathf.Min(Mathf.Max(target.transform.position.x, leftLimit), rightLimit), 0, -23.4f);
	}


}
