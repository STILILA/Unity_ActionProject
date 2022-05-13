using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMotion : MonoBehaviour
{
	[SerializeField] LayerMask groundMask;

	public Animator animator;
	[SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
	protected Rigidbody2D rig2d;

	public BoxCollider2D groundChecker;

	public string startState = "stand";
	public string currentState = "stand";
	public int currentFrameNo = 0;
	public string spriteBundleName = "unitychan";
	public bool canCancel = false;
	public bool onAir;
	

	public float walkSpeed = 3;
	public float runSpeed = 5;
	public float airSpeed = 3;

	public float fricX = 2;
	public float jumpPower = 5;

	public GameObject tempBody;  // 為了掛BoxCollider用，直接new出來會回傳null，幹你Unity
	public GameObject tempAtk;
	

	protected int wait = 0;
	protected int nextFrameNo = 0;
	protected int stateTime = 0;
	protected string nextState = string.Empty;
	protected string nextMethod = string.Empty;

	[SerializeField] float speedX = 0;
	[SerializeField] float speedY = 0;
	[SerializeField] float gravity = -0.2f;
	[SerializeField] float gravityMax = -9;
	[SerializeField] int dir = 1;
	public List<BoxCollider2D> bodyRects = new List<BoxCollider2D>();
	public List<BoxCollider2D> atkRects = new List<BoxCollider2D>();

	//

	protected List<string> atkState = new List<string>() { "atk1"};

	protected JsonData stateData = new JsonData();
	




	void Awake() {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		rig2d = GetComponent<Rigidbody2D>();

		if (!tempBody.GetComponent<MotionGetter>()) {
			tempBody.AddComponent<MotionGetter>();
		}
		tempBody.GetComponent<MotionGetter>().motion = this;
		if (!tempAtk.GetComponent<MotionGetter>()) {
			tempAtk.AddComponent<MotionGetter>();
		}
		tempAtk.GetComponent<MotionGetter>().motion = this;


		SetupAnime();
	}


	void Start() {
		ChangeState(startState);
	}

	public virtual void SetupAnime() {

		var animes = @"{
			'stand':[
				{'pic':'Unitychan_Idle_1', 'wait':8, 'nextNo':1, 'body':[[-0.03774764, -0.02141318, 0.2443573, 0.4237701]]},
				{'pic':'Unitychan_Idle_2', 'wait':8, 'nextNo':2 },
				{'pic':'Unitychan_Idle_3', 'wait':8, 'nextNo':3 },
				{'pic':'Unitychan_Idle_4', 'wait':8, 'nextNo':4 },
				{'pic':'Unitychan_Idle_1', 'wait':8, 'nextNo':1 }
			],
			
			'run':[
				{'pic':'Unitychan_Run_3', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Run_4', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Run_5', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Run_6', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Run_7', 'wait':3, 'nextNo':5 },
				{'pic':'Unitychan_Run_8', 'wait':3, 'nextNo':6 },
				{'pic':'Unitychan_Run_9', 'wait':3, 'nextNo':7 },
				{'pic':'Unitychan_Run_10', 'wait':3, 'nextNo':8 },
				{'pic':'Unitychan_Run_11', 'wait':3, 'nextNo':9 },
				{'pic':'Unitychan_Run_12', 'wait':3, 'nextNo':10 },
				{'pic':'Unitychan_Run_13', 'wait':3, 'nextNo':11 },
				{'pic':'Unitychan_Run_14', 'wait':3, 'nextNo':4 }
			],

			'crouchStart':[
				{'pic':'Unitychan_Crouch_1', 'wait':3, 'nextNo':1, 'body':[[-0.03774764, -0.02141318, 0.2443573, 0.4237701]]},
				{'pic':'Unitychan_Crouch_2', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Crouch_3', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Crouch_4', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Crouch_5', 'wait':3, 'nextState':'crouch' }
			],
			'crouch':[
				{'pic':'Unitychan_Crouch_5', 'wait':-1, 'nextNo':0}
			],
			'crouchEnd':[
				{'pic':'Unitychan_Crouch_2', 'wait':3, 'nextNo':1, 'body':[[-0.03774764, -0.02141318, 0.2443573, 0.4237701]]},
				{'pic':'Unitychan_Crouch_1', 'wait':3, 'nextState':'stand' }
			],

			'jumpStart':[
				{'pic':'Unitychan_Jump_Landing', 'wait':3, 'nextState':'jumpUp'}

			],
			'jumpUp':[
				{'pic':'Unitychan_Jump_Up_1', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Jump_Up_2', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Jump_Up_1', 'wait':3, 'nextNo':1 }
			],
			'jumpMid':[
				{'pic':'Unitychan_Jump_MidAir_1', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Jump_MidAir_2', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Jump_MidAir_3', 'wait':3, 'nextState':'jumpFall' }
			],
			'jumpFall':[
				{'pic':'Unitychan_Jump_Fall_1', 'wait':3, 'nextNo':1},
				{'pic':'Unitychan_Jump_Fall_2', 'wait':3, 'nextNo':0 }
			],
			'landing':[
				{'pic':'Unitychan_Jump_Landing', 'wait':5, 'nextState':'stand' }
			],


			'atk1':[
				{'pic':'Unitychan_Soard_Combo_2', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_3', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_4', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_5', 'wait':3, 'nextNo':4, 'atk':[[0.14, -0.08, 0.52, 0.35]] },
				{'pic':'Unitychan_Soard_Combo_6', 'wait':4, 'nextNo':5, 'atk':[], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_7', 'wait':7, 'nextMethod':'ActionEnd' }
			],

			'atk2':[
				{'pic':'Unitychan_Soard_Combo_8', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_9', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_10', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_11', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Soard_Combo_12', 'wait':3, 'nextNo':5, 'atk':[[0.1599156, 0.08680066, 0.5492049, 0.6929471]] },
				{'pic':'Unitychan_Soard_Combo_13', 'wait':3, 'nextNo':6 },
				{'pic':'Unitychan_Soard_Combo_14', 'wait':4, 'nextNo':7, 'atk':[], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_15', 'wait':7, 'nextMethod':'ActionEnd' }
			],

			'atk3':[
				{'pic':'Unitychan_Soard_Combo_16', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_17', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_18', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_19', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Soard_Combo_20', 'wait':3, 'nextNo':5, 'atk':[[0.2902966, 0.05575758, 0.768576, 0.6308609]] },
				{'pic':'Unitychan_Soard_Combo_21', 'wait':3, 'nextNo':6, 'atk':[[0.5075981, -0.07876238, 0.5078144, 0.361821]] },
				{'pic':'Unitychan_Soard_Combo_22', 'wait':3, 'nextNo':7, 'atk':[[0.553128, -0.07876238, 0.598874, 0.361821]] },
				{'pic':'Unitychan_Soard_Combo_23', 'wait':3, 'nextNo':8, 'atk':[[0.7580125, -0.1346399, 0.3877811, 0.2500659]] },
				{'pic':'Unitychan_Soard_Combo_24', 'wait':3, 'nextNo':9, 'atk':[[0.8200988, -0.1822393, 0.2636087, 0.1548671]], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_25', 'wait':12, 'nextMethod':'ActionEnd', 'atk':[] }
			],

			'gun1':[
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Hundgun1_3', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Hundgun2_5', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Hundgun2_6', 'wait':3, 'nextNo':5, 'atk':[[0.9590002, 0.02490908, 1.971455, 0.5018182]] },
				{'pic':'Unitychan_Hundgun2_7', 'wait':3, 'nextNo':6 },
				{'pic':'Unitychan_Hundgun2_8', 'wait':3, 'nextNo':7 },
				{'pic':'Unitychan_Hundgun2_9', 'wait':3, 'nextNo':8, 'atk':[], 'selfCancel':true, 'cancel' : true },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':9 },
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextMethod':'ActionEnd' }
			],
			'gun1_hold':[
				{'pic':'Unitychan_Hundgun2_6', 'wait':3, 'nextNo':1, 'atk':[[0.9590002, 0.02490908, 1.971455, 0.5018182]] },
				{'pic':'Unitychan_Hundgun2_7', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Hundgun2_8', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Hundgun2_9', 'wait':3, 'nextNo':4, 'atk':[], 'selfCancel':true, 'cancel' : true },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':5 },
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextMethod':'ActionEnd' }
			]


}";

		this.stateData = JsonMapper.ToObject(animes);
	}


	private void Update() {

		UpdateMotion();

		onAir = IsOnAir();

		if (currentState == "damage1" || currentState == "damage2") {
			if (currentFrameNo == 1) {
				if (onAir) {
					ChangeState("jumpFall");
				} else {
					ChangeState("stand");
				}
				
			}
		}



		Vector2 velocity = rig2d.velocity;

		if (currentState == "walk") {
			speedX = walkSpeed * dir;
		}
		if (currentState == "run") {
			speedX = runSpeed * dir;
		}

		if (currentState == "jumpUp") {
			if (stateTime == 0) {
				speedY = jumpPower;
			}
			if (speedY <= 2) {
				ChangeState("jumpMid");
			}
		}


		velocity.x = speedX;
		velocity.y = speedY;

		rig2d.velocity = velocity;

		// X速度扣除
		if (speedX > 0) {
			speedX = Mathf.Max(speedX - fricX, 0);
		}
		else {
			speedX = Mathf.Min(speedX + fricX, 0);
		}
		// Y速度扣除
		if (IsOnAir()) {
			speedY += gravity;
			if (speedY < gravityMax) {
				speedY = gravityMax; 
			}
		}
		else {
			if (currentState == "jumpFall") { ChangeState("landing"); speedY = 0; }
			//speedY = 0;
		}

		

	}

	void ChangeSprite(string filename) {
		if (filename != string.Empty) {
			var sp = AssetBundleManager.GetAsset<Sprite>(spriteBundleName, filename);
			spriteRenderer.sprite = sp;
		} else {
			spriteRenderer.sprite = null;
		}
	}


	void UpdateMotion() {

		stateTime++;


		if (wait > 0) { 
			wait--; 
			return; 
		}
		// 準備跳至狀態機下一格
		if (wait == 0) {
			// 有指定nextMethod的情況
			if (!string.IsNullOrEmpty(nextMethod)) {
				System.Reflection.MethodInfo mi = this.GetType().GetMethod(nextMethod);
				nextMethod = string.Empty;
				mi.Invoke(this, null);
				
			}
			// 不然有指定nextState的情況
			else if (!string.IsNullOrEmpty(nextState)) {
				ChangeState(nextState);
			}
			// 都沒指定的情況
			else {
				UpdateFrame(nextFrameNo);
			}
		}
	}

	void UpdateFrame(int frameNo) {

		currentFrameNo = frameNo;
		var frameData = stateData[currentState][frameNo];

		foreach (var param in frameData.Keys) {
			switch (param) {
				case "pic":
					ChangeSprite( (string)frameData[param] );
					break;
				case "wait":
					wait = (int)frameData[param];
					break;
				case "nextNo":
					nextFrameNo = (int)frameData[param];
					break;
				case "nextState":
					nextState = (string) frameData[param];
					break;
				case "nextMethod":
					nextMethod = (string) frameData[param];
					break;
				case "cancel":
					SetCanCancel();
					break;
				case "atk":
					SetRectByData(frameData[param], 10);
					break;
				case "body":
					SetRectByData(frameData[param], 9);
					break;
				case "speedX":
					speedX = (float) frameData[param] * dir;
					break;
				case "speedY":
					speedY = (float) frameData[param];
					break;
			}


		}



	}

	// 用animator運作
	//public void ChangeState(string stateName) {
	//	if (currentState != stateName) {
	//		canCancel = false;
	//		animator.Play(stateName);
	//		animator.Update(0);  // 這個要加，不然內部參數不會馬上改變(還是舊的動畫)
	//		currentState = stateName;
	//	}
	//}

	// 用自訂狀態機表運作
	public void ChangeState(string stateName) {
		//if (currentState != stateName) {
		if (!stateData.ContainsKey(stateName)) {
			Debug.LogError($"狀態機名稱：{stateName}不存在。");
			return;
		}
		ClearRects(null, 10);
		nextMethod = nextState = string.Empty; // 要清空，以免誤執行
		nextFrameNo = 0;
		wait = 0;
		stateTime = 0;
		canCancel = false;
		currentState = stateName;
		UpdateFrame(nextFrameNo);
		//}
	}


	public void DoAction(string stateName, bool forceCancel = false) {
		if ((currentState != stateName || forceCancel) && (!IsAtk() || IsCanCancel())) {
			ChangeState(stateName);
		}
	}

	public void ChangeDir(int dir) {
		if (dir == 0) { return; }
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

	public virtual bool IsAtkState() {
		return atkState.Contains(currentState);
	}


	public bool IsAtk() {
		return IsAtkState(); // C#的欄位不能override，只好這樣，ㄍㄋㄇ
	}




	/// <summary>
	/// layer：9身體、10攻擊
	/// </summary>
	/// <param name="rects"></param>
	/// <param name="layer"></param>
	public void SetRects(List<BoxCollider2D> rects, int layer) {
		if (layer == 9) {
			bodyRects.Clear();
			var comps = tempBody.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}

			foreach (var rect in rects) {
				bodyRects.Add(rect);
			}
		} else {
			atkRects.Clear();
			var comps = tempAtk.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}
			foreach (var rect in rects) {
				atkRects.Add(rect);
			}
		}
	}
	/// <summary>
	/// layer：9身體、10攻擊
	/// </summary>
	void SetRectByData(JsonData rectDatas, int layer) {
		if (layer == 9) {
			bodyRects.Clear();
			var comps = tempBody.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}
			foreach (JsonData bodyRect in rectDatas) {
				BoxCollider2D rect = tempBody.AddComponent<BoxCollider2D>();
				rect.isTrigger = true;
				rect.offset = new Vector2((float) (double) bodyRect[0], (float) (double) bodyRect[1]);
				rect.size = new Vector2((float) (double) bodyRect[2], (float) (double) bodyRect[3]);
				bodyRects.Add(rect);
			}
		}
		else {
			atkRects.Clear();
			var comps = tempAtk.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}
			foreach (JsonData atkRect in rectDatas) {
				BoxCollider2D rect = tempAtk.AddComponent<BoxCollider2D>();
				rect.isTrigger = true;
				rect.offset = new Vector2((float) (double) atkRect[0], (float) (double) atkRect[1]);
				rect.size = new Vector2((float) (double) atkRect[2], (float) (double) atkRect[3]);
				atkRects.Add(rect);
			}
		}
	}


	public void ClearRects(List<BoxCollider2D> rects = null, int layer = -1) {
		if (layer == -1) {
			atkRects.Clear();
			var comps = tempAtk.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}
			bodyRects.Clear();
			comps = tempBody.GetComponents<BoxCollider2D>();
			foreach (var comp in comps) {
				Destroy(comp);
			}
		} else {
	//		if (rects == null) { return; }
			if (layer == 9) {
				if (rects == null || rects.Count <= 0) {
					bodyRects.Clear();
					var comps = tempBody.GetComponents<BoxCollider2D>();
					foreach (var comp in comps) {
						Destroy(comp);
					}
				} else {
					foreach (var rect in rects) {
						bodyRects.Remove(rect);
						Destroy(rect);
					}
				}

			} else {
				if (rects == null || rects.Count <= 0) {
					atkRects.Clear();
					var comps = tempAtk.GetComponents<BoxCollider2D>();
					foreach (var comp in comps) {
						Destroy(comp);
					}
				} else {
					foreach (var rect in rects) {
						atkRects.Remove(rect);
						Destroy(rect);
					}
				}

			}
		}

	}


	public virtual void DoNoInput() {
		if (IsAtk()) { return; }

		if (onAir) {
			if (currentState != "jumpStart" && currentState != "jumpUp" && currentState != "jumpMid" && currentState != "jumpFall") {
				ChangeState("jumpFall");
			}
		}
		else {
			if (currentState == "crouch" || currentState == "crouchStart") {
				ChangeState("crouchEnd");
			} else if (currentState == "crouchEnd" || currentState == "landing") {
				// 什麼都不做
			} else if (currentState == "jumpStart" || currentState == "jumpUp" || currentState == "jumpMid") {

				// 什麼都不做
			}
			else {
				if (currentState != "stand") {
					ChangeState("stand");
				}
			}
		}
	}

	public virtual void DoWalk(int dir) {
		if (IsAtk()) { return; }
		ChangeDir(dir);
		
		if (onAir) {
			speedX = runSpeed * dir;
		}
		else {
			if (currentState == "jumpStart" || currentState == "jumpUp" || currentState == "walk") { return; }
			ChangeState("walk");
		}
	}




	public virtual void DoRun(int dir) {
		if (IsAtk()) { return; }
		ChangeDir(dir);
		if (onAir) {
			speedX = runSpeed * dir;
			//ChangeState("jump");
		}
		else {
			if (currentState == "jumpStart" || currentState == "jumpUp" || currentState == "run") { return; }
			ChangeState("run");
		}
	}

	public virtual void DoCrouch(int dir) {
		if (IsAtk()) { return; }
		if (onAir) { return; }
		ChangeDir(dir);
		if (currentState == "jumpStart" || currentState == "jumpUp") { return; }
		switch (currentState) {
			case "crouchStart":
				break;
			case "crouch":
				break;
			default:
				ChangeState("crouchStart");
				break;
		}
	}
	public virtual void DoJump(int dir) {
		if (onAir) { return; }
		if (currentState == "jumpStart") { return; }
		if (!IsAtk() || IsCanCancel()) {
			ChangeState("jumpStart");
		}
	}


	public virtual void DoAction_Z() {
	}

	public virtual void DoAction_6Z() {

	}
	public virtual void DoAction_2Z() {

	}
	public virtual void DoAction_X() {

	}
	public virtual void DoAction_6X() {

	}
	public virtual void DoAction_2X() {

	}

	public virtual void ActionEnd() {
		ClearRects();
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

	// 動畫事件
	void AniEV_ActionEnd() {
		ActionEnd();
	}
	void AniEV_SetCanCancel() {
		SetCanCancel();
	}
}
