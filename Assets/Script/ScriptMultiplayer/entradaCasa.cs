using UnityEngine;
using System.Collections;

public class entradaCasa : MonoBehaviour
{

		GameObject player;
		public bool soyEntrar;
		bool trasportar;
		public GameObject dentro, fuera, camara;
		float tiempo;
		public int numeroCasa;
		// Use this for initialization
		void Start ()
		{
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
				if (General.username == "") {
						Application.LoadLevel ("main");
				}
				trasportar = false;
				tiempo = 0;
		}

		// Update is called once per frame
		void Update ()
		{
				if (trasportar) {
						if(soyEntrar)
								player.transform.position = new Vector3(dentro.transform.position.x,dentro.transform.position.y,dentro.transform.position.z - 2f);
						else
								player.transform.position = new Vector3(fuera.transform.position.x,fuera.transform.position.y,fuera.transform.position.z + 5f);
						trasportar = false;

				}

				tiempo -= Time.deltaTime;	
		}

		void OnGUI ()
		{
				if (tiempo > 0) {
						if (player.GetComponent<NetworkView> ().isMine) {
								if (tiempo > 0) {
										string mensaje = "";

										GUIStyle style = new GUIStyle ();
										style.alignment = TextAnchor.MiddleCenter;
										style = GUI.skin.GetStyle ("Box");
										style.fontSize = (int)(20.0f);

										//GUI.Box (new Rect (0, 3 * Screen.height / 4, Screen.width, Screen.height / 4), mensaje + "de tu casa");
								}
								if (tiempo < 1 && tiempo > 0) {
										MoverMouse.cambioCamara = false;
								}
						}
				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						player = colision.gameObject;
						if (General.paso_mision == 7 && General.misionActual [0] == "2") {
								
								tiempo = 5;
								MoverMouse.cambioCamara = true;
								trasportar = true;
								if (!soyEntrar) {
										Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
										mision.procesoMision2 (General.paso_mision);
								}
						} else if (General.paso_mision == 7 && General.misionActual [0] == "3") {
								
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								if (soyEntrar && numeroCasa == mision.numeroLlave) {
										MoverMouse.cambioCamara = true;
										trasportar = true;
										mision.procesoMision3 (General.paso_mision);
								}
						} else if (General.paso_mision == 8 && General.misionActual [0] == "3") {
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								if (numeroCasa == mision.numeroLlave) {
										trasportar = true;
								}
						} else if (General.paso_mision > 8 && General.paso_mision < 11 && General.misionActual [0] == "3") {
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								if (numeroCasa == mision.numeroLlave) {
										trasportar = true;
								}
						}else if(General.paso_mision == 11 && General.misionActual[0]=="3"){
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								if (soyEntrar && numeroCasa == mision.numeroLlave) {
										mision.procesoMision3 (General.paso_mision);
										MoverMouse.cambioCamara = true;
										trasportar = true;
								}
						}
				}

		}
}
