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
	public string currentState = "stand";
	private bool canCancel;

	readonly List<string> atkState = new List<string>() { "atk1", "atk2", "atk3" };


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

		AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

		Vector2 velocity = rig2d.velocity;
		if (!IsAtk()) {
			if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow)) {
				dir = -1;
				if (!onAir) { ChangeState("run"); }
				isInput = true;
				velocity.x = speedX * dir;
			}
			if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow)) {
				dir = 1;
				if (!onAir) { ChangeState("run"); }
				isInput = true;
				velocity.x = speedX * dir;
			}
		}


		if (velocity.x > 0) {
			velocity.x = Mathf.Max(velocity.x - fricX, 0);
		} else {
			velocity.x = Mathf.Min(velocity.x + fricX, 0);
		}

		var scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * dir;
		transform.localScale = scale;
		if (!IsAtk()) {
			if (Input.GetKey(KeyCode.DownArrow)) {
				if (!onAir) { ChangeState("crouch"); }
				isInput = true;
			}
			else {
				if (currentStateInfo.IsName("crouch")) {
					ChangeState("stand");
				}
			}
		}
		if (!IsAtk() || IsCanCancel()) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				velocity.y = 5;
				ChangeState("jump");
				isInput = true;
			}
		}


		

		// 
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (!onAir) {
				switch (currentState) {
					case "atk1":
						DoAction("atk2");
						break;
					case "atk2":
						DoAction("atk3");
						break;
					default:
						DoAction("atk1");
						break;
				}
				
			}

			isInput = true;
		}

		rig2d.velocity = velocity;

		if (!IsAtk()) {
			if (!onAir && !isInput) {
				ChangeState("stand");
			}
			if (onAir) {
				ChangeState("jump");
			}
		}






		// 目前動畫播完一輪
		//if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) { // stateInfo要重拿，哭啊
		//	Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name + " is End"); // 獲取個目前動畫名稱真的老木麻煩
		//	ActionEnd();
		//}


	}
    

	void ChangeState(string stateName) {
		
		if (currentState != stateName) {
			canCancel = false;
			animator.Play(stateName);
			animator.Update(0);  // 這個要加，不然內部參數不會馬上改變(還是舊的動畫)
			currentState = stateName;
		}
	}

	void DoAction(string stateName) {
		if (currentState != stateName && (IsCanCancel() || !IsAtk())) {
			ChangeState(stateName);
		}
		
	}

	void SetCanCancel() {
		canCancel = true;
	}

	bool IsCanCancel() {
		return canCancel;
	}

	bool IsAtk() {
		return atkState.Contains(currentState);
	}


	// 動畫事件
	void AniEV_ActionEnd() {
		switch (currentState) {
			case "atk1":
				ChangeState("stand");
				break;
			case "atk2":
				ChangeState("stand");
				break;
			case "atk3":
				ChangeState("stand");
				break;
			default:
				break;
		}
		canCancel = false;
	}
	void AniEV_SetCanCancel() {
		SetCanCancel();
	}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    animator.SetTrigger(hashDamage);  
    //}
}
