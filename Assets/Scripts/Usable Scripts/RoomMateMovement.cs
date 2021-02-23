using UnityEngine;
using System.Collections;

public class RoomMateMovement : MonoBehaviour
{
	public WayPoint currentWaypoint;
	WayPoint previousWaypoint;
	public Animator animatorFront;
	public Animator animatorBack;

	public void OnEnable ()
	{

	}

	void Start()
	{
		if (this.GetComponent<Flatmate> ()) {
			if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
				animatorFront = this.transform.FindChild ("Boy_Front").GetComponent<Animator> ();
				animatorBack = this.transform.FindChild ("Boy_Back").GetComponent<Animator> ();
			} else if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female) {
				animatorFront = this.transform.FindChild ("Girl_Front").GetComponent<Animator> ();
				animatorBack = this.transform.FindChild ("Girl_Back").GetComponent<Animator> ();
			}
		}
		Invoke ("Move", 5f);
	}

	public void NowMove()
	{
		Invoke ("Move", 5f);
	}

	void Move ()
	{
		if(previousWaypoint)
		{
			previousWaypoint._CanBeUsed = true;
			previousWaypoint.gameObject.SetActive (true);	
		}

		if(HostPartyManager.Instance.AttendingParty || SocietyPartyManager.Instance.AttendingParty)
		{
			switch (Random.Range (0, 4)) {
			case 0:
				StartCoroutine (MoveUpwardsInFlatParty ());
				break;
			case 1:
				StartCoroutine (MoveDownwardsInFlatParty ());
				break;
			case 2:
				StartCoroutine (MoveRightInFlatParty ());
				break;
			case 3:
				StartCoroutine (MoveLeftInFlatParty ());

				break;
			}
			
		} else {
			switch (Random.Range (0, 4)) {
			case 0:
				StartCoroutine (MoveUpwards ());
				break;
			case 1:
				StartCoroutine (MoveDownwards ());
				break;
			case 2:
				StartCoroutine (MoveRight ());
				break;
			case 3:
				StartCoroutine (MoveLeft ());

				break;
			}
		}
	}


	#region ForGamePlayMovement

	IEnumerator MoveUpwards ()
	{
		for (int i = 0; i < 5; i++) {
			if (currentWaypoint.upward != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.upward != null && currentWaypoint.upward.GetComponent<WayPoint> ()._CanBeUsed && currentWaypoint.upward.GetActive () == true) {						
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.upward.transform.position, 0.04f);											
						this.GetComponent<CharacterProperties> ().Front.SetActive (false);
						this.GetComponent<CharacterProperties> ().Back.SetActive (true);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	

						yield return null;
					} else
						break;
				}
				if (currentWaypoint.upward) {
					currentWaypoint = currentWaypoint.upward.GetComponent<WayPoint> ();
				
				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		Invoke ("Move", 5f);

	}

	IEnumerator MoveDownwards ()
	{

		for (int i = 0; i < 5; i++) {
			
			if (currentWaypoint.downward != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.downward != null && currentWaypoint.downward.GetComponent<WayPoint> ()._CanBeUsed  &&currentWaypoint.downward.GetActive () == true) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.downward.transform.position, 0.04f);
						this.GetComponent<CharacterProperties> ().Front.SetActive (true);
						this.GetComponent<CharacterProperties> ().Back.SetActive (false);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
						{
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;
				}
				if (currentWaypoint.downward) {
					currentWaypoint = currentWaypoint.downward.GetComponent<WayPoint> ();


				}
			} else
				break;
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		Invoke ("Move", 5f);
	}

	IEnumerator MoveRight ()
	{

		for (int i = 0; i < 5; i++) {

			if (currentWaypoint.right != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.right != null && currentWaypoint.right.GetComponent<WayPoint> ()._CanBeUsed &&currentWaypoint.right.GetActive () == true) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.right.transform.position, 0.04f);
							this.GetComponent<CharacterProperties> ().Front.SetActive (false);
							this.GetComponent<CharacterProperties> ().Back.SetActive (true);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {						
						animatorBack.SetBool ("Back Walk", true);
						animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;
				}
				if (currentWaypoint.right) {
					currentWaypoint = currentWaypoint.right.GetComponent<WayPoint> ();			

				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		Invoke ("Move", 5f);
	}

	IEnumerator MoveLeft ()
	{

		for (int i = 0; i < 5; i++) {
			
			if (currentWaypoint.left != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.left != null && currentWaypoint.left.GetComponent<WayPoint> ()._CanBeUsed &&currentWaypoint.left.GetActive () == true) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.left.transform.position, 0.04f);
							this.GetComponent<CharacterProperties> ().Front.SetActive (true);
							this.GetComponent<CharacterProperties> ().Back.SetActive (false);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
						
						animatorBack.SetBool ("Back Walk", true);
						animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;

				}
				if (currentWaypoint.left) {
					currentWaypoint = currentWaypoint.left.GetComponent<WayPoint> ();				

				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		Invoke ("Move", 5f);

	}
	#endregion

	#region For Flat Party Movment 
	IEnumerator MoveUpwardsInFlatParty ()
	{
		for (int i = 0; i < 5; i++) {
			if (currentWaypoint.upward != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.upward != null && currentWaypoint.upward.GetComponent<WayPoint> ()._CanBeUsed ) {						
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.upward.transform.position, 0.04f);											
						this.GetComponent<CharacterProperties> ().Front.SetActive (false);
						this.GetComponent<CharacterProperties> ().Back.SetActive (true);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<CharacterProperties> ().Gender == "Female"){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	

						yield return null;
					} else
						break;
				}
				if (currentWaypoint.upward) {
					currentWaypoint = currentWaypoint.upward.GetComponent<WayPoint> ();

				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<CharacterProperties> ().Gender == "Female")
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		Invoke ("Move", 5f);

	}

	IEnumerator MoveDownwardsInFlatParty ()
	{

		for (int i = 0; i < 5; i++) {

			if (currentWaypoint.downward != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.downward != null && currentWaypoint.downward.GetComponent<WayPoint> ()._CanBeUsed ) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.downward.transform.position, 0.04f);
						this.GetComponent<CharacterProperties> ().Front.SetActive (true);
						this.GetComponent<CharacterProperties> ().Back.SetActive (false);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<CharacterProperties> ().Gender == "Female")
						{
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;
				}
				if (currentWaypoint.downward) {
					currentWaypoint = currentWaypoint.downward.GetComponent<WayPoint> ();


				}
			} else
				break;
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<CharacterProperties> ().Gender == "Female")
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
		Invoke ("Move", 5f);
	}

	IEnumerator MoveRightInFlatParty ()
	{

		for (int i = 0; i < 5; i++) {

			if (currentWaypoint.right != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.right != null && currentWaypoint.right.GetComponent<WayPoint> ()._CanBeUsed ) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.right.transform.position, 0.04f);
						this.GetComponent<CharacterProperties> ().Front.SetActive (false);
						this.GetComponent<CharacterProperties> ().Back.SetActive (true);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<CharacterProperties> ().Gender == "Male") {						
							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<CharacterProperties> ().Gender == "Female"){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;
				}
				if (currentWaypoint.right) {
					currentWaypoint = currentWaypoint.right.GetComponent<WayPoint> ();			

				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<CharacterProperties> ().Gender == "Female")
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		Invoke ("Move", 5f);
	}

	IEnumerator MoveLeftInFlatParty ()
	{

		for (int i = 0; i < 5; i++) {

			if (currentWaypoint.left != null) {
				for (float j = 0; j < 1f; j += Time.deltaTime) {
					if (currentWaypoint.left != null && currentWaypoint.left.GetComponent<WayPoint> ()._CanBeUsed) {
						transform.position = Vector2.Lerp (transform.position, currentWaypoint.left.transform.position, 0.04f);
						this.GetComponent<CharacterProperties> ().Front.SetActive (true);
						this.GetComponent<CharacterProperties> ().Back.SetActive (false);
						if(previousWaypoint)
						{
							previousWaypoint._CanBeUsed = true;
							previousWaypoint.gameObject.SetActive (true);	
						}
						if (this.GetComponent<CharacterProperties> ().Gender == "Male") {

							animatorBack.SetBool ("Back Walk", true);
							animatorFront.SetBool ("Walk", true);
						}else if(this.GetComponent<CharacterProperties> ().Gender == "Female"){
							animatorBack.SetBool ("Girl Back Walk", true);
							animatorFront.SetBool ("Girl Front Walk", true);
						}
						previousWaypoint = currentWaypoint;
						previousWaypoint._CanBeUsed = false;
						previousWaypoint.gameObject.SetActive (false);	
						yield return null;
					} else
						break;

				}
				if (currentWaypoint.left) {
					currentWaypoint = currentWaypoint.left.GetComponent<WayPoint> ();				

				}
			} else
				break;	
		}
		yield return new WaitForSeconds (1f);
		if (this.GetComponent<CharacterProperties> ().Gender == "Male") {
			animatorBack.SetBool ("Back Walk", false);
			animatorFront.SetBool ("Walk", false);
		}else if(this.GetComponent<CharacterProperties> ().Gender == "Female")
		{
			animatorBack.SetBool ("Girl Back Walk", false);
			animatorFront.SetBool ("Girl Front Walk", false);
		}
		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
		Invoke ("Move", 5f);

	}
	#endregion

