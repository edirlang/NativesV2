using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Conexion : MonoBehaviour
{

		private const string typeName = "Natives-v1.0";
		private string ipServer = "", remoteIp = "", remotePort = "25000";
		public GameObject chozaFinal;
		public Texture corazonTexture;
		public Texture monedasTexture;
		public Texture ayudaTexture, chat, menu, correr1, correr2, volver, volverConexion, maletaText, piedra;
		public string textoAyuda = "Chia";
		public static string mensaje = "";
		private ArrayList mensajes;
		public GameObject prefab, chia, gonzalo;
		public Vector3 rotacion;
		private string idPersonaje;
		private bool salir = false, abrirMenu = false, verChat = false;
		private Vector2 scrollPosition;
		private int numeroMensajes = 0;
		private NetworkView nw;
		private Color color;
		private float tiempo = 30, tiempo_reinicio = 0.5f;
		public bool reiniciar;
		public GUIStyle stilobotones;

		void Start ()
		{
				if (General.username == "") {
						Application.LoadLevel ("main");
						Destroy (gameObject);
						Destroy (GameObject.Find ("Luz"));
				}

				color = new Color (Random.Range (0.0f, 0.7f), Random.Range (0.0f, 0.7f), Random.Range (0.0f, 0.7f));
				mensajes = new ArrayList ();
				nw = GetComponent<NetworkView> ();
				DontDestroyOnLoad (this.gameObject);
				reiniciar = false;
				if (Application.isMobilePlatform) {
						tiempo_reinicio = 2f;
				}
		}

		void Update ()
		{
				
				if (GameObject.Find ("MainCamera2")) {
						Destroy (GameObject.Find ("MainCamera2"));
				}

				if (GameObject.Find ("Main Camera")) {
						Destroy (GameObject.Find ("Main Camera"));
				}

				if (GameObject.Find ("camaraPrincipal") && Application.loadedLevelName != "introduccion") {
						Destroy (GameObject.Find("Main Camera"));
						GameObject.Find ("camaraPrincipal").name = "Main Camera";
				}
				if (reiniciar) {
						tiempo_reinicio -= Time.deltaTime;
						if (GameObject.Find ("PlayerJuego2")) {
								GameObject.Find ("PlayerJuego2").name = "PlayerJuego";
						}
						if (tiempo_reinicio < 0) {
								GameObject.Find (Network.player.ipAddress).transform.position = GameObject.Find ("PlayerJuego").transform.position;
								reiniciar = false;
						}
				}
		}

		void  OnGUI ()
		{
				if (General.salud <= 0) {
						return;
				}

				GUIStyle style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleLeft;
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f);

				style = GUI.skin.GetStyle ("button");
				style.fontSize = (int)(20.0f);

				style = GUI.skin.GetStyle ("textfield");
				style.fontSize = (int)(20.0f);

				// Checking if you are connected to the server or not
				if (Network.peerType == NetworkPeerType.Disconnected) {
						if (hayJugadores ())
								Application.LoadLevel ("SelecionarPersonaje");
						pantallaServidor ();
						if (GUI.Button (new Rect (25 * (Screen.width / 32), 5 * (Screen.height / 6), Screen.width / 5, Screen.height / 10), "Volver al Menú")) {
								Application.LoadLevel ("menu");
								Debug.Log ("Volver aL Menú");
								if (Application.isMobilePlatform) {
										Destroy (gameObject,1f);
								} else {
										Destroy (gameObject);
								}
						}
				} else {
						pantallaJuego ();
						if (!abrirMenu && !verChat) {
								if (GUI.Button (new Rect (15 * (Screen.width / 16), 5 * (Screen.height / 6), Screen.height/8, Screen.height/8), menu)) {
										abrirMenu = true;
										MoverMouse.movimiento = false;
										MoverMouse.cambioCamara = true;
								}
								if (GameObject.Find (Network.player.ipAddress).GetComponent<MoverMouse> ().speed == 3) {
										if (GUI.Button (new Rect (13 * (Screen.width / 16), 4 * (Screen.height / 6), Screen.height / 8, 5 * (Screen.height / 16)), correr1)) {
												GameObject.Find (Network.player.ipAddress).GetComponent<MoverMouse> ().speed = 8f;
												GameObject.Find (Network.player.ipAddress).GetComponent<movimiento> ().speed = 8f;
										}
								} else {
										if (GUI.Button (new Rect (13 * (Screen.width / 16), 4 * (Screen.height / 6), Screen.height / 8, 5 * (Screen.height / 16)), correr2)) {
												GameObject.Find (Network.player.ipAddress).GetComponent<MoverMouse> ().speed = 3f;
												GameObject.Find (Network.player.ipAddress).GetComponent<movimiento> ().speed = 3f;
										}
								}
						}
				}

				if (salir) {
						//nw.RPC ("guardarDatos", RPCMode.All,General.username);
						StartCoroutine (General.actualizarUser ());
						Network.Disconnect (500);
						if (General.misionActual [0] == "4") {
									//Camera.main.transform.parent = GameObject.Find (Network.player.ipAddress).transform;
								Application.LoadLevel ("menu");
						} else {
								Application.LoadLevel ("lobyScena");
						}
						GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
						foreach (GameObject jugador in players) {
								Destroy(jugador);
						}
						Destroy (this.gameObject,100f);
				}
		}

		private void pantallaJuego ()
		{
				GUIStyle style = new GUIStyle ();
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f);
				mensajesEnviados ();

				if (Network.isServer) {
						GUI.Label (new Rect (Screen.width / 2 - Screen.width / 8, 0, Screen.width / 4, Screen.height / 12), "TU IP: " + Network.player.ipAddress);
						//GUI.Label (new Rect (7*(Screen.width / 10), Screen.height - Screen.height / 9, Screen.width / 4, Screen.height / 9),"Puerto: " + Network.player.port.ToString() );
				}

				style.fontSize = (int)(25.0f);
				style.alignment = TextAnchor.LowerLeft;
				// Vidas
				GUI.Box (new Rect (Screen.width - 5 * (Screen.width / 20) , 10, Screen.width / 10, Screen.height / 9), corazonTexture, style);
				GUI.Label (new Rect (Screen.width - 4 * (Screen.width / 20), 10, Screen.width / 10, Screen.height / 9), "x " + General.salud + "");

				//Monedas
				GUI.Box (new Rect (Screen.width - 3 * (Screen.width / 20), 10, Screen.width / 10, Screen.height / 9), monedasTexture, style);

				GUI.Label (new Rect (Screen.width - 2 * (Screen.width / 20), 10, Screen.width / 10, Screen.height / 9), "x " + General.monedas);

				// Ayuda
				if (GUI.Button (new Rect (Screen.width/30 , 9 * (Screen.height / 12) , Screen.height/6, Screen.height / 6), ayudaTexture) && Application.loadedLevelName != "introduccion") {
						Misiones.instanciar = true;
						MoverMouse.movimiento = false;
						MoverMouse.cambioCamara = true;
				}

				if (abrirMenu) {
						GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "Menú Pausa");

						if (GUI.Button (new Rect (Screen.width / 12, Screen.height /3, Screen.width / 6, Screen.height / 4), new GUIContent(volverConexion,"Desconectar"), stilobotones)) {
								StartCoroutine (desconectarUser ());
						}

						if (GUI.Button (new Rect (7*(Screen.width / 24), Screen.height /3, Screen.width / 6, Screen.height / 4),  new GUIContent(maletaText,"Maleta"), stilobotones)) {
								Maleta maleta = Camera.main.gameObject.GetComponent<Maleta> ();
								maleta.mostarMaleta = true;

								abrirMenu = false;
						}

						if (GUI.Button (new Rect (12*(Screen.width / 24), Screen.height /3, Screen.width / 6, Screen.height / 4),  new GUIContent(piedra,"Piedra Hogar"), stilobotones)) {
								abrirMenu = false;
								MoverMouse.movimiento = false;
								if (GameObject.Find ("PlayerJuego"))
										GameObject.Find (Network.player.ipAddress).transform.position = GameObject.Find ("PlayerJuego").transform.position;
								else {
										GameObject.Find (Network.player.ipAddress).transform.position = GameObject.Find ("PlayerJuego2").transform.position;
								}
								MoverMouse.movimiento = true;
						}

						if (GUI.Button (new Rect (17*(Screen.width / 24), Screen.height /3, Screen.width / 6, Screen.height / 4),new GUIContent(volver,"volver"), stilobotones)) {
								abrirMenu = false;
								MoverMouse.movimiento = true;
								MoverMouse.cambioCamara = false;
						}

				}

				//Chat
				if (!abrirMenu) {
						if (verChat) {
								style = GUI.skin.GetStyle ("label");
								style.fontSize = (int)(15.0f);
								chatVer ();
						} else {
								if (GUI.Button (new Rect (15 * (Screen.width / 16), 4 * (Screen.height / 6), Screen.height/8, Screen.height/8), chat)) {
										verChat = true;
										MoverMouse.movimiento = false;
										MoverMouse.cambioCamara = true;
								}
						}
				}
		}

		private void pantallaServidor ()
		{
				GUI.Box (new Rect (0, 0, Screen.width, (Screen.height)), "Bienvenido a Natives");

				if (Network.peerType == NetworkPeerType.Disconnected) {
						GUI.Label (new Rect (Screen.width / 24, 2 * (Screen.height / 10), 2 * (Screen.width / 3), (Screen.height / 10)), "Deseas ser el anfitrión de tus amigos");

						if (GUI.Button (new Rect (2 * (Screen.width / 3), 2 * (Screen.height / 10), (Screen.width / 6), (Screen.height / 10)), "Crear Sala")) {
								StartServer ();
						}

						GUI.Label (new Rect (Screen.width / 24, 4 * (Screen.height / 10), Screen.width, (Screen.height / 10)), "Deseas conectarte a una sala");

						GUI.Label (new Rect (Screen.width / 24, 5 * (Screen.height / 10), 2 * (Screen.width / 3), (Screen.height / 10)), "Escribe el numero IP de tu amigo");
						remoteIp = GUI.TextField (new Rect (7 * (Screen.width / 12), 5 * (Screen.height / 10), Screen.width / 4, (Screen.height / 10)), remoteIp);
						//GUI.Label (new Rect(Screen.width/24, 6*(Screen.height/10), 2*(Screen.width/3), 5*(Screen.height/10)),"Escribe el numero del puerto de tu amigo");
						//remotePort = GUI.TextField(new Rect(7*(Screen.width/12), 6*(Screen.height/10), Screen.width/4, (Screen.height/10)),remotePort);

						if (GUI.Button (new Rect (7 * (Screen.width / 12), 5 * (Screen.height / 6), Screen.width / 6, Screen.height / 10), "Conectar")) {
								JoinServer ();
						}
				}
		}

		private void StartServer ()
		{
				Network.InitializeServer (20, 25000, false);
				ipServer = Network.player.ipAddress;
				//SpawnPlayer ();
		}

		void OnServerInitialized ()
		{
				SpawnPlayer ();
		}

		private void JoinServer ()
		{
				Network.Connect (remoteIp, int.Parse (remotePort));
		}


		void  OnConnectedToServer ()
		{
				Network.isMessageQueueRunning = false;
				SpawnPlayer ();
		}

		private void SpawnPlayer ()
		{
				if (Application.isMobilePlatform) {
						General.posicionIncial.y += 10f;
				}

				GameObject g = (GameObject)Network.Instantiate (General.personaje, new Vector3 (General.posicionIncial.x, General.posicionIncial.y + 10f, General.posicionIncial.z), transform.rotation, 0);
				g.transform.localScale = new Vector3 (2, 2, 2);
				g.AddComponent<BoxCollider> ();
				g.GetComponent<BoxCollider> ().size = new Vector3 (0.1f, 0.1f, 0.1f);

				g.name = Network.player.ipAddress;

				Network.isMessageQueueRunning = true;

				if (GameObject.Find("Luz_tormenta")) {
						Camera.main.GetComponent<Misiones>().luzrayos = GameObject.Find ("Luz_tormenta");
						Camera.main.GetComponent<Misiones> ().luzrayos.SetActive (false);
				}
				switch(int.Parse(General.misionActual[0])){
				case 1:
						if (General.paso_mision == 1 && General.misionActual [0] == "1") {
								Application.LoadLevel("introduccion");
						} else {
								Application.LoadLevel("level1");
						}
						break;
				case 2:
						Application.LoadLevel("level2");
						break;
				case 3:
						Application.LoadLevel("level2");
						break;
				}
				//GameObject g = (GameObject) Network.Instantiate (General.personaje, transform.position, transform.rotation, 0);
				//g.name = Network.player.ipAddress;
		}

		public IEnumerator desconectarUser ()
		{
				string url = General.hosting + "logout";
				WWWForm form = new WWWForm ();
				form.AddField ("username", General.username);
				form.AddField ("mision", General.misionActual [0] + "");
				form.AddField ("pos_x", General.posicionIncial.x + "");
				form.AddField ("pos_y", General.posicionIncial.y + "");
				form.AddField ("pos_z", General.posicionIncial.z + "");
				form.AddField ("vidas", General.salud + "");
				form.AddField ("monedas", General.monedas + "");
				form.AddField ("bono", General.bono + "");
				form.AddField ("paso", General.paso_mision + "");
				WWW www = new WWW (url, form);
				yield return www;
				if (www.error == null) {
						Debug.Log (www.text);
						MoverMouse.cambioCamara = false;
						salir = true;
				} else {
						Debug.Log (www.error);
				}
		}

		public void chatVer ()
		{
				GUIStyle style = new GUIStyle ();
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(25.0f);
				style.alignment = TextAnchor.LowerLeft;



				if (mensajes == null) {
						mensajes = new ArrayList ();
				}
				numeroMensajes = mensajes.Count;
				scrollPosition [1] = 20 * (Screen.height / 16);

				GUI.Box (new Rect (2 * (Screen.width / 3), 0, Screen.width / 3, Screen.height), "Chat");
				string mensaje = "";
				scrollPosition = GUI.BeginScrollView (new Rect (2 * (Screen.width / 3), 0, Screen.width / 3, Screen.height), scrollPosition, new Rect (0, 0, Screen.width, Screen.height));
				mensaje = cargarMensajes ();
				GUI.EndScrollView ();

				if (mensaje != "")
						send (mensaje);
		}

		void mensajesEnviados ()
		{
				if (mensajes.Count > 0) {
						tiempo -= Time.deltaTime;
						numeroMensajes = mensajes.Count;
						if (numeroMensajes >= 3) {
								numeroMensajes = 3;
						}
						for (int i = 0; i < numeroMensajes; i++) {
								string[] mensajeNuevo = (string[])mensajes [i];
								string[] colorRGB = mensajeNuevo [2].Split (',');

								string[] mensajeConcatenado = mensajeNuevo [1].Split ('[');
								GUI.color = new Color (float.Parse (colorRGB [0]), float.Parse (colorRGB [1]), float.Parse (colorRGB [2]));

										GUI.Label (new Rect (Screen.width / 12, (numeroMensajes - i) * (Screen.height / 10), 2 * Screen.width, Screen.height / 10), mensajeNuevo [0] + ": " + mensajeNuevo [1]);

						}

						GUI.color = Color.white;
						if (tiempo <= 0) {
								mensajes.RemoveAt (0);
								tiempo = 30;
						}
				}
		}

		public void send (string mensaje)
		{
				MoverMouse.movimiento = true;
				MoverMouse.cambioCamara = false;
				nw.RPC ("recibir", RPCMode.AllBuffered, mensaje, General.username, color.r + "," + color.g + "," + color.b);
		}

		bool hayJugadores ()
		{
				bool hayjugador = false;
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject player in players) {
						if (player != null)
								hayjugador = true;
				}
				return hayjugador;
		}

		string cargarMensajes ()
		{
				string mensaje = "";
				if (GUI.Button (new Rect (0, (Screen.height / 16), Screen.width / 3, Screen.height / 16), "Hola, Suerte")) {
						mensaje = "Hola, Suerte";
						verChat = false;
				}
				if (General.misionActual [0] == "1") {
						if (GUI.Button (new Rect (0, 2 * (Screen.height / 16), Screen.width / 3, Screen.height / 16), "¿Dónde consigo Madera?")) {
								mensaje = "¿Dónde consigo Madera?";
								verChat = false;
						}

						if (GUI.Button (new Rect (0, 3 * (Screen.height / 16), Screen.width / 3, Screen.height / 16), "¿Dónde consigo arcilla?")) {
								mensaje = "¿Dónde consigo arcilla?";
								verChat = false;
						}

						if (GUI.Button (new Rect (0, 4 * (Screen.height / 16), Screen.width / 3, 2 * (Screen.height / 16)), "¿Dónde consigo Hojas de \n Palma boba?")) {
								mensaje = "¿Dónde consigo Hojas de Palma boba?";
								verChat = false;
						}
				} else if (General.misionActual [0] == "2") {
						if (GUI.Button (new Rect (0, 2 * (Screen.height / 16), Screen.width / 3, 2 * (Screen.height / 16)), "¿Dónde esta Nuestra \n señora de Altagracia?")) {
								mensaje = "¿Dónde esta Nuestra señora de Altagracia?";
								verChat = false;
						}

						if (GUI.Button (new Rect (0, 4 * (Screen.height / 16), Screen.width / 3, 2 * (Screen.height / 16)), "¿Necesito un equipo?")) {
								mensaje = "¿Necesito un equipo?";
								verChat = false;
						}

						if (GUI.Button (new Rect (0, 6 * (Screen.height / 16), Screen.width / 3, 2 * (Screen.height / 16)), "Estoy en Altagracia")) {
								mensaje = "Estoy en Altagracia";
								verChat = false;
						}

						if (GUI.Button (new Rect (0, 8 * (Screen.height / 16), Screen.width / 3, 2 * (Screen.height / 16)), "Estoy en Fusagasuga")) {
								mensaje = "Estoy en Fusagasuga";
								verChat = false;
						}
				}
				return mensaje;
		}

		void OnPlayerDisconnected (NetworkPlayer player)
		{
				Debug.Log ("Clean up after player " + player);
				Network.RemoveRPCs (player);
				Network.DestroyPlayerObjects (player);
				salir = true;
		}
				
		[RPC]
		public void recibir (string text, string usuario, string color)
		{
				string[] mensajeNuevo = { usuario, text, color }; 
				mensajes.Add (mensajeNuevo);
		}

		[RPC]
		public void crearChozaMultiplayer (string usuario, Vector3 posicionInstanciar, int nivel)
		{
				if (General.misionActual [0] == "1") {
						GameObject chozaLevel = (GameObject)Instantiate (chozaFinal, new Vector3 (posicionInstanciar.x, posicionInstanciar.y - 2, posicionInstanciar.z - 5), new Quaternion ()); 
						chozaLevel.transform.Rotate (-90f, 0f, 0f);
						chozaLevel.transform.localScale = new Vector3 (4.0f, 4.0f, 3.0f);
						chozaLevel.name = "choza-" + usuario;
				}
		}

		[RPC]
		public void guardarDatos (string usuario)
		{
				StartCoroutine (General.actualizarUser ());
		}
}