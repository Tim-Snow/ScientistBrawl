using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

	public 	bool 				canHoldWeapon, holdingWeapon;
	public 	int  				joystickNum, weaponCurAmmo, weaponMaxAmmo;

	public Transform			aimPoint, armAnchor, weaponHoldPosition, armHoldPos;
	[HideInInspector]
	public 	Transform 			nearbyPickup;

	private bool				ducking;
	private string 				joystickString, weaponString;
	private PickupObject 		weapon;
	private MovementController	moveCont;
	private Transform			arms, armL, armR;
	private SpriteRenderer		armLSR, armRSR;
	private Sprite				armIdleRes, armHoldRes;

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

			if(weapon != null && holdingWeapon){
				Aim();
				arms.transform.position = armHoldPos.transform.position;

				if(weapon.objName.Equals("rocketlauncher")){
					weaponCurAmmo = weapon.GetComponent<RocketLauncher> ().ammoRemaining;
				}
				if(weapon.objName.Equals("ak47")){
					weaponCurAmmo = weapon.GetComponent<AK47> ().ammoRemaining;
				}
			} else {
				canHoldWeapon = true;
			}

			if (Input.GetAxis ("RT" + joystickString) > 0.5 && holdingWeapon) {
				Use();
			}

			if (Input.GetButton ("B" + joystickString) && nearbyPickup != null && canHoldWeapon && !holdingWeapon) {
				PickUp();
			}

			if (Input.GetButton ("Y" + joystickString) && holdingWeapon) {
				Drop ();
			}

			if (Input.GetButton ("X" + joystickString) && holdingWeapon) {
				Reload ();
			}
		}
	}

	void Reload(){
		if(weapon.objName.Equals("rocketlauncher")){
			weapon.GetComponent<RocketLauncher> ().ForceReload();
		} else if(weapon.objName.Equals("ak47")){
			weapon.GetComponent<AK47> ().ForceReload();
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

		if(weapon.objName.Equals("rocketlauncher")){
			weaponCurAmmo = weapon.GetComponent<RocketLauncher> ().ammoAmount;
			weaponMaxAmmo = weaponCurAmmo;
		} else if(weapon.objName.Equals("ak47")){
			weaponCurAmmo = weapon.GetComponent<AK47> ().ammoAmount;
			weaponMaxAmmo = weaponCurAmmo;
		} else {
			weaponCurAmmo = 0;
			weaponMaxAmmo = weaponCurAmmo;
		}
	}
	
	public void Drop(){
		if(weapon.objName.Equals("rocketlauncher")){
			weapon.GetComponent<RocketLauncher> ().StopReload();
		}
		if(weapon.objName.Equals("ak47")){
			weapon.GetComponent<AK47> ().StopReload();
		}

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
	
	void Use(){
		if(weapon.objName.Equals("rocketlauncher")){
			if(!weapon.GetComponent<RocketLauncher> ().reloading)
				weapon.GetComponent<RocketLauncher> ().Shoot (weapon.shootPoint);
		}
		if(weapon.objName.Equals("ak47")){
			if(!weapon.GetComponent<AK47> ().reloading)
				weapon.GetComponent<AK47> ().Shoot (weapon.shootPoint);
		}
		if(weapon.objName.Equals("chainsaw")){
			weapon.GetComponent<Chainsaw> ().Use ();
		}
	}
		
	void Aim(){
		if (!ducking) {	aimPoint.position = weaponHoldPosition.position; } 
		else 		  {	aimPoint.position = armAnchor.position;		 	 }

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

