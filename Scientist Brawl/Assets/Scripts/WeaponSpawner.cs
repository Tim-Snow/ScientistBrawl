using UnityEngine;
using System.Collections;

public class WeaponSpawner : MonoBehaviour {

	public 	Transform 	spawnPoint;
	public 	int 		respawnTime 	= 5;
	public 	int 		typeOfSpawner 	= 0;	// 0 = random //1 = rocket launcher // 2 = ak // 3 = chainsaw

	[HideInInspector]
	public  bool 		holdingWeapon 	= true;
	
	private int 		nextWeapon 		= 1;
	private GameObject 	weapon;

	void Start () {
		SpawnWeapon ();
	}

	void Update () {
		if (!holdingWeapon) {
			weapon = null;
			StartCoroutine(SpawnWeaponCountdown());
			holdingWeapon = true;
		}
	}

	void SpawnWeapon(){
		if (typeOfSpawner == 0) {
			nextWeapon = (int)Mathf.Ceil (Random.value * 3);
		} else {
			nextWeapon = typeOfSpawner;
		}

		switch (nextWeapon){
		case 0:		weapon = (GameObject)Resources.Load("RocketLauncher");
			print ("Should not occur");
			break;
		case 1: 	weapon = (GameObject)Resources.Load("RocketLauncher");
			break;
		case 2: 	weapon = (GameObject)Resources.Load("AK47");
			break;
		case 3: 	weapon = (GameObject)Resources.Load("Chainsaw");
			break;
		}
		
		Instantiate (weapon);
		weapon.transform.position = spawnPoint.position;
		weapon.GetComponent<PickupObject> ().spawner = transform.gameObject.GetComponent<WeaponSpawner>();
		weapon.GetComponent<PickupObject> ().inSpawner = true;
		holdingWeapon = true;
	}

	IEnumerator SpawnWeaponCountdown(){
		yield return new WaitForSeconds (respawnTime);
		SpawnWeapon ();
	}
}
