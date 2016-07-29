using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private GameObject player;
	private Vector3 offset;
	private bool isPlayerFound = false;

	// Use this for initialization
	void Start () {
		//cameraPos = transform.position;
	}
	void Update(){
		//if (GameObject.FindGameObjectWithTag ("Player")){
			player = GameObject.FindGameObjectWithTag ("Player");
			if (player != null && !isPlayerFound) {
				isPlayerFound = true; 
				offset = transform.position - player.transform.position;
			} 
		/*else {
			transform.position = cameraPos;
			isPlayerFound = false;
		}*/
	}
	// Update is called once per frame
	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position + offset;
		}
	}
}
