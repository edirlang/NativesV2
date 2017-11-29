using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoverMouse : MonoBehaviour
{
		Vector3 posicion;
		private Vector3 moveDirection = Vector3.zero;
		public float speed = 3f, gravity;
		public static bool movimiento, equipo = false;
		public static bool cambioCamara = false;
		bool solicitudEquipo = false, ubicar=true;
		float x,y;
		private NetworkView nw;
		private Animator animator;

		public static Vector3 targetObjeto;
		public Texture mapa;

		// Use this for initialization
		void Start ()
		{
				DontDestroyOnLoad (this.gameObject);

				MoverMouse.movimiento = true;
				nw = GetComponent<NetworkView> ();
				animator = GetComponent<Animator> ();

				posicion = transform.position;

				if (General.paso_mision == 1 && nw.isMine && Application.loadedLevelName != "lobyScena" && Application.loadedLevelName != "introduccion") {
						Debug.Log ("Llamando a chia en "+ Application.loadedLevelName);
						if (General.misionActual [0] == "2") {
								General.timepo = 35;
								General.timepoChia = 35;
						} else if (General.misionActual [0] == "3") {
								General.timepo = 30;
								General.timepoChia = 30.5f;
						} else {
								General.timepo = 15;
								General.timepoChia = 15;
						}
						Misiones.instanciar = true;
				}
		}

		// Update is called once per frame
		void Update ()
		{
				General.posicionIncial = transform.position;
				animator = GetComponent<Animator> ();
				nw = GetComponent<NetworkView> ();

				if (General.paso_mision == 0 || General.paso_mision == 1 ) {
						if (GameObject.Find ("PlayerJuego") && ubicar) {
								transform.position = GameObject.Find ("PlayerJuego").transform.position;
								ubicar = false;
						}
				}

				if(GameObject.Find ("camara") && nw.isMine){
						GameObject camaraMapa = GameObject.Find ("camara");
						camaraMapa.transform.parent = gameObject.transform;
						camaraMapa.transform.localPosition = new Vector3(0f,10f,0f);
				}

				if (nw.isMine && !cambioCamara) {
						GameObject camara = Camera.main.gameObject;
						camara.transform.parent = transform;
						camara.transform.localRotation = new Quaternion ();
						camara.transform.Rotate (new Vector3 (20f, 0, 0));
						camara.transform.localPosition = new Vector3 (-0.352941f, 1.576233f, -1.929336f);
				}



				if (!Application.isMobilePlatform) {
					return;
				}

				Debug.Log ("celular");
				Ray ray = new Ray ();

				if (Input.GetMouseButtonDown (0)) {
						RaycastHit hit;
						ray = Camera.main.ScreenPointToRay (Input.mousePosition);

						if (Physics.Raycast (ray, out hit)) {
								posicion = hit.point;
						}
				}

				if (nw.isMine) {
						mover (posicion);
				}

		}

		private void mover (Vector3 target)
		{
				float distaciapunto = 0.5f;

				CharacterController controller = GetComponent<CharacterController> ();

				if (Vector3.Distance (new Vector3 (target.x, transform.position.y, target.z), transform.position) > distaciapunto && movimiento) {

						Quaternion rotacion = Quaternion.LookRotation (new Vector3 (target.x, transform.position.y, target.z) - transform.position);
						transform.rotation = Quaternion.Slerp (transform.rotation, rotacion, 6.0f * Time.deltaTime);
						moveDirection = new Vector3 (0, 0, 1);
						moveDirection = transform.TransformDirection (moveDirection);
						moveDirection *= speed;

						animator.SetFloat ("speed", 1.0f);
						if(Application.loadedLevelName != "introduccion")
								nw.RPC ("activarCaminar", RPCMode.AllBuffered, 1.0f);


				} else {
						moveDirection = Vector3.zero;
						animator.SetFloat ("speed", 0.0f);
						if(Application.loadedLevelName != "introduccion")
								nw.RPC ("activarCaminar", RPCMode.AllBuffered, 0.0f);
				}

				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move (moveDirection * Time.deltaTime);

		}

		[RPC]
		void activarCaminar (float valor)
		{
				if (animator != null)
						animator.SetFloat ("speed", valor);
		}
}