using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {

	public 	Transform		groundCheckPos;
	public 	LayerMask		whatIsGround;
	public 	bool			inSpawner;

	private Transform  		spawner;
	private Transform 		playerHolding;
	private Transform		holdPosition;

	private bool			isHeld;
	private bool 			playerNear;
	private bool			facingRight;
	private bool			onGround;

	void Start () {
		playerNear 	= false;
		facingRight = true;
	}

	void FixedUpdate () {
		CheckGround ();

		if (Input.GetButton ("B1") && playerNear) {
			if(playerHolding != null){
				if(playerHolding.GetComponent<ActionController>().canHoldWeapon){
					playerHolding.GetComponent<ActionController>().canHoldWeapon = false;
					PickUp ();
				}
			}
		}
		if (Input.GetButton("Y1") 	&& isHeld) 		Drop ();

		if (isHeld)		transform.position = holdPosition.position;

		if (!onGround && !inSpawner) transform.position = 
						 new Vector3(transform.position.x, 
					                 transform.position.y - 0.1f, 
					                 transform.position.z);
	}

	public void SetSpawner(Transform sp){
		spawner = sp;
	}

	void CheckGround(){
		onGround = Physics2D.OverlapCircle(groundCheckPos.position, 0.02f, whatIsGround);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			playerHolding = coll.gameObject.transform;
			holdPosition = playerHolding.transform.GetChild(1).transform;
			playerNear = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			playerHolding = null;
			playerNear = false;
		}
	}

	void PickUp(){
		if (!isHeld) {
			if(inSpawner){
				spawner.GetComponent<WeaponSpawner>().WeaponTaken();
				inSpawner = false;
			}

			isHeld 			 = true;
			transform.parent = playerHolding.transform;

			if ( facingRight && !playerHolding.GetComponent<MovementController> ().facingRight ||
			    !facingRight &&  playerHolding.GetComponent<MovementController> ().facingRight) {
					Flip ();
			}

			if(GetComponent<PolygonCollider2D> () != null){
				GetComponent<PolygonCollider2D> ().enabled = false;
			} else {
				GetComponent<BoxCollider2D> ().enabled = false;
			}
		}
	}

	void Flip(){
		facingRight 	 	 = !facingRight;
		Vector3 theScale 	 = transform.localScale;
		theScale.x 			*= -1;
		transform.localScale = theScale;
	}

	void Drop(){
		if (isHeld) {
			if(GetComponent<PolygonCollider2D> () != null){
				GetComponent<PolygonCollider2D> ().enabled = true;
			} else {
				GetComponent<BoxCollider2D> ().enabled = true;
			}

			playerHolding.GetComponent<ActionController>().canHoldWeapon = true;
			facingRight 	 = playerHolding.GetComponent<MovementController> ().facingRight;
			isHeld 			 = false;
			transform.parent = null;
		}
	}
}
