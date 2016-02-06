using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomizationController : MonoBehaviour {

	public string joystickString;
	public bool active;
	public Transform sel1, sel2, sel3;
	public GameObject selectionIcon;
	public Text	headText, armText, legText;

	private int size, numHeadMods, numArmMods, numLegMods, curHeadMod, curArmMod, curLegMod;
	private int currentSelection;
	private int move; 
	private bool canMove;

	private string[] headMods = new string[]{"Head", "Tesla", "Mallet", "Flamethrower", "Propellor", "Lazer", "Helmet"};
	private string[] armMods = new string[]{"Arms", "Rocket Launcher", "Machine Gun", "Chainsaw"};
	private string[] legMods = new string[]{"Legs", "Rocket Boosters", "Mech-Spider", "Tracks"};
	
	void Start () {
		numHeadMods = 7;
		numArmMods = 4;
		numLegMods = 4;
		curHeadMod = 1;
		curArmMod = 1;
		curLegMod = 1;

		move = 0;
		size = 3;
		currentSelection = 1;
		canMove = true;
	}

	void CheckInput(){
		float input = Input.GetAxis ("LeftYAxis" + joystickString);
		move = 0;

		if (input <= -0.7f)
			move = 1;
		else if (input >= 0.7f)
			move = -1;
		else 
			move = 0;

		if (canMove) {
			if (move == 1 && currentSelection != 3){
				currentSelection++;
				StartCoroutine(MenuTimeout());
			}
			if (move == -1 && currentSelection != 1){
				currentSelection--;
				StartCoroutine(MenuTimeout());
			}
		}

		float horizInput = Input.GetAxis ("LeftXAxis" + joystickString);
		if (canMove) {
			if (horizInput <= -0.7f)
				MoveLeft ();
			if (horizInput >= 0.7f)
				MoveRight ();
		}
	}

	void MoveLeft(){
		switch (currentSelection) {
		case 1:
			if(curHeadMod != 1){
				curHeadMod--;
				headText.text = headMods[curHeadMod-1];
			}
			break;
		case 2:
			if(curArmMod != 1){
				curArmMod--;
				armText.text = armMods[curArmMod-1];
			}
			break;
		case 3:
			if(curLegMod != 1){
				curLegMod--;
				legText.text = legMods[curLegMod-1];
			}
			break;
		default:
			break;
		}

		StartCoroutine (MenuTimeout ());
	}

	void MoveRight(){
		switch (currentSelection) {
		case 1:
			if(curHeadMod != numHeadMods){
				curHeadMod++;
				headText.text = headMods[curHeadMod-1];
			}
			break;
		case 2:
			if(curArmMod != numArmMods){
				curArmMod++;
				armText.text = armMods[curArmMod-1];
			}
			break;
		case 3:
			if(curLegMod != numLegMods){
				curLegMod++;
				legText.text = legMods[curLegMod-1];
			}
			break;
		default:
			break;
		}
		StartCoroutine (MenuTimeout ());
	}

	IEnumerator MenuTimeout(){
		canMove = false;
		yield return new WaitForSeconds (0.1f);
		canMove = true;
	}

	void FixedUpdate () {
		CheckInput ();

		if (active) {
			switch(currentSelection){
			case 1:
				selectionIcon.transform.position = sel1.position;
				break;
			case 2:
				selectionIcon.transform.position = sel2.position;
				break;
			case 3:
				selectionIcon.transform.position = sel3.position;
				break;
			default: break;
			}
		}
	}
}
