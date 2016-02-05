using UnityEngine;
using System.Collections;

public class CustomizationController : MonoBehaviour {

	public string joystickString;
	public bool active;
	public Transform sel1, sel2, sel3;
	public GameObject selectionIcon;

	private int size;
	private int currentSelection;
	private int move; 
	private bool canMove;
	
	void Start () {
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
		//if (horizInput <= -0.7f)
		//if (horizInput >= 0.7f)
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
