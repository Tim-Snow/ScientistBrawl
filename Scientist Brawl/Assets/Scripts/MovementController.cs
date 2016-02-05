using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovementController : MonoBehaviour {

	public 		int				hp;
	public		string			joystickString;
	public 		float 			maxMoveSpd, jumpHeight;
	public 		bool			isDead;
	[HideInInspector]
	public 		bool 			facingRight;
	public		Transform		groundCheck;
	[HideInInspector]
	public		Animator		anim;

	private		bool			onGround, immune;
	private		Slider 			slider;
	private 	Rigidbody2D 	rb;

	void Start () {
		immune 		= false;
		hp 			= 100;
		isDead 		= false;
		facingRight = true;
		rb 			= GetComponent<Rigidbody2D> ();
		anim 		= gameObject.transform.GetChild(3).GetComponent<Animator> ();
		anim.SetBool("Moving", false);
	}

	public void Hit(int damage){
		if (!immune)
			hp -= damage;
	}

	public void SetImmune(bool b){
		immune = b;
	}

	void CheckDead(){
		if (hp > 0) {
			isDead = false;
		} else {
			if(isDead == false){
				isDead = true;
				anim.SetBool("Dead", true);
				anim.SetBool ("On Ground", true);
				if(GetComponent<ActionController>().holdingWeapon){
					GetComponent<ActionController>().Drop();
				}
				GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<CircleCollider2D>().radius = 0.05f;
			}
		}
	}

	void FixedUpdate () {
		CheckDead ();

		if (!isDead) {
			float move = Input.GetAxis ("LeftXAxis" + joystickString);
			Move (move);					
			Aim ();
			CheckGround();

			if (Input.GetButton ("A" + joystickString) && onGround)
				Jump ();
		}
	}

	void CheckGround(){
		onGround = Physics2D.OverlapCircle (groundCheck.position, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
		anim.SetBool ("On Ground", onGround);
	}

	void Move(float m){
		rb.velocity = new Vector2 (m * maxMoveSpd, rb.velocity.y);
		
		if (m >= 0.1f && !facingRight) {
			Flip ();
		} else if (m <= -0.1f && facingRight) {
			Flip ();
		}

		if (m >= 0.05f || m <= -0.05f) {
			anim.SetBool ("Moving", true);
			anim.SetFloat ("Speed", Mathf.Abs (m));
		} else { 
			anim.SetBool ("Moving", false);
		}
	}

	void Aim(){
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
