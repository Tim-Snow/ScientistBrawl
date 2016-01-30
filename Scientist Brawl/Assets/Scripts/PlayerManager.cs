using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public 	int 		maxPlayers = 4;
	public 	Transform 	spawnPoint;

	private int 		numPlayers = 0;
	private Object		scientist;

	Dictionary<int, bool> joinedPlayers;

	void Start () {
		joinedPlayers = new Dictionary<int, bool> ();
		joinedPlayers[1] = false;
		joinedPlayers[2] = false;
		joinedPlayers[3] = false;
		joinedPlayers[4] = false;

		scientist = Resources.Load ("Scientist");
	}

	void FixedUpdate () {
		if(Input.GetButtonDown("Start")){
			if(numPlayers < maxPlayers){
				if(Input.GetButtonDown("Start1") && joinedPlayers[1] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 1;
					joinedPlayers[1] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start2") && joinedPlayers[2] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 2;
					joinedPlayers[2] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start3") && joinedPlayers[3] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 3;
					joinedPlayers[3] = true;
					numPlayers++;
				}
				if(Input.GetButtonDown("Start4") && joinedPlayers[4] == false){
					GameObject s = (GameObject)Instantiate(scientist, spawnPoint.position, Quaternion.identity);
					s.GetComponent<ActionController>().joystickNum = 4;
					joinedPlayers[4] = true;
					numPlayers++;
				}
			}
		}
	}
}
