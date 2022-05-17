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
	public string spriteBundleName = "unitychan";
	public GameObject tempBody;  // 為了掛BoxCollider用，直接new出來會回傳null，幹你Unity
	public GameObject tempAtk;

	// 監視用
	public bool canCancel = false;
	public bool onAir;

	public int hp = 500;
	public int maxhp = 500;
	public float walkSpeed = 3;
	public float runSpeed = 5;
	public float airSpeed = 3;

	public float fricX = 2;
	public float jumpPower = 5;



	public string currentState = "stand";
	public int currentFrameNo = 0;
	protected int wait = 0;
	protected int nextFrameNo = 0;
	protected int stateTime = 0;
	protected string nextState = string.Empty;
	protected string nextMethod = string.Empty;

	[SerializeField] int hitstop = 0;
	[SerializeField] int hitstun = 0;
	[SerializeField] float speedX = 0;
	[SerializeField] float speedY = 0;
	[SerializeField] float gravity = -0.2f;
	[SerializeField] float gravityMax = -9;
	public int dir = 1;
	public Action commandPlan = null;
	[SerializeField] int commandPlanCount = 0;
	public List<BoxCollider2D> bodyRects = new List<BoxCollider2D>();
	public List<BoxCollider2D> atkRects = new List<BoxCollider2D>();
	public List<GameMotion> hitTargets = new List<GameMotion>();

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
				{'pic':'Unitychan_Idle_1', 'wait':8, 'nextNo':1, 'body':[[-0.03774764, -0.02141318, 0.2443573, 0.4237701]], 'z':1},
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
				{'pic':'Unitychan_Jump_Landing', 'wait':3, 'nextState':'jumpUp', 'z':1}

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
			'damage1':[
				{'pic':'Toko_Damage_3', 'wait':2, 'nextNo':1, 'z':-1 },
				{'pic':'Toko_Damage_4', 'wait':-1, 'nextNo':1 }
			],
			'damage2':[
				{'pic':'Toko_Damage_3', 'wait':2, 'nextNo':1, 'z':-1 },
				{'pic':'Toko_Damage_4', 'wait':-1, 'nextNo':1 }
			],

			'atk1':[
				{'pic':'Toko_Punch_2', 'wait':2, 'nextNo':1, 'z':10 },
				{'pic':'Toko_Punch_3', 'wait':2, 'nextNo':2 },
				{'pic':'Toko_Punch_4', 'wait':2, 'nextNo':3, 'atk':[[0.14, -0.08, 0.52, 0.35]] },
				{'pic':'Toko_Punch_5', 'wait':2, 'nextNo':4 },
				{'pic':'Toko_Punch_6', 'wait':2, 'nextNo':5, 'atk':[], 'cancel' : true },
				{'pic':'Toko_Punch_7', 'wait':2, 'nextNo':6 },
				{'pic':'Toko_Punch_8', 'wait':2, 'nextNo':7 },
				{'pic':'Toko_Punch_9', 'wait':2, 'nextNo':8 },
				{'pic':'Toko_Punch_10', 'wait':2, 'nextMethod':'ActionEnd' }
			]


}";

		this.stateData = JsonMapper.ToObject(animes);
	}


	private void Update() {
		onAir = IsOnAir();

		UpdateMotion();

		onAir = IsOnAir();


		if (currentState == "damage1" || currentState == "damage2") {
			if (currentFrameNo == 1 && hitstun <= 0) {
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

	public void ChangeDir(int dir) {
		if (dir == 0) { return; }
		this.dir = dir;
		var scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * dir;
		transform.localScale = scale;
	}
	// 用自訂狀態機表運作
	public void ChangeState(string stateName) {
		//if (currentState != stateName) {
		if (!stateData.ContainsKey(stateName)) {
			Debug.LogError($"狀態機名稱：{stateName}不存在。");
			return;
		}
		ClearAtkRects();
		nextMethod = nextState = string.Empty; // 要清空，以免誤執行
		hitTargets.Clear();
		nextFrameNo = 0;
		wait = 0;
		stateTime = 0;
		canCancel = false;
		currentState = stateName;
		UpdateFrame(nextFrameNo);
		//}
	}

	void UpdateMotion() {

		if (hitstop > 0) {
			hitstop--;
			return;
		}

		stateTime++;

		if (commandPlanCount > 0) {
			commandPlanCount--;
			if (commandPlanCount == 0) {
				commandPlan = null;
			}
		}

		if (wait > 0) { 
			wait--; 
			return; 
		}
		if (hitstun > 0) { hitstun--; }

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
					SetRectByData(frameData[param], Global.atkLayer);
					break;
				case "body":
					SetRectByData(frameData[param], Global.bodyLayer);
					break;
				case "speedX":
					speedX = (float) frameData[param] * dir;
					break;
				case "speedY":
					speedY = (float) frameData[param];
					break;
				case "z":
					spriteRenderer.sortingOrder = (int) frameData[param];
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
		if (layer == Global.bodyLayer) {
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
		if (layer == Global.bodyLayer) {
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
			if (layer == Global.bodyLayer) {
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
	protected void ClearAtkRects() {
		ClearRects(null, Global.atkLayer);
	}
	protected void ClearBodyRects() {
		ClearRects(null, Global.bodyLayer);
	}

	public bool CanAction() {
		return (!IsAtk() || IsCanCancel()) && !IsDamaging();
	}


	public bool IsDamaging() {
		return hitstun > 0;
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
			if (currentState != "jumpStart" && currentState != "jumpUp" && currentState != "jumpMid" && currentState != "jumpFall") {
				ChangeState("jumpFall");
			}
				
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
			if (currentState != "jumpStart" && currentState != "jumpUp" && currentState != "jumpMid" && currentState != "jumpFall") {
				ChangeState("jumpFall");
			}
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


	public virtual void Cmd_Z() {
	}

	public virtual void Cmd_6Z() {

	}
	public virtual void Cmd_4Z() {

	}
	public virtual void Cmd_2Z() {

	}
	public virtual void Cmd_X() {

	}
	public virtual void Cmd_6X() {

	}
	public virtual void Cmd_2X() {

	}
	public virtual void Cmd_4X() {

	}


	public void DoAction(string stateName, Action cmd, bool forceCancel = false, bool needTurn = false) {
		if ((currentState != stateName || forceCancel)) {
			// 可實行指令的情況
			if (!IsAtk() || IsCanCancel()) {
				if (needTurn) { ChangeDir(dir *= -1); }
				ChangeState(stateName);
			} else {
				// 預約輸入
				commandPlan = cmd;
				commandPlanCount = 6;
			}
		}
	}

	public virtual void DamageFormula(int damage) {
		hp = Mathf.Min(maxhp, Mathf.Max(hp - damage, 0));
	}


	public virtual void DoDamage(GameMotion atker) {
		var skill = atker.SkillEffect(atker);
		if (skill == null) { return; }

		if (skill.hitstun != null) { hitstun = (int)skill.hitstun; }
		if (skill.hitstop != null) { hitstop = atker.hitstop = (int)skill.hitstop; }
		if (skill.damage != null) { DamageFormula((int)skill.damage); }


		//if (hp == 0) {
		//	DoDead();
		//	return;
		//}

		switch (skill.damageType) {
			case DamageType.None:
				break;
			case DamageType.Blow:
				break;
			case DamageType.HardBlow:
				break;
			default:
				ChangeState(UnityEngine.Random.Range(0, 2) == 0 ? "damage1" : "damage2");
				break;
		}
		
	}

	public void DoDead() {

	}



	public virtual void ActionEnd() {
		ClearAtkRects();
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


	public virtual SkillEffectData SkillEffect(GameMotion targetMotion) {
		var se = new SkillEffectData();
		return se;
	}



	// 動畫事件
	void AniEV_ActionEnd() {
		ActionEnd();
	}
	void AniEV_SetCanCancel() {
		SetCanCancel();
	}
}


public class SkillEffectData{
	public int? damage = null;
	public int? hitstun = null;
	public int? hitstop = null;
	public DamageType damageType = DamageType.Normal;
}
public enum DamageType {
	None,  // 沒反應
	Normal, // 站姿
	Blow,  // 擊飛態
	HardBlow // 重擊飛態
}
