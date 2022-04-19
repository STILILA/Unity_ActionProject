using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMotion : MonoBehaviour
{
	[SerializeField] LayerMask groundMask;

	public Animator animator;
	[SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
	Rigidbody2D rig2d;

	public BoxCollider2D groundChecker;

	public string currentState = "stand";
	private bool canCancel;
	public bool onAir;
	public int dir = 1;
	public float speedX = 5;
	public float fricX = 2;
	public float jumpPower = 5;


	//

	readonly List<string> atkState = new List<string>() { "atk1", "atk2", "atk3" };


	void Awake() {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		rig2d = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		onAir = IsOnAir();


		Vector2 velocity = rig2d.velocity;

		if (currentState == "run") {
			velocity.x = speedX * dir;
		}
		if (velocity.x > 0) {
			velocity.x = Mathf.Max(velocity.x - fricX, 0);
		}
		else {
			velocity.x = Mathf.Min(velocity.x + fricX, 0);
		}

		rig2d.velocity = velocity;


	}


	public void ChangeState(string stateName) {
		if (currentState != stateName) {
			canCancel = false;
			animator.Play(stateName);
			animator.Update(0);  // 這個要加，不然內部參數不會馬上改變(還是舊的動畫)
			currentState = stateName;
		}
	}

	public void DoAction(string stateName) {
		if (currentState != stateName && (!IsAtk() || IsCanCancel())) {
			ChangeState(stateName);
		}
	}

	public void ChangeDir(int dir) {
		this.dir = dir;
		var scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * dir;
		transform.localScale = scale;
	}

	void SetCanCancel() {
		canCancel = true;
	}

	public bool IsOnAir() {
		return !Physics2D.OverlapBox(groundChecker.bounds.center, groundChecker.bounds.extents * 2, 0, groundMask);
	}

	public bool IsCanCancel() {
		return canCancel;
	}

	public bool IsAtk() {
		return atkState.Contains(currentState);
	}


	public void DoNoInput() {
		if (IsAtk()) { return; }

		if (onAir) {
			ChangeState("jump");
		} else {
			if (currentState == "crouch") {
				ChangeState("stand");
			} else {
				ChangeState("stand");
			}
		}
	}

	public void DoWalk(int dir) {
		if (IsAtk()) { return; }
		ChangeDir(dir);
		if (onAir) {

			Vector2 velocity = rig2d.velocity;
			velocity.x = speedX * dir;
			rig2d.velocity = velocity;

		} else {
			ChangeState("walk");
		}
	}




	public void DoRun(int dir) {
		if (IsAtk()) { return; }
		ChangeDir(dir);
		if (onAir) {

			Vector2 velocity = rig2d.velocity;
			velocity.x = speedX * dir;
			rig2d.velocity = velocity;

			ChangeState("jump");
		} else {
			ChangeState("run");
		}
	}

	public void DoCrouch(int dir) {
		if (IsAtk()) { return; }
		if (onAir) { return; }
		ChangeState("crouch");
	}
	public void DoJump(int dir) {
		if (onAir) { return; }
		if (!IsAtk() || IsCanCancel()) {
			ChangeState("jump");
			var velocity = rig2d.velocity;
			velocity.y = 5;
			rig2d.velocity = velocity;
		}
	}


	public void DoAction_Z() {
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
	}

	public void DoAction_6Z() {

	}
	public void DoAction_2Z() {

	}
	public void DoAction_X() {

	}
	public void DoAction_6X() {

	}
	public void DoAction_2X() {

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
}
