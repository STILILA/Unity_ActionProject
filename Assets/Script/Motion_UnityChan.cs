using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Motion_UnityChan : GameMotion
{
	new readonly List<string> atkState = new List<string>() { "atk1", "atk2", "atk3", "gun1", "gun1_hold" };




	public override void SetupAnime() {

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


			'damage1':[
				{'pic':'Unitychan_Damage_2', 'wait':2, 'nextNo':1 },
				{'pic':'Unitychan_Damage_3', 'wait':-1, 'nextNo':1 }
			],
			'damage2':[
				{'pic':'Unitychan_Damage_4', 'wait':2, 'nextNo':1 },
				{'pic':'Unitychan_Damage_5', 'wait':-1, 'nextNo':1 }
			],

			'atk1':[
				{'pic':'Unitychan_Soard_Combo_2', 'wait':2, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_3', 'wait':2, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_4', 'wait':2, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_5', 'wait':2, 'nextNo':4, 'atk':[[0.14, -0.08, 0.52, 0.35]] },
				{'pic':'Unitychan_Soard_Combo_6', 'wait':3, 'nextNo':5, 'atk':[], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_7', 'wait':7, 'nextMethod':'ActionEnd' }
			],

			'atk2':[
				{'pic':'Unitychan_Soard_Combo_8', 'wait':2, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_9', 'wait':2, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_10', 'wait':2, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_11', 'wait':2, 'nextNo':4 },
				{'pic':'Unitychan_Soard_Combo_12', 'wait':2, 'nextNo':5, 'atk':[[0.1599156, 0.08680066, 0.5492049, 0.6929471]] },
				{'pic':'Unitychan_Soard_Combo_13', 'wait':2, 'nextNo':6 },
				{'pic':'Unitychan_Soard_Combo_14', 'wait':3, 'nextNo':7, 'atk':[], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_15', 'wait':7, 'nextMethod':'ActionEnd' }
			],

			'atk3':[
				{'pic':'Unitychan_Soard_Combo_16', 'wait':2, 'nextNo':1 },
				{'pic':'Unitychan_Soard_Combo_17', 'wait':2, 'nextNo':2 },
				{'pic':'Unitychan_Soard_Combo_18', 'wait':2, 'nextNo':3 },
				{'pic':'Unitychan_Soard_Combo_19', 'wait':2, 'nextNo':4 },
				{'pic':'Unitychan_Soard_Combo_20', 'wait':2, 'nextNo':5, 'atk':[[0.2902966, 0.05575758, 0.768576, 0.6308609]] },
				{'pic':'Unitychan_Soard_Combo_21', 'wait':2, 'nextNo':6, 'atk':[[0.5075981, -0.07876238, 0.5078144, 0.361821]] },
				{'pic':'Unitychan_Soard_Combo_22', 'wait':2, 'nextNo':7, 'atk':[[0.553128, -0.07876238, 0.598874, 0.361821]] },
				{'pic':'Unitychan_Soard_Combo_23', 'wait':2, 'nextNo':8, 'atk':[[0.7580125, -0.1346399, 0.3877811, 0.2500659]] },
				{'pic':'Unitychan_Soard_Combo_24', 'wait':2, 'nextNo':9, 'atk':[[0.8200988, -0.1822393, 0.2636087, 0.1548671]], 'cancel' : true },
				{'pic':'Unitychan_Soard_Combo_25', 'wait':12, 'nextMethod':'ActionEnd', 'atk':[] }
			],

			'gun1':[
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextNo':1 },
				{'pic':'Unitychan_Hundgun1_3', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':3 },
				{'pic':'Unitychan_Hundgun2_5', 'wait':3, 'nextNo':4 },
				{'pic':'Unitychan_Hundgun2_6', 'wait':3, 'nextNo':5, 'atk':[[0.9590002, 0.02490908, 1.971455, 0.5018182]] },
				{'pic':'Unitychan_Hundgun2_7', 'wait':3, 'nextNo':6 },
				{'pic':'Unitychan_Hundgun2_8', 'wait':3, 'nextNo':7, 'atk':[] },
				{'pic':'Unitychan_Hundgun2_9', 'wait':3, 'nextNo':8, 'selfCancel':true, 'cancel' : true },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':9 },
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextMethod':'ActionEnd' }
			],
			'gun1_hold':[
				{'pic':'Unitychan_Hundgun2_6', 'wait':3, 'nextNo':1, 'atk':[[0.9590002, 0.02490908, 1.971455, 0.5018182]] },
				{'pic':'Unitychan_Hundgun2_7', 'wait':3, 'nextNo':2 },
				{'pic':'Unitychan_Hundgun2_8', 'wait':3, 'nextNo':3, 'atk':[] },
				{'pic':'Unitychan_Hundgun2_9', 'wait':3, 'nextNo':4, 'selfCancel':true, 'cancel' : true },
				{'pic':'Unitychan_Hundgun1_4', 'wait':3, 'nextNo':5 },
				{'pic':'Unitychan_Hundgun1_2', 'wait':3, 'nextMethod':'ActionEnd' }
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


	public override void DoAction_Z() {
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

	public override void DoAction_6Z() {

	}
	public override void DoAction_2Z() {

	}
	public override void DoAction_X() {
		switch (currentState) {
			case "gun1":
			case "gun1_hold":
				DoAction("gun1_hold", true);
				break;
			default:
				DoAction("gun1");
				break;
		}
	}
	public override void DoAction_6X() {

	}
	public override void DoAction_2X() {

	}


	public override SkillEffectData SkillEffect(GameMotion targetMotion) {
		var se = new SkillEffectData();

		switch (currentState) {
			case "atk1":
				se.damage = 4;
				se.hitstop = 10;
				se.hitstun = 20;
				break;
			case "atk2":
				se.damage = 4;
				se.hitstop = 10;
				se.hitstun = 20;
				break;
			case "atk3":
				se.damage = 30;
				se.hitstop = 7;
				se.hitstun = 20;
				break;
			case "gun1":
				se.damage = 8;
				se.hitstop = 2;
				se.hitstun = 20;
				break;
		}

		return se;
	}


	public override void ActionEnd() {
		ClearRects(null,10);
		switch (currentState) {
			case "atk1":
			case "atk2":
			case "atk3":
			case "gun1":
				ChangeState("stand");
				break;
			default:
				if (onAir) {
					ChangeState("jumpFall");
				} else {
					ChangeState("stand");
				}
				break;
		}
		canCancel = false;
	}

}