//	#region For Society Party Movement 
//	IEnumerator MoveUpwards ()
//	{
//		for (int i = 0; i < 5; i++) {
//			if (currentWaypoint.upward != null) {
//				for (float j = 0; j < 1f; j += Time.deltaTime) {
//					if (currentWaypoint.upward != null && currentWaypoint.upward.GetComponent<WayPoint> ()._CanBeUsed ) {						
//						transform.position = Vector2.Lerp (transform.position, currentWaypoint.upward.transform.position, 0.04f);											
//						this.GetComponent<CharacterProperties> ().Front.SetActive (false);
//						this.GetComponent<CharacterProperties> ().Back.SetActive (true);
//						if(previousWaypoint)
//						{
//							previousWaypoint._CanBeUsed = true;
//							previousWaypoint.gameObject.SetActive (true);	
//						}
//						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//							animatorBack.SetBool ("Back Walk", true);
//							animatorFront.SetBool ("Walk", true);
//						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
//							animatorBack.SetBool ("Girl Back Walk", true);
//							animatorFront.SetBool ("Girl Front Walk", true);
//						}
//						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
//						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
//						previousWaypoint = currentWaypoint;
//						previousWaypoint._CanBeUsed = false;
//						previousWaypoint.gameObject.SetActive (false);	
//
//						yield return null;
//					} else
//						break;
//				}
//				if (currentWaypoint.upward) {
//					currentWaypoint = currentWaypoint.upward.GetComponent<WayPoint> ();
//
//				}
//			} else
//				break;	
//		}
//		yield return new WaitForSeconds (1f);
//		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//			animatorBack.SetBool ("Back Walk", false);
//			animatorFront.SetBool ("Walk", false);
//		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
//		{
//			animatorBack.SetBool ("Girl Back Walk", false);
//			animatorFront.SetBool ("Girl Front Walk", false);
//		}
//		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
//		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
//		Invoke ("Move", 5f);
//
//	}
//
//	IEnumerator MoveDownwards ()
//	{
//
//		for (int i = 0; i < 5; i++) {
//
//			if (currentWaypoint.downward != null) {
//				for (float j = 0; j < 1f; j += Time.deltaTime) {
//					if (currentWaypoint.downward != null && currentWaypoint.downward.GetComponent<WayPoint> ()._CanBeUsed ) {
//						transform.position = Vector2.Lerp (transform.position, currentWaypoint.downward.transform.position, 0.04f);
//						this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//						this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//						if(previousWaypoint)
//						{
//							previousWaypoint._CanBeUsed = true;
//							previousWaypoint.gameObject.SetActive (true);	
//						}
//						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//							animatorBack.SetBool ("Back Walk", true);
//							animatorFront.SetBool ("Walk", true);
//						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
//						{
//							animatorBack.SetBool ("Girl Back Walk", true);
//							animatorFront.SetBool ("Girl Front Walk", true);
//						}
//						animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
//						animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = true;
//						previousWaypoint = currentWaypoint;
//						previousWaypoint._CanBeUsed = false;
//						previousWaypoint.gameObject.SetActive (false);	
//						yield return null;
//					} else
//						break;
//				}
//				if (currentWaypoint.downward) {
//					currentWaypoint = currentWaypoint.downward.GetComponent<WayPoint> ();
//
//
//				}
//			} else
//				break;
//		}
//		yield return new WaitForSeconds (1f);
//		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//			animatorBack.SetBool ("Back Walk", false);
//			animatorFront.SetBool ("Walk", false);
//		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
//		{
//			animatorBack.SetBool ("Girl Back Walk", false);
//			animatorFront.SetBool ("Girl Front Walk", false);
//		}
//		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//		animatorBack.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
//		animatorFront.gameObject.GetComponent<Puppet2D_GlobalControl> ().flip = false;
//		Invoke ("Move", 5f);
//	}
//
//	IEnumerator MoveRight ()
//	{
//
//		for (int i = 0; i < 5; i++) {
//
//			if (currentWaypoint.right != null) {
//				for (float j = 0; j < 1f; j += Time.deltaTime) {
//					if (currentWaypoint.right != null && currentWaypoint.right.GetComponent<WayPoint> ()._CanBeUsed ) {
//						transform.position = Vector2.Lerp (transform.position, currentWaypoint.right.transform.position, 0.04f);
//						this.GetComponent<CharacterProperties> ().Front.SetActive (false);
//						this.GetComponent<CharacterProperties> ().Back.SetActive (true);
//						if(previousWaypoint)
//						{
//							previousWaypoint._CanBeUsed = true;
//							previousWaypoint.gameObject.SetActive (true);	
//						}
//						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {						
//							animatorBack.SetBool ("Back Walk", true);
//							animatorFront.SetBool ("Walk", true);
//						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
//							animatorBack.SetBool ("Girl Back Walk", true);
//							animatorFront.SetBool ("Girl Front Walk", true);
//						}
//						previousWaypoint = currentWaypoint;
//						previousWaypoint._CanBeUsed = false;
//						previousWaypoint.gameObject.SetActive (false);	
//						yield return null;
//					} else
//						break;
//				}
//				if (currentWaypoint.right) {
//					currentWaypoint = currentWaypoint.right.GetComponent<WayPoint> ();			
//
//				}
//			} else
//				break;	
//		}
//		yield return new WaitForSeconds (1f);
//		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//			animatorBack.SetBool ("Back Walk", false);
//			animatorFront.SetBool ("Walk", false);
//		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
//		{
//			animatorBack.SetBool ("Girl Back Walk", false);
//			animatorFront.SetBool ("Girl Front Walk", false);
//		}
//		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//		Invoke ("Move", 5f);
//	}
//
//	IEnumerator MoveLeft ()
//	{
//
//		for (int i = 0; i < 5; i++) {
//
//			if (currentWaypoint.left != null) {
//				for (float j = 0; j < 1f; j += Time.deltaTime) {
//					if (currentWaypoint.left != null && currentWaypoint.left.GetComponent<WayPoint> ()._CanBeUsed) {
//						transform.position = Vector2.Lerp (transform.position, currentWaypoint.left.transform.position, 0.04f);
//						this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//						this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//						if(previousWaypoint)
//						{
//							previousWaypoint._CanBeUsed = true;
//							previousWaypoint.gameObject.SetActive (true);	
//						}
//						if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//
//							animatorBack.SetBool ("Back Walk", true);
//							animatorFront.SetBool ("Walk", true);
//						}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female){
//							animatorBack.SetBool ("Girl Back Walk", true);
//							animatorFront.SetBool ("Girl Front Walk", true);
//						}
//						previousWaypoint = currentWaypoint;
//						previousWaypoint._CanBeUsed = false;
//						previousWaypoint.gameObject.SetActive (false);	
//						yield return null;
//					} else
//						break;
//
//				}
//				if (currentWaypoint.left) {
//					currentWaypoint = currentWaypoint.left.GetComponent<WayPoint> ();				
//
//				}
//			} else
//				break;	
//		}
//		yield return new WaitForSeconds (1f);
//		if (this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Male) {
//			animatorBack.SetBool ("Back Walk", false);
//			animatorFront.SetBool ("Walk", false);
//		}else if(this.GetComponent<Flatmate> ().data.Gender == GenderEnum.Female)
//		{
//			animatorBack.SetBool ("Girl Back Walk", false);
//			animatorFront.SetBool ("Girl Front Walk", false);
//		}
//		this.GetComponent<CharacterProperties> ().Front.SetActive (true);
//		this.GetComponent<CharacterProperties> ().Back.SetActive (false);
//		Invoke ("Move", 5f);
//
//	}
//	#endregi
//
}