using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

	public 	int  				joystickNum;
	public 	bool 				canHoldWeapon, holdingWeapon;

	public Transform			aimPoint, armAnchor, weaponHoldPosition, armHoldPos;
	[HideInInspector]
	public 	Transform 			nearbyPickup;

	private bool				ducking;
	private string 				joystickString, weaponString;
	private PickupObject 		weapon;
	private MovementController	moveCont;
	private Transform			arms, armL, armR;
	private SpriteRenderer		armLSR, armRSR;

	private Sprite			armIdleRes, armHoldRes;

	void Start () {
		moveCont 	= GetComponent<MovementController> ();
		arms 		= transform.GetChild (4);
		armL 		= arms.transform.GetChild (0);
		armR 		= arms.transform.GetChild (1);
		armLSR 		= armL.GetComponent<SpriteRenderer> ();
		armRSR 		= armR.GetComponent<SpriteRenderer> ();

		Object [] sprites;
		sprites = Resources.LoadAll ("scientist2");
		armIdleRes = (Sprite)sprites[3];
		armHoldRes = (Sprite)sprites[2];

		if(joystickNum == 0)
			joystickNum = 1;

		joystickString = joystickNum.ToString();
		moveCont.joystickString = joystickString;
	}

	void CheckDuck(){
		float duckInput = Input.GetAxis ("LeftYAxis" + joystickString);
		if (duckInput <= -0.2f) {
			ducking = true;
			moveCont.anim.SetBool ("Ducking", true);
		} else {
			ducking = false;
			moveCont.anim.SetBool ("Ducking", false);
		}
	}

	void FixedUpdate () {
		arms.transform.position = armAnchor.transform.position;
		if (!moveCont.isDead) {
			CheckDuck();
			Aim();

			if (!holdingWeapon)//eventually add parameter for arm mods to disable pickups
				canHoldWeapon = true;

			if (holdingWeapon) {
				arms.transform.position = armHoldPos.transform.position;
			}

			if (Input.GetAxis ("RT" + joystickString) > 0.5 && holdingWeapon) {
				Shoot();
			}

			if (Input.GetButton ("B" + joystickString) && nearbyPickup != null && canHoldWeapon && !holdingWeapon) {
				PickUp();
			}

			if (Input.GetButton ("Y" + joystickString) && holdingWeapon) {
				Drop ();
			}
		}
	}

	void PickUp(){
		weapon = nearbyPickup.GetComponent<PickupObject> ();
		moveCont.anim.SetBool ("Holding", true);
		weapon.PickUp (this.transform);

		armLSR.sprite = armHoldRes;
		armRSR.sprite = armHoldRes;

		Vector3 theScale 	 	  = armR.transform.localScale;
		theScale.x 				 *= -1;
		armR.transform.localScale = theScale;
		arms.transform.position   = armHoldPos.transform.position;
	}
	
	public void Drop(){
		nearbyPickup = weapon.gameObject.transform;
		moveCont.anim.SetBool ("Holding", false);
		weapon.Drop ();
		weapon = null;

		armLSR.sprite = armIdleRes;
		armRSR.sprite = armIdleRes;

		Vector3 theScale 	 	  = armR.transform.localScale;
		theScale.x 			   	 *= -1;
		armR.transform.localScale = theScale;
		arms.transform.position   = armAnchor.transform.position;
	}
	
	void Shoot(){
		if(weapon.objName.Equals("rocketlauncher")){
			weapon.GetComponent<RocketLauncher> ().Shoot (weapon.shootPoint);
		}
		if(weapon.objName.Equals("ak47")){
			weapon.GetComponent<AK47> ().Shoot (weapon.shootPoint);
		}
		if(weapon.objName.Equals("chainsaw")){
			weapon.GetComponent<Chainsaw> ().Use ();
		}
	}
		
	void Aim(){
		if (!ducking) {		aimPoint.position = weaponHoldPosition.position; } 
		else 		  {		aimPoint.position = armAnchor.position;		 	 }

		Vector3 aimInput = new Vector3 ( Input.GetAxis ("RightXAxis" + joystickString), 
		                                -Input.GetAxis ("RightYAxis" + joystickString), 0);

		float posInX = Mathf.Abs (aimInput.x);
		float posInY = Mathf.Abs (aimInput.y);

		bool joyPulledBack = (aimInput.x >=  0.2f || aimInput.y >= 0.2f || aimInput.x <= -0.2f || 
		                      aimInput.y <= -0.2f || ((posInX + posInY) / 2) >= 0.2f) ? true : false;
		
		if (joyPulledBack && weapon != null) {

			Vector3 vectorToTarget;

			if (!ducking) {
				aimPoint.position = weaponHoldPosition.position + (aimInput/2);
				vectorToTarget = aimPoint.position - weaponHoldPosition.position;
			} else {
				aimPoint.position = armAnchor.position + (aimInput/2);
				vectorToTarget = aimPoint.position - armAnchor.position;
			}

			float angle;
			if (moveCont.facingRight) {
				angle = Mathf.Atan2 (vectorToTarget.y,  vectorToTarget.x) * Mathf.Rad2Deg;
			} else {
				angle = Mathf.Atan2 (vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
			}
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);

			weapon.gameObject.transform.position = aimPoint.position;
			weapon.gameObject.transform.rotation = q;
		}
	}
}

