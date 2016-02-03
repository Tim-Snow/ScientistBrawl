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
		if (rb != null) {
			rb.gravityScale = 1f;
		}
		lifetime = 2f;
		StopCoroutine (CheckExpire ());
		StartCoroutine (CheckExpire ());

		if (canDamage && coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			coll.gameObject.GetComponent<MovementController>().hp -= 10;
		}

		canDamage = false;
	}
}
