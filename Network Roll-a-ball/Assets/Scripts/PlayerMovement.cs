using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

	private Rigidbody rb;
	public float Speed;
	public GameObject prefab;
//	private bool isLocalPlayerAuthority = false;
	private NetworkInstanceId ownerNetID; 
	public ParticleSystem explosion;
	public GameObject pickup;

	void Start (){
		rb = GetComponent<Rigidbody> ();
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
		// Destroy (other.gameObject); //

		//other.gameObject.GetComponent<CubeMovement> ().connectionToServer.clientOwnedObjects.Contains (other.GetComponent<NetworkIdentity> ());
		if (isLocalPlayer){

		//isLocalPlayerAuthority = other.gameObject.GetComponent<NetworkIdentity>().localPlayerAuthority;

		if (other.gameObject.CompareTag("PickUp")){
				//if (other.ownerNetID != gameObject.GetComponent<NetworkIdentity>().netId) {
				CmdAssignAuthority(other.gameObject.GetComponent<NetworkIdentity>());

				Color myColour = new Color(Random.value, Random.value, Random.value);
				// other.GetComponent<Renderer> ().material.SetColor ("_Color", Color.blue);
				other.GetComponent<Renderer> ().material.SetColor ("_Color", myColour);

				Debug.Log ("Authority assigned to PickUp");
				Debug.Log (other.gameObject.GetComponent<NetworkIdentity>().ToString());

				var Explosion = Instantiate (explosion, other.transform.position, other.transform.rotation);
				Destroy (other.gameObject, 2.0f);
				Destroy (Explosion, 2.0f);

				var PickUp = Instantiate (pickup, other.transform.position, other.transform.rotation);

			} else {
				return;
			}
		}
	}



	public void AssignAuthority (NetworkIdentity netid){
		CmdAssignAuthority (netid);
	}

	[Command]
	void CmdAssignAuthority (NetworkIdentity netid){
			netid.AssignClientAuthority (connectionToClient);
	}

}

/*NetworkServer.SpawnWithClientAuthority(go, base.connectionToClient);

AssignClientAuthority(NetworkConnection conn) 
RemoveClientAuthority(NetworkConnection conn) 


 myColor = new Color(Random.value, Random.value, Random.value);
 */