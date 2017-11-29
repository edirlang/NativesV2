using UnityEngine;
using System.Collections;

public class ChozaLevel : MonoBehaviour {

		GameObject player;
		float tiempo;
		bool entro;
	// Use this for initialization
	void Start () {
				tiempo = 5;
				entro = false;
				GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
				if (entro) {
						tiempo -= Time.deltaTime;

						if (tiempo < 0) {
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
								mision.procesoMision1(General.paso_mision);
								entro = false;
						}
				}
	}

		void OnGUI(){
				GUIStyle style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleCenter;
				style = GUI.skin.GetStyle ("Box");
				style.fontSize = (int)(20.0f );
				if(entro && Camera.main.GetComponent<NetworkView>().isMine)
				{
						GUI.Box(new Rect(0,3*Screen.height/4, Screen.width,Screen.height/4),"Vas a ser llevado al futuro, al año 1592, las cosas van a cambiar");
				}
		}

		public void OnTriggerEnter(Collider colision){
				if (colision.name == Network.player.ipAddress) {
						player = colision.gameObject;

						entro = true;
				}
		}
}
