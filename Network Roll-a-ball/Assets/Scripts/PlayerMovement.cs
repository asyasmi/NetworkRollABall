using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

	private Rigidbody rb;
	public float Speed;
	private GameObject[] points; 
	private NetworkInstanceId ownerNetID; 
	public ParticleSystem explosion;
	public GameObject pickup;

	//On start of work
	void Start (){
		rb = GetComponent<Rigidbody> ();
		points = GameObject.FindGameObjectsWithTag ("SpawnPoint");

		for (int i = 0; i < points.Length; i++) {
			Instantiate (pickup, points [i].transform.position, points [i].transform.rotation);
		}
	}
		
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.white;
	}


	// Use this for initialization
	void FixedUpdate () {
		if (!isLocalPlayer) {
			return;
		} else {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			rb.AddForce (movement * Speed);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

		if (isLocalPlayer){
			
			if (other.gameObject.CompareTag("PickUp")){
					CmdAssignAuthority(other.gameObject.GetComponent<NetworkIdentity>());

					Color myColour = new Color(Random.value, Random.value, Random.value);
					other.GetComponent<Renderer> ().material.SetColor ("_Color", myColour);

					Debug.Log ("Authority assigned to PickUp");
					Debug.Log (other.gameObject.GetComponent<NetworkIdentity>().ToString());

					var Explosion = Instantiate (explosion, other.transform.position, other.transform.rotation);
					Destroy (other.gameObject, 2.0f);
					Destroy (Explosion, 2.0f);
					
					SpawnPickUp (other.transform.position, other.transform.rotation);

				} else {
					return;
				}
		}
	}


	void SpawnPickUp (Vector3 position, Quaternion rotation){

		int i =Random.Range (0, points.Length - 1);
		/*
		point.transform.position.x = point.transform.position.x + Random.Range (-5.0f, 5.0f);
		point.transform.position.z = point.transform.position.x + Random.Range (-5.0f, 5.0f);

		point.transform.rotation.x = point.transform.rotation.x + Random.Range (-2.0f, 2.0f);
		point.transform.rotation.z = point.transform.rotation.z + Random.Range (-2.0f, 2.0f);*/

		var PickUp = Instantiate (pickup, points[i].transform.position, points[i].transform.rotation);
	}

	[Command]
	void CmdAssignAuthority (NetworkIdentity netid){
		if (netid.RemoveClientAuthority(connectionToClient))
			netid.RemoveClientAuthority (connectionToClient);
		
		netid.AssignClientAuthority (connectionToClient);
		Debug.Log ("assigned!");
	}




}