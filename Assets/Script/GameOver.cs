using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
		public float tiempo = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
				tiempo -= Time.deltaTime;
			
				if (tiempo < 0) {
								NetworkView nw = Camera.main.GetComponent<NetworkView> ();
								Network.Disconnect (200);
								nw.RPC ("guardarDatos", RPCMode.All,General.username);
								Application.LoadLevel ("menu");
								GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
								foreach (GameObject jugador in players) {
										Destroy(jugador);
								}
								Destroy (Camera.main,100f);

				}
	
	}
}
