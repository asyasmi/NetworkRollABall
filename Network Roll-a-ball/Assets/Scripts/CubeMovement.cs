﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CubeMovement : NetworkBehaviour {

	private Rigidbody rb;
	public float Speed;
	public GameObject prefab;
	private bool isLocalPlayerAuthority = false;
	private NetworkInstanceId ownerNetID; 

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
				//other.gameObject.GetComponent<NetworkBehaviour>().is
				other.GetComponent<Renderer> ().material.SetColor ("_Color", Color.blue);
				Debug.Log ("Authority assigned to PickUp");
				Debug.Log (other.gameObject.GetComponent<NetworkIdentity>().ToString());
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