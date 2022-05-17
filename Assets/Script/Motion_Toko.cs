using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion_Toko : GameMotion
{
	new readonly List<string> atkState = new List<string>() { "atk1", "atk2", "atk3", "gun1", "gun1_hold" };




	public override void SetupAnime() {

		var animes = @"{
			'stand':[
				{'pic':'Toko_Idle_1', 'wait':8, 'nextNo':1, 'body':[[-0.001684546, -0.01715124, 0.200902, 0.4854634]], 'z':1},
				{'pic':'Toko_Idle_2', 'wait':8, 'nextNo':2 },
				{'pic':'Toko_Idle_3', 'wait':8, 'nextNo':3 },
				{'pic':'Toko_Idle_4', 'wait':8, 'nextNo':4 },
				{'pic':'Toko_Idle_1', 'wait':8, 'nextNo':1 }
			],
			
			'run':[
				{'pic':'Toko_Run_3', 'wait':3, 'nextNo':1 },
				{'pic':'Toko_Run_4', 'wait':3, 'nextNo':2 },
				{'pic':'Toko_Run_5', 'wait':3, 'nextNo':3 },
				{'pic':'Toko_Run_6', 'wait':3, 'nextNo':4 },
				{'pic':'Toko_Run_7', 'wait':3, 'nextNo':5 },
				{'pic':'Toko_Run_8', 'wait':3, 'nextNo':6 },
				{'pic':'Toko_Run_1', 'wait':3, 'nextNo':7 },
				{'pic':'Toko_Run_2', 'wait':3, 'nextNo':0 }
			],

			'crouchStart':[
				{'pic':'Toko_Crouch_2', 'wait':3, 'nextNo':1, 'body':[[-0.03774764, -0.02141318, 0.2443573, 0.4237701]] },
				{'pic':'Toko_Crouch_3', 'wait':3, 'nextNo':2 },
				{'pic':'Toko_Crouch_4', 'wait':3, 'nextNo':3 },
				{'pic':'Toko_Crouch_5', 'wait':3, 'nextState':'crouch' }
			],
			'crouch':[
				{'pic':'Toko_Crouch_5', 'wait':-1, 'nextNo':0}
			],
			'crouchEnd':[
				{'pic':'Toko_Crouch_3', 'wait':3, 'nextNo':1, 'body':[[-0.001684546, -0.01715124, 0.200902, 0.4854634]]},
				{'pic':'Toko_Crouch_2', 'wait':3, 'nextState':'stand' }
			],

			'jumpStart':[
				{'pic':'Toko_Crouch_2', 'wait':3, 'nextState':'jumpUp', 'z':1}

			],
			'jumpUp':[
				{'pic':'Toko_Jump_1', 'wait':-1, 'nextNo':0 }
			],
			'jumpMid':[
				{'pic':'Toko_Jump_2', 'wait':3, 'nextNo':1 },
				{'pic':'Toko_Jump_3', 'wait':3, 'nextNo':2 },
				{'pic':'Toko_Jump_4', 'wait':3, 'nextState':'jumpFall' }
			],
			'jumpFall':[
				{'pic':'Toko_Jump_5', 'wait':-1, 'nextNo':0}
			],
			'landing':[
				{'pic':'Toko_Crouch_2', 'wait':5, 'nextState':'stand' }
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

		//Debug.Log(stateData.ToJson());
	}


	public override bool IsAtkState() {
		return atkState.Contains(currentState);
	}


	public override void DoNoInput() {
		base.DoNoInput();
	}

	public override void DoWalk(int dir) {
		base.DoWalk(dir);
	}




	public override void DoRun(int dir) {
		base.DoRun(dir);
	}

	public override void DoCrouch(int dir) {
		base.DoCrouch(dir);
	}
	public override void DoJump(int dir) {
		base.DoJump(dir);
	}


	public override void Cmd_Z() {
		if (!onAir) {
			switch (currentState) {
				//case "atk1":
				//	DoAction("atk2");
				//	break;
				//case "atk2":
				//	DoAction("atk3");
				//	break;
				default:
					DoAction("atk1", Cmd_Z);
					break;
			}
		}
	}

	public override void Cmd_6Z() {

	}
	public override void Cmd_2Z() {

	}
	public override void Cmd_4Z() {

	}
	public override void Cmd_X() {
		//switch (currentState) {
		//	case "gun1":
		//	case "gun1_hold":
		//		DoAction("gun1_hold", true);
		//		break;
		//	default:
		//		DoAction("gun1");
		//		break;
		//}
	}
	public override void Cmd_6X() {

	}
	public override void Cmd_2X() {

	}
	public override void Cmd_4X() {

	}

	public override void ActionEnd() {
		ClearAtkRects();
		switch (currentState) {
			default:
				if (onAir) {
					ChangeState("jumpFall");
				}
				else {
					ChangeState("stand");
				}
				break;
		}
		canCancel = false;
	}
}
