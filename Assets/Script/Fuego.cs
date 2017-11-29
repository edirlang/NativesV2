using UnityEngine;
using System.Collections;

public class Fuego : MonoBehaviour
{

		public GameObject fuego;
		float tiempo = 0;
		bool quemarse;
		// Use this for initialization
		void Start ()
		{
				quemarse = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (quemarse) {
						GameObject mequemo =(GameObject) Instantiate (fuego, transform.position, transform.rotation);
						mequemo.transform.parent = GameObject.Find (Network.player.ipAddress).transform;
						mequemo.transform.localPosition = Vector3.zero;
						mequemo.name = "meQuemo";
						quemarse = false;
				}

				tiempo -= Time.deltaTime;

				if (tiempo < 0 && GameObject.Find ("meQuemo")) {
						Destroy (GameObject.Find ("meQuemo"));
				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						quemarse = true;
						tiempo = 5;

				}
		}
}
