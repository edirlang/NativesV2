using UnityEngine;
using System.Collections;

public class recaudador : MonoBehaviour {

		public string mensaje;
		public GameObject recaudador_game, titulo, llave, alfonso, quina, cafe;
		GameObject player;
		ArrayList players;
		public AudioClip r1, r2;
		public float tiempo = -1f;
		public bool iniciarConversasion = false, persegir = false;
		Vector3 moveDirection = Vector3.zero;
		Animator animator;
		AudioSource voz;
		// Use this for initialization
		void Start ()
		{
				if (General.username == "") {
						Application.LoadLevel ("main");
				}
				players = new ArrayList ();
				animator = recaudador_game.GetComponent<Animator> ();
				voz = recaudador_game.GetComponent<AudioSource> ();
		}

		// Update is called once per frame
		void Update ()
		{

				if (persegir) {
						moveDirection = Vector3.zero;
						iniciarConversasion = true;
						if (General.paso_mision > 9) {
								tiempo = 5;
						}else{
							tiempo = 17;
						}
						persegir = false;
				}

				if (iniciarConversasion) {
						tiempo -= Time.deltaTime;
				}

				if (tiempo < 0) {
						iniciarConversasion = false;
				}

				if (General.paso_mision >= 7) {
						Vector3 target = alfonso.transform.position;

						CharacterController controller = recaudador_game.GetComponent<CharacterController> ();

						if (Vector3.Distance (new Vector3 (target.x, recaudador_game.transform.position.y, target.z), recaudador_game.transform.position) > 2) {
								Quaternion rotacion = Quaternion.LookRotation (new Vector3 (target.x, recaudador_game.transform.position.y, target.z) - recaudador_game.transform.position);
								recaudador_game.transform.rotation = Quaternion.Slerp (recaudador_game.transform.rotation, rotacion, 6.0f * Time.deltaTime);
								moveDirection = new Vector3 (0, 0, 1);
								moveDirection = recaudador_game.transform.TransformDirection (moveDirection);
								moveDirection *= 2;
								animator.SetFloat ("speed",1.0f);
						} else {
								moveDirection = Vector3.zero;
								animator.SetFloat ("speed",0.0f);
						}
						moveDirection.y -= 200 * Time.deltaTime;
						controller.Move (moveDirection * Time.deltaTime);
				}
		}

		void OnGUI ()
		{
				if (iniciarConversasion && player.GetComponent<NetworkView> ().isMine) {

						if (General.paso_mision == 6) {
								if (voz.clip.name != r1.name) {
										voz.clip = r1;
										voz.Play ();
								}
								if (tiempo > 13) {

										mensaje = "Bienvenido a esta nueva ciudad.";

								} else if (tiempo > 8) {
										mensaje = "Te recibo el título de propiedad y 30 monedas\n" +
											"de oro de impuestos de tu casa.";

										if (!GameObject.Find ("titulo")) {
												General.monedas -= 30;
												GameObject permisoObj = (GameObject)Instantiate (titulo, transform.position, transform.rotation);
												permisoObj.transform.parent = player.transform;
												permisoObj.transform.rotation = new Quaternion ();
												permisoObj.transform.Rotate (300, 0, 0);
												permisoObj.transform.localPosition = new Vector3 (-0.95f, 0.5858f, 2.3f);
												permisoObj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
												permisoObj.name = "titulo";

										}
								} 
								else if (tiempo > 0) {
										mensaje = "Gracias, te entrego las llaves de tu nuevo hogar, " +
											"\n tu casa la puedes encontrar en las casas que \n" +
											"ves al lado de nosotros. Ve y encuéntrala.";

										if (!GameObject.Find ("llave")) {
												GameObject llaveObj = (GameObject)Instantiate (llave, recaudador_game.transform.position, transform.rotation);
												llaveObj.transform.parent = player.transform;
												llaveObj.transform.localPosition = new Vector3 (2.14f, 0.84f, 2.08f);
												llaveObj.name = "llave"; 
										}
								}

								if (General.paso_mision == 6 && General.misionActual [0] == "3" && tiempo < 0.5) {
										//General.timepo = 10;
										if (GameObject.Find ("llave")) {
												Destroy (GameObject.Find ("titulo"));
												Destroy (GameObject.Find ("llave"));
										}
										iniciarConversasion = false;
										Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
										mision.procesoMision3 (General.paso_mision);
								}
						} else if (General.paso_mision == 10) {
								if (voz.clip.name != r2.name) {
										voz.clip = r2;
										voz.Play ();
								}
								mensaje = "Gracias, Alfonso te envía este regalo por ayudarle.";
								if (!GameObject.Find ("cafe")) {
										General.monedas -= 30;
										GameObject quinaobj = (GameObject)Instantiate (quina, transform.position, transform.rotation);
										quinaobj.transform.parent = player.transform;
										quinaobj.transform.rotation = new Quaternion ();
										quinaobj.transform.Rotate (300, 0, 0);
										quinaobj.transform.localPosition = new Vector3 (-0.95f, 0.5858f, 2.3f);
										quinaobj.name = "quina";

										GameObject cafeObj = (GameObject)Instantiate (cafe, recaudador_game.transform.position, transform.rotation);
										cafeObj.transform.parent = player.transform;
										cafeObj.transform.localPosition = new Vector3 (2.14f, 0f, 2.08f);
										cafeObj.name = "cafe"; 
								}

								if (General.paso_mision == 10 && General.misionActual [0] == "3" && tiempo < 0.5) {
										//General.timepo = 10;
										if (GameObject.Find ("cafe")) {
												Destroy (GameObject.Find ("quina"));
												Destroy (GameObject.Find ("cafe"));
										}
										iniciarConversasion = false;
										Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
										mision.procesoMision3 (General.paso_mision);
								}
						}else {
								mensaje = "Hola, bienvenido a Fusagasugá";
						}

						GUIStyle style = new GUIStyle ();
						style.alignment = TextAnchor.MiddleCenter;
						style = GUI.skin.GetStyle ("Box");
						style.fontSize = (int)(20.0f);

						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4, 2*(Screen.width/3),Screen.height/4), mensaje);

						style.fontSize = (int)(15.0f);
						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4 - Screen.height/24, Screen.width/4,Screen.height/24),"Recaudador");

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
