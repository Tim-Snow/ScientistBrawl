using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharSelectController : MonoBehaviour {

	public int 			maxPlayers = 4;
	public Text 		t1, t2, t3, t4,	mainText;
	public Transform 	spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4;
	public Canvas		customizeUI;
	
	private int 		numPlayers = 0;
	private string		enteredMessage, emptyMessage, mainMessage, backMessage;
	private Object		scientist;
	private Transform	cPan1, cPan2, cPan3, cPan4;
	private Dictionary<int, GameObject> thePlayers;
	private Dictionary<int, bool> joinedPlayers;
	private Dictionary<int, bool> playersCustomizing;
	
	void Start () {
		thePlayers = new Dictionary<int, GameObject>();
		thePlayers[1] = null;
		thePlayers[2] = null;
		thePlayers[3] = null;
		thePlayers[4] = null;

		joinedPlayers = new Dictionary<int, bool> ();
		joinedPlayers[1] = false;
		joinedPlayers[2] = false;
		joinedPlayers[3] = false;
		joinedPlayers[4] = false;

		playersCustomizing = new Dictionary<int, bool> ();
		playersCustomizing[1] = false;
		playersCustomizing[2] = false;
		playersCustomizing[3] = false;
		playersCustomizing[4] = false;

		enteredMessage 	= "Press X to customize";
		mainMessage 	= "Press Start to play";
		backMessage 	= "Press B to return";
		emptyMessage 	= "";
		
		scientist 		= Resources.Load ("Scientist");

		Transform o = (Transform)customizeUI.transform.GetChild (0);
		cPan1 = o.GetChild (0);
		cPan2 = o.GetChild (1);
		cPan3 = o.GetChild (2);
		cPan4 = o.GetChild (3);
	}
	
	void FixedUpdate () {
		if (numPlayers >= 1) {
			mainText.text = mainMessage;

			//if already entered char presses start, go to level
		} else {
			mainText.text = emptyMessage;
		}


		if(numPlayers < maxPlayers){
			CheckForMorePlayers();
		}

		CheckCustomizeUI ();
		//CheckPlayerLeave ();

	}

	void CheckCustomizeUI(){
		for (int i = 1; i <= maxPlayers; i++) {
			if(joinedPlayers[i] == true){
				if(Input.GetButtonDown("X" + i)){
					if(playersCustomizing[i] == false){
						ChangePanelVis(i, playersCustomizing[i]);
						playersCustomizing[i] = true;
						thePlayers[i].SetActive(false);
					}
				}
				if(Input.GetButtonDown("B" + i)){
					if(playersCustomizing[i] == true){
						ChangePanelVis(i, playersCustomizing[i]);
						playersCustomizing[i] = false;
						thePlayers[i].SetActive(true);
					}
				}
			}
		}
	}

	void ChangePanelVis(int pNum, bool enabled){
		if (enabled) {
			switch(pNum){
			case 1:
				cPan1.GetComponent<CustomizationController>().active = false;
				cPan1.gameObject.SetActive(false);
				t1.text = enteredMessage;
				break;
			case 2:
				cPan2.GetComponent<CustomizationController>().active = false;
				cPan2.gameObject.SetActive(false);
				t2.text = enteredMessage;
				break;
			case 3:
				cPan3.GetComponent<CustomizationController>().active = false;
				cPan3.gameObject.SetActive(false);
				t3.text = enteredMessage;
				break;
			case 4:
				cPan4.GetComponent<CustomizationController>().active = false;
				cPan4.gameObject.SetActive(false);
				t4.text = enteredMessage;
				break;
			default: break;
			}
		} else {
			switch(pNum){
			case 1:
				cPan1.gameObject.SetActive(true);
				cPan1.GetComponent<CustomizationController>().joystickString = pNum.ToString();
				cPan1.GetComponent<CustomizationController>().active = true;
				t1.text = backMessage;
				break;
			case 2:
				cPan2.gameObject.SetActive(true);
				cPan2.GetComponent<CustomizationController>().joystickString = pNum.ToString();
				cPan2.GetComponent<CustomizationController>().active = true;
				t2.text = backMessage;
				break;
			case 3:
				cPan3.gameObject.SetActive(true);
				cPan3.GetComponent<CustomizationController>().joystickString = pNum.ToString();
				cPan3.GetComponent<CustomizationController>().active = true;
				t3.text = backMessage;
				break;
			case 4:
				cPan4.gameObject.SetActive(true);
				cPan4.GetComponent<CustomizationController>().joystickString = pNum.ToString();
				cPan4.GetComponent<CustomizationController>().active = true;
				t4.text = backMessage;
				break;
			default: break;
			}
		}
	}

	void CheckPlayerLeave(){
		//if(Input.GetButtonDown("Back")){
	}

	void CheckForMorePlayers(){
		if(Input.GetButtonDown("Start")){
			if(Input.GetButtonDown("Start1") && joinedPlayers[1] == false){
				GameObject s = (GameObject)Instantiate(scientist, spawnPoint1.position, Quaternion.identity);
				s.GetComponent<ActionController>().joystickNum = 1;
				s.GetComponent<MovementController>().SetImmune(true);
				joinedPlayers[1] = true;
				thePlayers[1] = s;
				numPlayers++;
				t1.text = enteredMessage;
			}
			if(Input.GetButtonDown("Start2") && joinedPlayers[2] == false){
				GameObject s = (GameObject)Instantiate(scientist, spawnPoint2.position, Quaternion.identity);
				s.GetComponent<ActionController>().joystickNum = 2;
				s.GetComponent<MovementController>().SetImmune(true);
				joinedPlayers[2] = true;
				thePlayers[2] = s;
				numPlayers++;
				t2.text = enteredMessage;
			}
			if(Input.GetButtonDown("Start3") && joinedPlayers[3] == false){
				GameObject s = (GameObject)Instantiate(scientist, spawnPoint3.position, Quaternion.identity);
				s.GetComponent<ActionController>().joystickNum = 3;
				s.GetComponent<MovementController>().SetImmune(true);
				joinedPlayers[3] = true;
				thePlayers[3] = s;
				numPlayers++;
				t3.text = enteredMessage;
			}
			if(Input.GetButtonDown("Start4") && joinedPlayers[4] == false){
				GameObject s = (GameObject)Instantiate(scientist, spawnPoint4.position, Quaternion.identity);
				s.GetComponent<ActionController>().joystickNum = 4;
				s.GetComponent<MovementController>().SetImmune(true);
				joinedPlayers[4] = true;
				thePlayers[4] = s;
				numPlayers++;
				t4.text = enteredMessage;
			}
		}
	}
}
