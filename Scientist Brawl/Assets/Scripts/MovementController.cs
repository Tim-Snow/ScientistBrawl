using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public		string			joystickString;
	public 		float 			maxMoveSpd;
	public		float			jumpHeight;
	[HideInInspector]
	public 		bool 			facingRight;
	[HideInInspector]
	public		Animator		anim;

	public		Transform		groundCheck;
	
	private		bool			onGround;

	private 	Rigidbody2D 	rb;

	void Start () {
		facingRight = true;
		rb 			= GetComponent<Rigidbody2D> ();
		anim 		= gameObject.transform.GetChild(3).GetComponent<Animator> ();

		anim.SetBool("Moving", false);
	}

	void FixedUpdate () {
		float move = Input.GetAxis ("LeftXAxis" + joystickString);
		rb.velocity = new Vector2(move * maxMoveSpd, rb.velocity.y);

		if 		(move >=  0.1f && !facingRight) { Flip (); } 
		else if (move <= -0.1f &&  facingRight) { Flip (); }

		if (move >= 0.05f || move <= -0.05f) {
			anim.SetBool("Moving", true);
			anim.SetFloat("Speed", Mathf.Abs(move));
		} else { 
			anim.SetBool("Moving", false);
		}

		onGround = Physics2D.OverlapCircle (groundCheck.position, 0.1f, 1 << LayerMask.NameToLayer("Ground"));
		anim.SetBool ("On Ground", onGround);

		if(Input.GetButton("A" + joystickString) && onGround)	Jump ();

		float aim = Input.GetAxis ("RightXAxis" + joystickString);

		if (facingRight && aim < 0f)
			Flip ();
		if (!facingRight && aim > 0f)
			Flip ();

	}

	void Jump(){
		rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
		anim.SetBool ("On Ground", false);
	}

	void Flip(){
		facingRight 	 	 = !facingRight;
		Vector3 theScale 	 = transform.localScale;
		theScale.x 			*= -1;
		transform.localScale = theScale;
	}
}
