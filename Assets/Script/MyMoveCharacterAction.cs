using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyMoveCharacterAction : MonoBehaviour
{

	public int dir = 1;

	public GameMotion motion;
	KeyCode? memoryKey;
	int memoryKeyCount = 0;


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

		

		if (memoryKeyCount > 0) {
			memoryKeyCount--;
			if (memoryKeyCount == 0) {
				memoryKey = null;
			}
		}


		if (memoryKey != null && motion.CanAction()) {
			switch ((KeyCode)memoryKey) {
				case KeyCode.Z:
					motion.DoAction_Z();
					break;
				case KeyCode.X:
					motion.DoAction_X();
					break;

			}
		}



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

			if (!motion.CanAction()) {
				SetMemoryKey(KeyCode.UpArrow);
				return;
			}

			motion.DoJump(dir);
			isInput = true;
		}
		//}


		// 
		if (Input.GetKeyDown(KeyCode.Z)) {

			if (!motion.CanAction()) {
				SetMemoryKey(KeyCode.Z);
				return;
			}

			motion.DoAction_Z();
			isInput = true;
		}
		if (Input.GetKeyDown(KeyCode.X)) {

			if (!motion.CanAction()) {
				SetMemoryKey(KeyCode.X);
				return;
			}

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
    


	void SetMemoryKey(KeyCode key) {
		memoryKey = key;
		memoryKeyCount = 6;
	}


}
