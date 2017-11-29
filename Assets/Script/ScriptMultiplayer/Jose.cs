using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Jose : MonoBehaviour {

		public string mensaje;
		public GameObject jose, quina;
		GameObject player;
		public float tiempo = -1f;
		public bool iniciarConversasion = false, persegir = false;
		Vector3 moveDirection = Vector3.zero;
		Animator animator;
		AudioSource voz;
		// Use this for initialization
		void Start ()
		{
				if (General.username == "") {
						SceneManager.LoadScene("main");
				}
				animator = GameObject.Find("Jose-Celestino-Mutis").GetComponent<Animator> ();
				voz = jose.GetComponent<AudioSource> ();
		}

		// Update is called once per frame
		void Update ()
		{
				if (persegir) {
						Vector3 target = player.transform.position;

						CharacterController controller = jose.GetComponent<CharacterController> ();

						if (Vector3.Distance (new Vector3 (target.x, jose.transform.position.y, target.z), jose.transform.position) > 2) {
								Quaternion rotacion = Quaternion.LookRotation (new Vector3 (target.x, jose.transform.position.y, target.z) - jose.transform.position);
								jose.transform.rotation = Quaternion.Slerp (jose.transform.rotation, rotacion, 6.0f * Time.deltaTime);
								moveDirection = new Vector3 (0, 0, 1);
								moveDirection = jose.transform.TransformDirection (moveDirection);
								moveDirection *= 2;
								animator.SetFloat ("speed", 1.0f);
						} else {
								moveDirection = Vector3.zero;
								iniciarConversasion = true;
								tiempo = 22;
								if (General.paso_mision != 9) {
										tiempo = 5;
								}else {
										voz.Play ();
								}
								persegir = false;
								animator.SetFloat ("speed", 0.0f);
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

						if (General.paso_mision == 9) {
								if (tiempo > 19	) {
										mensaje = "Bienvenido, esta es la casona la venta, \n ";
								}else if (tiempo > 10) {
										mensaje = "aquí estoy realizando varias investigaciones sobre la fauna\n" +
												"del Sumapaz. Me ha llegado información de que \n" +
												"Alfonso López está enfermo";
								}else if (tiempo > 6) {
										mensaje = "y no ha podido publicar las leyes \n " +
											"Llévale esta planta,";
										if (!GameObject.Find ("quina")) {
												GameObject cruzObj = (GameObject)Instantiate (quina, transform.position, transform.rotation);
												cruzObj.transform.parent = player.transform;
												cruzObj.transform.rotation = new Quaternion ();
												cruzObj.transform.Rotate (300,0,0);
												cruzObj.transform.localPosition = new Vector3 (-0.95f, 0.5858f, 2.3f );
												cruzObj.name = "quina";
										}
								}else if (tiempo > 0.5) {
										mensaje = "se llama Quina, es muy usada como medicina. " +
											"\n Ve rápido, no te demores mucho.";
								}

								if (General.paso_mision == 9 && General.misionActual [0] == "3" && tiempo < 0.5) {
										//General.timepo = 10;
										if (GameObject.Find ("quina")) {
												Destroy (GameObject.Find ("quina"));
										}
										iniciarConversasion = false;
										Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
										mision.procesoMision3 (General.paso_mision);
								}
						} else {
								mensaje = "Bienvenido a la Casona la Venta.";
						}

						GUIStyle style = new GUIStyle ();
						style.alignment = TextAnchor.MiddleCenter;
						style = GUI.skin.GetStyle ("Box");
						style.fontSize = (int)(20.0f);

						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4, 2*(Screen.width/3),Screen.height/4), mensaje);

						style.fontSize = (int)(15.0f);
						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4 - Screen.height/24, Screen.width/3,Screen.height/24),"Jose Celestino Mutis");

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
