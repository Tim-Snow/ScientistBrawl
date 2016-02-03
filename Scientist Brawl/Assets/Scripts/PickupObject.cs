using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {

	public 	string				objName;
	public  Transform  			spawner;
	public 	Transform			groundCheckPos;
	public 	Transform 			shootPoint;
	[HideInInspector]
	public 	bool				inSpawner;
	
	private bool				isHeld;
	private bool				facingRight;
	private bool				onGround;

	private Transform 			playerHolding;
	private ActionController 	acCont;
	private MovementController 	moveCont;

	void Start () {
		facingRight = true;
		onGround 	= false;
	}

	void FixedUpdate () {
		CheckGround ();

		if (isHeld)		transform.position = acCont.aimPoint.transform.position;

		if (!onGround && !inSpawner) transform.position = 
						 new Vector3(transform.position.x, 
					                 transform.position.y - 0.1f, 
					                 transform.position.z);
	}
	
	void CheckGround(){
		onGround = Physics2D.OverlapCircle(groundCheckPos.position, 0.02f, 1 << LayerMask.NameToLayer("Ground"));
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			coll.GetComponent<ActionController>().nearbyPickup = this.transform;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			if(coll.GetComponent<ActionController>().nearbyPickup == this.transform)
				coll.GetComponent<ActionController>().nearbyPickup = null;
		}
	}

	public void PickUp(Transform player){
		if (!isHeld) {
			playerHolding 	= player;
			acCont   		= playerHolding.GetComponent<ActionController> ();
			moveCont 		= playerHolding.GetComponent<MovementController> ();

			if (acCont.canHoldWeapon) {
				isHeld 				 = true;
				acCont.canHoldWeapon = false;
				acCont.holdingWeapon = true;
				transform.parent 	 = playerHolding.transform;
				transform.position 	 = acCont.weaponHoldPosition.position;

				if (inSpawner) {
					spawner.GetComponent<WeaponSpawner> ().WeaponTaken ();
					inSpawner = false;
				}

				if (facingRight && !moveCont.facingRight ||	!facingRight && moveCont.facingRight) {
					Flip ();
				}

				if (GetComponent<PolygonCollider2D> () != null) {
					GetComponent<PolygonCollider2D> ().enabled = false;
				} else {
					GetComponent<BoxCollider2D> ().enabled = false;
				}
			}
		}
	}

	void Flip(){
		facingRight 	 	 = !facingRight;
		Vector3 theScale 	 = transform.localScale;
		theScale.x 			*= -1;
		transform.localScale = theScale;
	}

	public void Drop(){
		if (isHeld) {
			if(GetComponent<PolygonCollider2D> () != null){
				GetComponent<PolygonCollider2D> ().enabled = true;
			} else {
				GetComponent<BoxCollider2D> ().enabled = true;
			}

			transform.rotation 	 = Quaternion.identity;
			facingRight 	 	 = moveCont.facingRight;
			acCont.holdingWeapon = false;
			playerHolding 		 = null;
			acCont 				 = null;
			moveCont 			 = null;
			isHeld 			 	 = false;
			transform.parent 	 = null;
		}
	}
}
