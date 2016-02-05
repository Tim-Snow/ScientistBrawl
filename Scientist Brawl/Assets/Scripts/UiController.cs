using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiController : MonoBehaviour {
	
	private GameObject 	p1, 	p2, 	p3, 	p4;
	private Slider 		sl1, 	sl2, 	sl3, 	sl4;
	private Text 		t1, 	t2, 	t3, 	t4;

	void Start () {
		//transform.GetChild (0).GetChild(0).gameObject.SetActive (false);
		//transform.GetChild (1).GetChild(0).gameObject.SetActive (false);
		//transform.GetChild (2).GetChild(0).gameObject.SetActive (false);
		//transform.GetChild (3).GetChild(0).gameObject.SetActive (false);
	}

	void FixedUpdate () {
		if (p1 != null)		UpdateUI (p1, sl1, t1);
		if (p2 != null)		UpdateUI (p2, sl2, t2);
		if (p3 != null)		UpdateUI (p3, sl3, t3);
		if (p4 != null)		UpdateUI (p4, sl4, t4);
	}

	private void UpdateUI(GameObject player, Slider hpSlider, Text ammoText){
		hpSlider.value = player.GetComponent<MovementController> ().hp;

		if (player.GetComponent<ActionController> ().holdingWeapon) {
			ammoText.text = "Ammo: " + player.GetComponent<ActionController> ().weaponCurAmmo + "/" + player.GetComponent<ActionController> ().weaponMaxAmmo;
		} else {
			ammoText.text = "";
		}

		if (hpSlider.value <= 0) {
			ammoText.text = "Dead";
		}
	}

	public void AddPlayer(int num, GameObject p){
		switch (num) {
		case 1:
			p1 	= p;
			transform.GetChild (0).GetChild(0).gameObject.SetActive (true);
			sl1 = transform.GetChild (0).GetChild (0).GetComponent<Slider> ();
			t1 	= transform.GetChild (0).GetChild (1).GetComponent<Text> ();
			t1.text = "";
			break;
		case 2:
			p2 	= p;
			transform.GetChild (1).GetChild(0).gameObject.SetActive (true);
			transform.GetChild (2).GetChild(0).gameObject.SetActive (true);
			transform.GetChild (3).GetChild(0).gameObject.SetActive (true);
			sl2 = transform.GetChild (1).GetChild (0).GetComponent<Slider> ();
			t2 	= transform.GetChild (1).GetChild (1).GetComponent<Text> ();
			t2.text = "";
			break;
		case 3:
			p3 	= p;
			transform.GetChild (2).GetChild(0).gameObject.SetActive (true);
			sl3 = transform.GetChild (2).GetChild (0).GetComponent<Slider> ();
			t3 	= transform.GetChild (2).GetChild (1).GetComponent<Text> ();
			t3.text = "";
			break;
		case 4:
			p4 	= p;
			transform.GetChild (3).GetChild(0).gameObject.SetActive (true);
			sl4 = transform.GetChild (3).GetChild (0).GetComponent<Slider> ();
			t4 	= transform.GetChild (3).GetChild (1).GetComponent<Text> ();
			t4.text = "";
			break;
		default: break;
		}
	}
}
