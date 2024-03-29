﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterAction : MonoBehaviour
{
	static int hashSpeed = Animator.StringToHash ("Speed");
	static int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	static int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	static int hashIsCrouch = Animator.StringToHash ("IsCrouch");

	static int hashDamage = Animator.StringToHash ("Damage");

	[SerializeField] private float characterHeightOffset = 0.2f;
	[SerializeField] LayerMask groundMask;

	[SerializeField, HideInInspector] Animator animator;
	[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;

	public int hp = 4;
	public int dir = 1;


	void Awake ()
	{
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		float axis = Input.GetAxis ("Horizontal");
		bool isDown = Input.GetAxisRaw ("Vertical") < 0;

        Vector2 velocity = rig2d.velocity;
		if (Input.GetButtonDown ("Jump")) {
			velocity.y = 5;
		}
		if (axis != 0 && !isDown){
			//spriteRenderer.flipX = axis < 0;
			dir = axis < 0 ? -1 : 1 ;

			var scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x) * dir;
			transform.localScale = scale;


			velocity.x = axis * 2;
        } else {
			velocity.x = 0;
		}

	//	if (isDown && !animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")) {
	//		animator.Play("Crouch");
	//	}

        rig2d.velocity = velocity;


		var distanceFromGround = Physics2D.Raycast (transform.position, Vector3.down, 1, groundMask);
		//animator.Play("Stand");
		// update animator parameters
		
		animator.SetFloat (hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat (hashFallSpeed, rig2d.velocity.y);
		animator.SetFloat(hashSpeed, Mathf.Abs(axis));
		animator.SetBool(hashIsCrouch, isDown);


	}
    

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    animator.SetTrigger(hashDamage);  
    //}
}
