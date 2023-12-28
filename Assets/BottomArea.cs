using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomArea : MonoBehaviour
{
    public float returnPosX = 0;
    public float returnPosY = 0;




	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			collision.transform.position = new Vector2(returnPosX, returnPosY);
		}
	}

}
