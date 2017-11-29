using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActivarGonzalo : MonoBehaviour
{
		public string mensaje;
		public GameObject gonzalo;
		public static GameObject[] equipo;
		public AudioClip g1, g2, g3, g4, g0;
		GameObject gonzaloInstanciado;
		GameObject player;
		ArrayList players;
		public float tiempo = -1f;
		public bool iniciarConversasion = false, persegir = false;
		Vector3 moveDirection = Vector3.zero;
		Animator animator;
		AudioSource voz;

		// Use this for initialization
		void Start ()
		{
				if (General.username == "") {
						SceneManager.LoadScene ("main", LoadSceneMode.Single);
				}
				equipo = new GameObject[3];
				players = new ArrayList ();

				gonzaloInstanciado = (GameObject)Instantiate (gonzalo, new Vector3 (transform.position.x, transform.position.y - 2f, transform.position.z), transform.rotation);
				gonzaloInstanciado.name = "gonzaloJimenez";
				animator = GameObject.Find("Gonzalo-Jimenez-1").GetComponent<Animator> ();

				voz = gonzaloInstanciado.GetComponent<AudioSource> ();
		}
	
		// Update is called once per frame
		void Update ()
		{

				if (persegir) {
						Vector3 target = player.transform.position;

						CharacterController controller = gonzaloInstanciado.GetComponent<CharacterController> ();
			
						if (Vector3.Distance (new Vector3 (target.x, gonzaloInstanciado.transform.position.y, target.z), gonzaloInstanciado.transform.position) > 2) {

								Quaternion rotacion = Quaternion.LookRotation (new Vector3 (target.x, gonzaloInstanciado.transform.position.y, target.z) - gonzaloInstanciado.transform.position);
								gonzaloInstanciado.transform.rotation = Quaternion.Slerp (gonzaloInstanciado.transform.rotation, rotacion, 6.0f * Time.deltaTime);
								moveDirection = new Vector3 (0, 0, 1);
								moveDirection = gonzaloInstanciado.transform.TransformDirection (moveDirection);
								moveDirection *= 2;
								animator.SetFloat ("speed", 1.0f);

						} else {
								animator.SetFloat ("speed", 0.0f);
								moveDirection = Vector3.zero;
								iniciarConversasion = true;
								persegir = false;

						}
			
						moveDirection.y -= 200 * Time.deltaTime;
						controller.Move (moveDirection * Time.deltaTime);


				}

				if (iniciarConversasion) {
						tiempo -= Time.deltaTime;
						voz.enabled = true;
				}

				if (tiempo < 0) {

						if (players.Count >= 1) {

								player = (GameObject)players [0];
								persegir = true;
				
								if (General.paso_mision == 2) {
										tiempo = 25;
								} else if (General.paso_mision == 3) {
										tiempo = 15;


										if (General.paso_mision == 3) {
												tiempo = 15;
												Camera.main.GetComponent<Misiones> ().procesoMision2 (General.paso_mision);
										}
								} else {
										
										if (General.paso_mision >= 3) {
												tiempo = 5;
												mensaje = "podéis pasar, sean respetuosos con su virrey.";
												if (voz.clip.name != g4.name) {
														voz.clip = g4;
														voz.Play ();
												}
												//GameObject.Find ("puerta").GetComponent<BoxCollider> ().isTrigger = true;
										} else {
												tiempo = 8;
												mensaje = "Recuerda que debes unirte con otros compañeros y completar tres tributos.\n Así podrán hablar con el virrey. ";
										}
								}

								players.RemoveAt (0);
						} else {
								iniciarConversasion = false;
								persegir = false;
						}
				}

				if (tiempo < 0 && General.paso_mision <= 2 ) {
						GameObject reja = GameObject.Find ("Reja");
						if(reja.transform.localPosition.z > -4.798f)
							reja.transform.Translate (-0.02f, 0, 0);
				}

				if (tiempo < 0) {
						voz.enabled = false;
				}
		}

		void OnGUI ()
		{
				GameObject reja = GameObject.Find ("Reja");
				if (iniciarConversasion && player.GetComponent<NetworkView> ().isMine) {

						switch (General.paso_mision) {
						case 2: 
								if (tiempo > 20) {
										if (voz.clip.name != g1.name) {
												voz.clip = g1;
												voz.Play ();
										}
										mensaje = "Bienvenidos a Altagracia de Sumapaz,";
								} else if (tiempo > 15) {
										if (voz.clip.name != g2.name) {
												voz.clip = g2;
												voz.Play ();
										}
										mensaje = "para poder hablar con el virrey, debes traer un tributo.";
								} else if (tiempo > 8) {
										
										mensaje = "He observado que traes tu tributo";
								} else if (tiempo > 0) {
										if (voz.clip.name != g4.name) {
												voz.clip = g4;
												voz.Play ();
										}
										mensaje = "podéis pasar, sean respetuosos con su virrey";
										reja.transform.Translate(0.01f, 0, 0);
								}
								break;
						case 3:
								if (voz.clip.name != g0.name) {
										voz.clip = g0;
										voz.Play ();
								}
								mensaje = "Recuerda que debes unirte con otros compañeros y completar \n" +
									"tres tributos. Así podrán hablar con el virrey. ";
								break;
						case 4: 
								if (tiempo > 8) {
										mensaje = "He observado que traen sus tributos";
								} else if (tiempo > 1) {
										if (voz.clip.name != g4.name) {
												voz.clip = g4;
												voz.Play ();
										}
										mensaje = "podéis pasar, sean respetuosos con su virrey";
										//GameObject.Find ("puerta").GetComponent<BoxCollider> ().isTrigger = true;
								}
								if(reja.transform.localPosition.z < 4.1f)
										reja.transform.Translate(0.01f, 0, 0);

								break;
				
						}

						if (tiempo > 0 && General.paso_mision >= 5 && reja.transform.localPosition.z < 4.1f) {
								reja.transform.Translate (0.02f, 0, 0);
						}
						GUIStyle style = new GUIStyle ();
						style.alignment = TextAnchor.MiddleCenter;
						style = GUI.skin.GetStyle ("Box");
						style.fontSize = (int)(20.0f);
			
						GUI.Box (new Rect (Screen.width/10, 3*Screen.height/4, 2*(Screen.width/3),Screen.height/4), mensaje);

						if (General.paso_mision == 2 && General.misionActual [0] == "2" && tiempo < 0.5) {
								//General.timepo = 10;
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								mision.procesoMision2 (General.paso_mision);
								tiempo = 0;
						}
						
						if (General.paso_mision == 4 && General.misionActual [0] == "2" && tiempo < 0.5) {
							//General.timepo = 10;
							Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
							mision.procesoMision2 (General.paso_mision);
						}
						MoverMouse.movimiento = true;
				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						players.Add (colision.gameObject);
				}
		}
}
