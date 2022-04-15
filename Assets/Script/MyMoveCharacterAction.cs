using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyMoveCharacterAction : MonoBehaviour
{
	//static int hashSpeed = Animator.StringToHash ("Speed");
	//static int hashFallSpeed = Animator.StringToHash ("FallSpeed");
	//static int hashGroundDistance = Animator.StringToHash ("GroundDistance");
	//static int hashIsCrouch = Animator.StringToHash ("IsCrouch");

	//static int hashDamage = Animator.StringToHash ("Damage");

	[SerializeField] LayerMask groundMask;

	[SerializeField, HideInInspector] Animator animator;
	[SerializeField, HideInInspector]SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector]Rigidbody2D rig2d;

	//
	public BoxCollider2D groundChecker;


	public int hp = 4;
	public int dir = 1;
	public float speedX = 5;
	public float fricX = 2;
	public float jumpPower = 5;


	void Awake ()
	{
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rig2d = GetComponent<Rigidbody2D> ();

	}

	void Update ()
	{

		bool onAir = !Physics2D.OverlapBox(groundChecker.bounds.center, groundChecker.bounds.extents * 2, 0, groundMask);
	//	Debug.Log(onAir);

		bool isInput = false;

		var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

		Vector2 velocity = rig2d.velocity;
		if (Input.GetKey(KeyCode.LeftArrow)) {
			dir = -1;
			if (!currentStateInfo.IsName("run") && !onAir) { animator.Play("run"); }
			isInput = true;
			velocity.x = speedX * dir;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			dir = 1;
			if (!currentStateInfo.IsName("run") && !onAir) { animator.Play("run"); }
			isInput = true;
			velocity.x = speedX * dir;
		}
		

		if (velocity.x > 0) {
			velocity.x = Mathf.Max(velocity.x - fricX, 0);
		} else {
			velocity.x = Mathf.Min(velocity.x + fricX, 0);
		}

		var scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * dir;
		transform.localScale = scale;

		if (Input.GetKey(KeyCode.DownArrow)) {
			if (!currentStateInfo.IsName("crouch") && !onAir) { animator.Play("crouch"); }
			isInput = true;
		} else {
			if (currentStateInfo.IsName("crouch")) {
				animator.Play("stand");
			}
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			velocity.y = 5;
			animator.Play("jump");
			isInput = true;
		}

        rig2d.velocity = velocity;

		if (!onAir && !isInput && !currentStateInfo.IsName("stand")) {
			animator.Play("stand");
		}
		if (onAir && !currentStateInfo.IsName("jump")) {
			animator.Play("jump");
		}

		// 目前動畫播完一輪
		if (currentStateInfo.normalizedTime >= 1) {
			Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name + " is End"); // 獲取個目前動畫名稱真的老木麻煩
		}


	}
    

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    animator.SetTrigger(hashDamage);  
    //}
}
