using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyMoveCharacterAction : MonoBehaviour
{

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

		// 處理預約指令
		if (motion.commandPlan != null && motion.CanAction()) {
			motion.commandPlan.Invoke();
			return;
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
		if (Input.GetKey(KeyCode.UpArrow)) {
			motion.DoJump(dir);
			isInput = true;
		}
		//}


		// 
		if (Input.GetKeyDown(KeyCode.Z)) {
			if (Input.GetKey(KeyCode.RightArrow)) {
				if (motion.dir == 1) {
					motion.Cmd_6Z();
				} else {
					motion.Cmd_4Z();
				}
			} else if (Input.GetKey(KeyCode.LeftArrow)) {
				if (motion.dir == -1) {
					motion.Cmd_6Z();
				}
				else {
					motion.Cmd_4Z();
				}
			} else if (Input.GetKey(KeyCode.DownArrow))	{
				motion.Cmd_2Z();
			} else {
				motion.Cmd_Z();
			}
			isInput = true;
		}

		if (Input.GetKeyDown(KeyCode.X)) {
			if (Input.GetKey(KeyCode.RightArrow)) {
				if (motion.dir == 1) {
					motion.Cmd_6X();
				}
				else {
					motion.Cmd_4X();
				}
			}
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				if (motion.dir == -1) {
					motion.Cmd_6X();
				}
				else {
					motion.Cmd_4X();
				}
			}
			else if (Input.GetKey(KeyCode.DownArrow)) {
				motion.Cmd_2X();
			}
			else {
				motion.Cmd_X();
			}
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
