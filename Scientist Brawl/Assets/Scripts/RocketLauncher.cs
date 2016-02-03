using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

	public int 			damage;
	public float 		rechargeTime;
	public float 		reloadTime;
	public int 			ammoAmount;
	public int 			missileSpeed;
	public bool 		shooting;
	public Transform 	shootPoint2;
	
	private bool 		reloading, delay;
	private int 		ammoRemaining;
	private Transform 	shootPoint;

	void Start () {
		rechargeTime 	= 0.5f;
		reloadTime 		= 2f;
		ammoAmount 		= 2;
		missileSpeed 	= 250;
		ammoRemaining 	= ammoAmount;
		delay 			= false;
	}

	void FixedUpdate () {
		if (shooting && !reloading && ammoRemaining > 0 && !delay) {
			Fire ();
		}
		
		if (ammoRemaining <= 0) {
			if(!reloading)
				StartCoroutine(Reload());
			
			shooting  = false;
			reloading = true;
		}
	}
	
	IEnumerator Reload(){
		yield return new WaitForSeconds (reloadTime);
		reloading 		= false;
		ammoRemaining 	= ammoAmount;
		StopCoroutine (Reload());
	}

	IEnumerator WaitDelay(){
		yield return new WaitForSeconds (rechargeTime);
		delay 		= false;
		shooting 	= false;
		StopCoroutine (WaitDelay ());
	}

	void Fire(){
		GameObject rocket = (GameObject)Instantiate (Resources.Load ("Rocket"));
		rocket.transform.position = shootPoint.position;

		Vector2 vectorToTarget = new Vector2 (shootPoint2.transform.position.x - shootPoint.transform.position.x, 
		                                      shootPoint2.transform.position.y - shootPoint.transform.position.y);

		float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		rocket.transform.rotation = q;

		rocket.GetComponent<Rigidbody2D> ().AddForce (vectorToTarget * missileSpeed);
		
		shooting 		 = false;
		delay 			 = true;
		ammoRemaining 	-= 1;

		StartCoroutine (WaitDelay ());
	}

	public void Shoot (Transform sPoint) {
		shooting 	= true;
		shootPoint 	= sPoint;
	}
}
