using UnityEngine;
using System.Collections;

public class AK47 : MonoBehaviour {

	public int 			damage;
	public float 		rechargeTime;
	public float 		reloadTime;
	public int 			ammoAmount;
	public int 			bulletSpeed;
	public Transform 	spark, shootPoint2;
	public bool 		shooting;

	private bool 		reloading, delay;
	private float 		lifetime;
	private int 		ammoRemaining, sparkCount;
	private Transform 	shootPoint;
	private Sprite 		spark1, spark2, spark3, spark4;
	private SpriteRenderer rend;
	
	void Start () {
		reloading   	= false;
		shooting 		= false;
		bulletSpeed 	= 80;
		ammoAmount  	= 8;
		sparkCount 		= 1;
		reloadTime 		= 1.5f;
		rechargeTime 	= 0.05f;
		ammoRemaining 	= ammoAmount;
		delay 			= false;

		Object[] sprites;
		sprites = Resources.LoadAll ("scientist2");
		spark1 	= (Sprite)sprites[28];
		spark2 	= (Sprite)sprites[29];
		spark3 	= (Sprite)sprites[30];
		spark4 	= (Sprite)sprites[31];
		rend 	= spark.GetComponent<SpriteRenderer> ();
	}
	
	void FixedUpdate () {
		rend.sprite = null;
	
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
		delay = false;
		StopCoroutine (WaitDelay ());
	}

	void Fire(){
		GameObject bullet = (GameObject)Instantiate (Resources.Load ("Bullet"));
		bullet.transform.position = shootPoint.position;

		Vector2 vectorToTarget = new Vector2 (shootPoint2.transform.position.x - shootPoint.transform.position.x, 
		                                      shootPoint2.transform.position.y - shootPoint.transform.position.y);
		
		float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		bullet.transform.rotation = q;
		
		bullet.GetComponent<Rigidbody2D> ().AddForce (vectorToTarget * bulletSpeed);

		shooting 		 = false;
		ammoRemaining 	-= 1;
		delay 			 = true;
		AnimateSpark ();

		StartCoroutine (WaitDelay ());
	}

	void AnimateSpark(){
		switch (sparkCount) {
		case 1:	rend.sprite = spark1;	break;
		case 2:	rend.sprite = spark2;	break;
		case 3:	rend.sprite = spark3;	break;
		case 4:	rend.sprite = spark4;	break;
		default:
			break;
		}

		sparkCount 		+= 1;
		if (sparkCount == 5)
			sparkCount = 1;
	}
	
	public void Shoot (Transform sPoint) {
		shooting 	= true;
		shootPoint 	= sPoint;
	}
}
