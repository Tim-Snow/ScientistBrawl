using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour {

	public bool active;

	private SpriteRenderer rend;
	private Sprite anim1, anim2;
	private bool toggle;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer> ();

		Object [] sprites = Resources.LoadAll ("scientist2");
		anim1 = (Sprite)sprites [6];
		anim2 = (Sprite)sprites [7];
		toggle = false;
		active = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		active = false;

	}

	public void Use(){
		active = true;
		if (toggle) {
			rend.sprite = anim1;
		} else {
			rend.sprite = anim2;
		}

		toggle = !toggle;
	}
}
