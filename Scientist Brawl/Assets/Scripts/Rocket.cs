using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

	public 	bool		canDamage;
	public  ParticleSystem trail, explode;

	private float 		lifetime;

	void Start () {
		canDamage 	= true;
		lifetime 	= 30f;
		explode.enableEmission = false;
		explode.Pause ();

		StartCoroutine (CheckExpire ());
	}
	
	void FixedUpdate () {
		
	}
	
	IEnumerator CheckExpire(){
		yield return  new WaitForSeconds (lifetime);
		Destroy (this.gameObject);
	}

	IEnumerator CheckParticleEnd(){
		yield return new WaitForSeconds (1f);

		if (!explode.IsAlive ()) {
			Destroy (explode);
			Destroy (trail);
			Destroy(this.gameObject);
		} else {
			StartCoroutine (CheckParticleEnd ());
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		//Stop colliding with player that fired
		StopCoroutine (CheckExpire ());
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<Rigidbody2D> ().Sleep ();

		if (canDamage && coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			coll.gameObject.GetComponent<MovementController>().hp -= 50;
		}

		//create coroutine to destroy when particles have ended
		if (trail != null) {
			trail.enableEmission = false;
			trail.Stop();
		}
		if (explode != null) {
			explode.Play();
			explode.enableEmission = true;
		}

		StartCoroutine (CheckParticleEnd ());
		canDamage = false;
	}
}