using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cura : MonoBehaviour {

		public string mensaje;
		public static GameObject[] equipo;
		public GameObject cura, cruz;
		public Animator animator;
		GameObject player;
		public float tiempo = -1f;
		public bool iniciarConversasion = false, persegir = false;
		Vector3 moveDirection = Vector3.zero;
		AudioSource voz;
		// Use this for initialization
		void Start ()
		{
				if (General.username == "") {
						SceneManager.LoadScene("main");
				}
				voz = cura.GetComponent<AudioSource> ();
		}

		// Update is called once per frame
		void Update ()
		{
				if (persegir) {
						Vector3 target = player.transform.position;

						CharacterController controller = cura.GetComponent<CharacterController> ();

						if (Vector3.Distance (new Vector3 (target.x, cura.transform.position.y, target.z), cura.transform.position) > 2) {
								Quaternion rotacion = Quaternion.LookRotation (new Vector3 (target.x, cura.transform.position.y, target.z) - cura.transform.position);
								cura.transform.rotation = Quaternion.Slerp (cura.transform.rotation, rotacion, 6.0f * Time.deltaTime);
								moveDirection = new Vector3 (0, 0, 1);
								moveDirection = cura.transform.TransformDirection (moveDirection);
								moveDirection *= 2;
								animator.SetFloat ("speed",1.0f);
						} else {
								animator.SetFloat ("speed",0.0f);
								moveDirection = Vector3.zero;
								iniciarConversasion = true;
								tiempo = 24;
								if (General.paso_mision != 2) {
										tiempo = 10;
								} else {
										voz.Play ();
								}
								persegir = false;

								
						}
						moveDirection.y -= 200 * Time.deltaTime;
						controller.Move (moveDirection * Time.deltaTime);
				}

				if (iniciarConversasion) {
						tiempo -= Time.deltaTime;
				}

				if (tiempo < 0) {
						iniciarConversasion = false;
				}
		}

		void OnGUI ()
		{
				if (iniciarConversasion && player.GetComponent<NetworkView> ().isMine) {

						if (General.paso_mision == 2) {
								
								if (tiempo > 18) {
										mensaje = "Bienvenido a la nueva iglesia, \n " +
											"esta iglesia fue reconstruida en 1776,";

								} else if (tiempo > 12) {
										mensaje = "mostrando la construcción de la nueva ciudad.";
								} else if (tiempo > 8) {
										mensaje = "Desde aquí tu comunidad y tu empiezan a tomar \n" +
											"la religión católica";
								}else if (tiempo > 0) {
										mensaje = "para ello te entrego esta cruz, \n " +
											"símbolo de nuestra religión católica.";
										if (!GameObject.Find ("cruz")) {
												GameObject cruzObj = (GameObject)Instantiate (cruz, transform.position, transform.rotation);
												cruzObj.transform.parent = player.transform;
												cruzObj.transform.rotation = new Quaternion ();
												cruzObj.transform.Rotate (300,0,0);
												cruzObj.transform.localPosition = new Vector3 (-0.95f, 0.5858f, 2.3f );
												cruzObj.name = "cruz";
										}
								}

								if (General.paso_mision == 2 && General.misionActual [0] == "3" && tiempo < 0.5) {
										//General.timepo = 10;
										if (GameObject.Find ("cruz")) {
												Destroy (GameObject.Find ("cruz"));
										}
										iniciarConversasion = false;
										Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
										mision.procesoMision3 (General.paso_mision);
								}
						} else {
								mensaje = "Bienvenido a la nueva iglesia.";
						}

						GUIStyle style = new GUIStyle ();
						style.alignment = TextAnchor.MiddleCenter;
						style = GUI.skin.GetStyle ("Box");
						style.fontSize = (int)(20.0f);

						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4, 2*(Screen.width/3),Screen.height/4), mensaje);

						style.fontSize = (int)(15.0f);
						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4 - Screen.height/24, Screen.width/3,Screen.height/24),"Antonio Martinez (Cura)");
						MoverMouse.movimiento = true;
				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						player = colision.gameObject;
						persegir = true;
				}
		}
}