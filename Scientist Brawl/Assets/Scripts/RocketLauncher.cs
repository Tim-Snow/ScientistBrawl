using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

	public int damage;
	public int rechargeTime;
	public int reloadTime;
	public int ammoAmount;
	public int missileSpeed;

	private float lifetime;
	private int ammoRemaining;

	void Start () {
	
	}

	void FixedUpdate () {
	
	}

	public void Shoot (Transform shootPoint, Quaternion rotation) {
		GameObject rocket = (GameObject)Instantiate (Resources.Load ("Rocket"));
		rocket.transform.position = shootPoint.position;
		rocket.transform.rotation = rotation;
		rocket.GetComponent<Rigidbody2D>().AddForce(new Vector2(90f, 1f));
	}
}
