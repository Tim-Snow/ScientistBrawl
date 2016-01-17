using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public		int				playerNumber;
	public 		float 			maxMoveSpd;
	public		float			jumpHeight;
	[HideInInspector]
	public 		bool 			facingRight;

	public		Transform		groundCheck;
	public 		LayerMask		whatIsGround;
	
	private		bool			onGround;

	private 	Rigidbody2D 	rb;

	void Start () {
		facingRight = true;
		rb 			= GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		rb.velocity = new Vector2(move * maxMoveSpd, rb.velocity.y);

		if 		(move >=  0.1f && !facingRight) { Flip (); } 
		else if (move <= -0.1f &&  facingRight) { Flip (); }

		onGround = Physics2D.OverlapCircle (groundCheck.position, 0.1f, whatIsGround);
		if(Input.GetButton("Jump") && onGround)	Jump ();
	}

	void Jump(){
		rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
	}

	void Flip(){
		facingRight 	 	 = !facingRight;
		Vector3 theScale 	 = transform.localScale;
		theScale.x 			*= -1;
		transform.localScale = theScale;
	}
}
