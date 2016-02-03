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

	private		bool			onGround;
	private		Slider 			slider;
	private 	Rigidbody2D 	rb;

	void Start () {
		hp 			= 100;
		isDead 		= false;
		facingRight = true;
		rb 			= GetComponent<Rigidbody2D> ();
		anim 		= gameObject.transform.GetChild(3).GetComponent<Animator> ();
		anim.SetBool("Moving", false);
		GameObject p = GameObject.Find ("PanelP" + joystickString);
		slider = p.transform.GetChild (0).GetComponent<Slider> ();
		slider.value = hp;
	}

	void CheckDead(){
		if (hp > 0) {
			isDead = false;
		} else {
			if(isDead == false){
				isDead = true;
				anim.SetBool("Dead", true);
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

		slider.value = hp;

		if (!isDead) {
			float move = Input.GetAxis ("LeftXAxis" + joystickString);
			rb.velocity = new Vector2 (move * maxMoveSpd, rb.velocity.y);

			if (move >= 0.1f && !facingRight) {
				Flip ();
			} else if (move <= -0.1f && facingRight) {
				Flip ();
			}
			float aim = Input.GetAxis ("RightXAxis" + joystickString);
			if (facingRight && aim < 0f)
				Flip ();
			if (!facingRight && aim > 0f)
				Flip ();
						
			if (move >= 0.05f || move <= -0.05f) {
				anim.SetBool ("Moving", true);
				anim.SetFloat ("Speed", Mathf.Abs (move));
			} else { 
				anim.SetBool ("Moving", false);
			}

			onGround = Physics2D.OverlapCircle (groundCheck.position, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
			anim.SetBool ("On Ground", onGround);

			if (Input.GetButton ("A" + joystickString) && onGround)
				Jump ();
		}
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
