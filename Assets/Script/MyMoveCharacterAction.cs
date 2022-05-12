using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyMoveCharacterAction : MonoBehaviour
{

	public int hp = 4;
	public int dir = 1;

	public GameMotion motion;
	
	private void Awake() {
		motion = GetComponent<GameMotion>();
	}

	void Update ()
	{

		bool onAir = motion.IsOnAir();
	//	Debug.Log(onAir);

		bool isInput = false;
		var dir = 0;
		//AnimatorStateInfo currentStateInfo = motion.animator.GetCurrentAnimatorStateInfo(0);

		

		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow)) {
			dir = -1;
			motion.DoRun(dir);
			isInput = true;
			
		}
		if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow)) {
			dir = 1;
			motion.DoRun(dir);
			isInput = true;
		}
		

		if (Input.GetKey(KeyCode.DownArrow)) {

			if (Input.GetKey(KeyCode.RightArrow)){
				dir = 1;
			}
			if (Input.GetKey(KeyCode.LeftArrow)) {
				dir = -1;
			}

			motion.DoCrouch(dir);
			isInput = true;
		}

		//if (!motion.IsAtk() || motion.IsCanCancel()) {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			motion.DoJump(dir);
			isInput = true;
		}
		//}


		// 
		if (Input.GetKeyDown(KeyCode.Z)) {
			motion.DoAction_Z();
			isInput = true;
		}
		if (Input.GetKey(KeyCode.X)) {
			motion.DoAction_X();
			isInput = true;
		}


		if (!isInput) {
			motion.DoNoInput();
		}


		// 目前動畫播完一輪
		//if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) { // stateInfo要重拿，哭啊
		//	Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name + " is End"); // 獲取個目前動畫名稱真的老木麻煩
		//	ActionEnd();
		//}


	}
    


}
