using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

	public 	bool 				canHoldWeapon;
	public 	bool 				holdingWeapon;
	public 	int  				joystickNum;

	public Transform			aimPoint;
	public Transform			weaponHoldPosition;
	[HideInInspector]
	public 	Transform 			nearbyPickup;

	private bool				ducking;
	private string 				joystickString;
	private string				weaponString;	//find way to get name of game object
	private PickupObject 		weapon;
	private MovementController	moveCont;

	void Start () {
		moveCont = GetComponent<MovementController> ();

		if(joystickNum == 0)
			joystickNum = 1;

		joystickString = joystickNum.ToString();
		moveCont.joystickString = joystickString;
	}

	void FixedUpdate () {
		float duckInput  = Input.GetAxis ("LeftYAxis" + joystickString);
		if (duckInput <= -0.2f) {
			ducking = true;
			moveCont.anim.SetBool("Ducking", true);
		} else {
			ducking = false;
			moveCont.anim.SetBool("Ducking", false);
		}

		if (!holdingWeapon)//eventually add parameter for arm mods to disable pickups
			canHoldWeapon = true;

		if (Input.GetAxis ("RT" + joystickString) > 0.5 && holdingWeapon) {
			weapon.GetComponent<RocketLauncher>().Shoot(weapon.shootPoint, weapon.gameObject.transform.rotation);
		}

		if (Input.GetButton ("B" + joystickString) && nearbyPickup != null && canHoldWeapon && !holdingWeapon) {
			weapon = nearbyPickup.GetComponent<PickupObject>();
			weapon.PickUp (this.transform);
		}

		if (Input.GetButton ("Y" + joystickString) && holdingWeapon) {
			nearbyPickup = weapon.gameObject.transform;
			weapon.Drop ();
			weapon = null;
		}

		aimPoint.position = weaponHoldPosition.position;

		Vector3 aimInput  = new Vector3 (Input.GetAxis ("RightXAxis" + joystickString), -Input.GetAxis ("RightYAxis" + joystickString), 0);
		float posInX = Mathf.Abs(aimInput.x);
		float posInY = Mathf.Abs(aimInput.y);
		bool joyPulledBack = (aimInput.x >= 0.2f  || aimInput.y >=  0.2f || aimInput.x <= -0.2f || aimInput.y <= -0.2f || 
		                      ((posInX + posInY) / 2) >= 0.2f) ? true : false;

		if (joyPulledBack && weapon != null) {

			aimPoint.position = weaponHoldPosition.position + aimInput;
			Vector3 vectorToTarget = aimPoint.position - weaponHoldPosition.position;

			float angle;
			if (moveCont.facingRight) {
				angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			} else {
				angle = Mathf.Atan2 (vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
			}
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);

			weapon.gameObject.transform.position = aimPoint.position;
			weapon.gameObject.transform.rotation = q;
		}
	}
}

