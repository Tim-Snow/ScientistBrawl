using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public 	bool		canDamage;

	private float 		lifetime;
	private Rigidbody2D rb;

	void Start () {
		canDamage 	= true;
		lifetime 	= 30f;
		rb 			= GetComponent<Rigidbody2D> ();

		StartCoroutine (CheckExpire ());
	}

	void FixedUpdate () {
	
	}

	IEnumerator CheckExpire(){
		yield return  new WaitForSeconds (lifetime);
		Object.Destroy (this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.layer != 1 << LayerMask.NameToLayer ("UI")){
			if (rb != null) {
				rb.gravityScale = 1f;
			}
			lifetime = 2f;
			StopCoroutine (CheckExpire ());
			StartCoroutine (CheckExpire ());

			if (canDamage && coll.gameObject.layer == 1 << LayerMask.NameToLayer ("Player")) {
				coll.gameObject.GetComponent<MovementController>().Hit(10);
			}

			canDamage = false;
		}
	}
}
