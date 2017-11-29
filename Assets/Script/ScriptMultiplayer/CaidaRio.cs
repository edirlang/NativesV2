using UnityEngine;
using System.Collections;

public class CaidaRio : MonoBehaviour {

	bool perdioVida=false;
	CharacterController controller;
	public GameObject chiaPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(perdioVida)
		{
						if (GameObject.Find ("Chia(Clone)")) {
								Destroy (GameObject.Find ("Chia(Clone)"));
						} 
						General.timepoChia = 10;
						GameObject player = GameObject.Find (Network.player.ipAddress);
						GameObject chia = Instantiate (chiaPrefab, player.transform.position, player.transform.rotation) as GameObject;
						if (General.misionActual [0] == "3") {
								chia.transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
						}
						chia.GetComponent<ChiaPerseguir> ().mensajeChia = "Haz perdido una vida \nTen cuidado la proxima vez";
						chia.transform.parent = player.transform;
						chia.transform.localPosition = new Vector3 (0f, 8f, 25f);

						perdioVida = false;
		}
	}

	void OnTriggerEnter(Collider collider)
	{

		if (collider.gameObject.name == Network.player.ipAddress) {

			collider.gameObject.transform.position = new Vector3(0,0,0);
			controller = collider.gameObject.GetComponent<CharacterController>();
			controller.enabled = false;
			GameObject objetoAparecer;
			if(GameObject.Find("PlayerJuego"))
				objetoAparecer = GameObject.Find("PlayerJuego");
			else
				objetoAparecer = GameObject.Find("PlayerJuego2");
			collider.gameObject.transform.position = objetoAparecer.transform.position;
			controller.enabled = true;
			perdioVida = true;
			General.salud--;
			StartCoroutine(General.actualizarUser());
		}
	}
}