using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public 	int 		maxPlayers = 4;
	public 	Transform 	spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4;

	private int 		numPlayers = 0;
	private Object		scientist;
	//private UiController ui;

	Dictionary<int, bool> joinedPlayers;

	void Start () {
		joinedPlayers = new Dictionary<int, bool> ();
		joinedPlayers[1] = false;
		joinedPlayers[2] = false;
		joinedPlayers[3] = false;
		joinedPlayers[4] = false;

		scientist = Resources.Load ("Scientist");

		//ui = GameObject.Find ("HUD").GetComponent<UiController>();
	}

	void FixedUpdate () {
		if(numPlayers < maxPlayers){
			if(Input.GetButtonDown("Start")){
				if(Input.GetButtonDown("Start1") && joinedPlayers[1] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint1.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 1;
					//ui.AddPlayer(1, s);
					joinedPlayers[1] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start2") && joinedPlayers[2] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint2.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 2;
					//ui.AddPlayer(2, s);
					joinedPlayers[2] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start3") && joinedPlayers[3] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint3.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 3;
					//ui.AddPlayer(3, s);
					joinedPlayers[3] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start4") && joinedPlayers[4] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint4.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 4;
					//ui.AddPlayer(4, s);
					joinedPlayers[4] = true;
					numPlayers++;
				}
			}
		}
	}
}
