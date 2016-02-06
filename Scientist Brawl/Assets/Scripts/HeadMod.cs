using UnityEngine;
using System.Collections;

public class HeadMod : MonoBehaviour {

	public int 			modId;
	public Transform 	modLocationF, modLocationR, mod, 
						baseObject, duckingPosition, headBand;
	public bool 		isFacingFront;

	public Sprite 	headBandSpr, teslaF, teslaR, MalletF, MalletR, FlamethrowerF, FlamethrowerR,
					PropellorF, PropellorR, LazerF, LazerR, HelmetF, HelmetR;

	private Sprite 				usingF, usingR;
	private SpriteRenderer 		sr;
	private MovementController 	moveCont;
	private ActionController 	acCont;

	void Start () {
		isFacingFront 	= true;
		moveCont 		= baseObject.GetComponent<MovementController> ();
		acCont 			= baseObject.GetComponent<ActionController> ();
		sr 				= mod.GetComponent<SpriteRenderer>();

		ChangeMod (0);
	}

	void FixedUpdate () {
		if (modId != 0) {
			if(!acCont.IsDucking()) {
				headBand.transform.position = transform.position;
				if (moveCont.IsMoving ()) {
					sr.sprite = usingR;
					mod.transform.position = modLocationR.position;;
				} else {
					sr.sprite = usingF;
					mod.transform.position = modLocationF.position;
				}
			} else {
				sr.sprite = usingR;
				headBand.transform.position = duckingPosition.position;
				mod.transform.position = duckingPosition.position;
			}
		}
	}

	public void ChangeMod(int id){
		modId = id;

		if (modId != 0) {
			baseObject.GetComponent<ActionController> ().SetHeadMod (true);
			headBand.GetComponent<SpriteRenderer>().sprite = headBandSpr;
			headBand.GetComponent<SpriteRenderer>().enabled = true;
			sr.enabled = true;
		}

		switch (modId) {
		case 0:
			baseObject.GetComponent<ActionController> ().SetHeadMod (false);
			headBand.GetComponent<SpriteRenderer>().enabled = false;
			sr.enabled = false;
			break;
		case 1:
			usingF = teslaF;
			usingR = teslaR;
			break;
		case 2:
			usingF = MalletF;
			usingR = MalletR;
			break;
		case 3:
			usingF = FlamethrowerF;
			usingR = FlamethrowerR;
			break;
		case 4:
			usingF = PropellorF;
			usingR = PropellorR;
			break;
		case 5:
			usingF = LazerF;
			usingR = LazerR;
			break;
		case 6:
			usingF = HelmetF;
			usingR = HelmetR;
			break;
		default: break;
		}
		
		sr.sprite = usingF;
	}

	public void Use(){
		if (modId == 1) {
			print ("zap");
		}
	}
}
