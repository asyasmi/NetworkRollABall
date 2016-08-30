using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

	public float speed;
	public ParticleSystem explosion;
	public GameObject pickup;
	public Text countText;
	public Text winText;
	public GameObject player;

	[SyncVar]
	private int score;

	private Rigidbody rb;
	private GameObject[] points; 
	private NetworkInstanceId ownerNetID; 
	private GameObject spawnPoint;

	//On start of work
	void Start (){

		// spawnPoint = GameObject.FindGameObjectWithTag ("SpawnPointPlayer"); 
		//Instantiate (player, spawnPoint.transform.position, spawnPoint.transform.rotation);

		rb = player.GetComponent<Rigidbody> ();

		winText = GameObject.Find ("WinText").GetComponent<Text>();
		countText = GameObject.Find ("CountText").GetComponent<Text>();
		 
		winText.text = "Hello! Start collecting! "; 
		score = 0;	

		points = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		for (int i = 0; i < points.Length; i++) {
			Instantiate (pickup, points[i].transform.position, points[i].transform.rotation);
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
			rb.AddForce (movement * speed);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator CleanWinText() {
		yield return new WaitForSeconds(5);
		winText.text = ("");
	}



	void OnTriggerEnter(Collider other){
		
		if (winText.text != "") {
			StartCoroutine(CleanWinText());
		}

		if (isLocalPlayer){
			
			if (other.gameObject.CompareTag("PickUp")){
				CmdAssignAuthority(other.gameObject.GetComponent<NetworkIdentity>());

				Color myColour = new Color(Random.value, Random.value, Random.value);
				other.GetComponent<Renderer> ().material.SetColor ("_Color", myColour);

				CmdSetCountText (gameObject.GetComponent<NetworkIdentity>());

				var Explosion = Instantiate (explosion, other.transform.position, other.transform.rotation);
				
				CmdDestroyPickUp (other.gameObject);
				Destroy (Explosion, 2.0f);
				
				SpawnPickUp (other.transform.position, other.transform.rotation);

				} else {
					return;
				}
		}
	}


	[Command]
	void CmdSetCountText(NetworkIdentity netId){
		countText.text = "Score: " + score.ToString () + ", for " + netId.ToString();

		if (score >= 15) {
			winText.text = "Victory!";
		}
	}

	[Command]
	void CmdDestroyPickUp(GameObject go){
		Destroy (go, 2.0f);
	}

	void SpawnPickUp (Vector3 position, Quaternion rotation){

		int i =Random.Range (0, points.Length - 1);

		var PickUp = Instantiate (pickup, points[i].transform.position, points[i].transform.rotation);
	}

	[Command]
	void CmdAssignAuthority (NetworkIdentity netid){
		if (netid.hasAuthority)
			CmdRemoveClientAuthority (netid);
		else
			return;
		
		netid.AssignClientAuthority (connectionToClient);

		Debug.Log (netId.ToString());
		Debug.Log ("assigned!");
	}

	[Command]
	void CmdRemoveClientAuthority(NetworkIdentity netid){
		netid.RemoveClientAuthority (connectionToClient);
	}



}