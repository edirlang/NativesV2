using UnityEngine;
using System.Collections;

public class interfaz_login : MonoBehaviour
{

		public Texture BoxTexture;
		public Texture fondo;
		private string username = "";
		private string Usuario = "";
		private bool resultado = false;
		private string mensaje;
		private bool registrar = false, loginAutomatico;
		private string Nombre = "", Apellido = "";

		// Use this for initialization
		void Start ()
		{
				loginAutomatico = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				
		}

		void OnGUI ()
		{
				if (registrar) {
						this.registrarUsuario ();
				} else {
						this.login ();
				}
		}

		public void login ()
		{
				Nombre = "";
				GUIStyle style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(30.0f);
				style.alignment = TextAnchor.MiddleCenter;
		
				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(30.0f);
				style.alignment = TextAnchor.UpperCenter;
		
				style = GUI.skin.GetStyle ("button");
				style.fontSize = (int)(30.0f);
		
				style = GUI.skin.GetStyle ("textField");
				style.fontSize = (int)(30.0f);


				if (!resultado) {
						style.alignment = TextAnchor.MiddleCenter;

						//GUI.Box (new Rect (0, 0, Screen.width, Screen.height), fondo);
						style = GUI.skin.GetStyle ("label");
						style.fontSize = (int)(40.0f);

						GUI.Label (new Rect (Screen.width / 2 - Screen.width / 4, Screen.height / 10, Screen.width / 2, (Screen.height / 10)), "Bienvenidos a Natives");

						style = GUI.skin.GetStyle ("label");
						style.fontSize = (int)(30.0f);

						if (GUI.Button (new Rect (5 * (Screen.width / 8), 6 * (Screen.height / 8), Screen.width / 4, Screen.height / 10), "Salir")) {
								Application.Quit ();
						}

						GUI.Label (new Rect (Screen.width / 4, 3 * (Screen.height / 7), Screen.width / 4, Screen.height / 10), "Usuario:");
						username = GUI.TextField (new Rect (2 * (Screen.width / 4), 3 * (Screen.height / 7), Screen.width / 4, Screen.height / 10), username, 25);

						style = GUI.skin.GetStyle ("label");
						style.fontSize = (int)(20.0f);
						if (mensaje == "El usuario ha sido creado") {
								GUI.color = Color.green;
						} else {
								GUI.color = Color.red;
						}
						GUI.Label (new Rect (Screen.width / 6, 3 * (Screen.height / 14), Screen.width / 2 + Screen.width / 4, Screen.height / 10), mensaje);
						GUI.color = Color.white;
						if (GUI.Button (new Rect (Screen.width / 2 - Screen.width / 8, 4 * (Screen.height / 7), Screen.width / 4, Screen.height / 10), "Ingresar")) {
								string url = General.hosting + "login";
								WWWForm form = new WWWForm ();
								form.AddField ("username", username);
								WWW www = new WWW (url, form);
								StartCoroutine (comprobarUser (www));
						}

						style = GUI.skin.GetStyle ("button");
						style.fontSize = (int)(30.0f);
						GUI.color = Color.blue;
						if (GUI.Button (new Rect (1 * (Screen.width / 8), 6 * (Screen.height / 8), Screen.width / 4, Screen.height / 10), "Registrar")) {
								registrar = true;
						}

						if (loginAutomatico) {
								string url = General.hosting + "login";
								WWWForm form = new WWWForm ();
								form.AddField ("username", Usuario);
								WWW www = new WWW (url, form);
								StartCoroutine (comprobarUser (www));
								loginAutomatico = false;
						}
				} else {
						General.username = username;
						Application.LoadLevel ("selecionarPersonaje");
			
				}

				GUI.color = Color.white;
		}

		public void registrarUsuario ()
		{

				GUI.color = Color.red;
				GUI.Label (new Rect (Screen.width / 6, Screen.height / 14, Screen.width / 2 + Screen.width / 4, Screen.height / 10), mensaje);
				GUI.color = Color.white;

				GUIStyle style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(30.0f);
				style.alignment = TextAnchor.UpperLeft;

				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(40.0f);

				style = GUI.skin.GetStyle ("button");
				style.fontSize = (int)(30.0f);

				style = GUI.skin.GetStyle ("textField");
				style.fontSize = (int)(30.0f);

				style = GUI.skin.GetStyle ("toggle");
				style.fontSize = (int)(30.0f);

				GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "Registro de Usuario");

				GUI.Label (new Rect (Screen.width / 6, (Screen.height / 7), Screen.width / 3, Screen.height / 10), "Nombre");
				Nombre = GUI.TextField (new Rect (2 * (Screen.width / 4), (Screen.height / 7), Screen.width / 4, Screen.height / 12), Nombre, 50);

				GUI.Label (new Rect (Screen.width / 6, 2 * (Screen.height / 7), Screen.width / 3, Screen.height / 10), "Apellido");
				Apellido = GUI.TextField (new Rect (2 * (Screen.width / 4), 2 * (Screen.height / 7), Screen.width / 4, Screen.height / 12), Apellido, 50);
		
				GUI.Label (new Rect (Screen.width / 6, 3 * (Screen.height / 7), Screen.width / 4, Screen.height / 10), "Usuario");
				Usuario = GUI.TextField (new Rect (2 * (Screen.width / 4), 3 * (Screen.height / 7), Screen.width / 4, Screen.height / 12), Usuario, 50);

				if (GUI.Button (new Rect (Screen.width / 6, 5 * (Screen.height / 7), Screen.width / 5, Screen.height / 14), "Atras")) {
						registrar = false;
						mensaje = "";
				}

				if (GUI.Button (new Rect (2 * (Screen.width / 4), 5 * (Screen.height / 7), Screen.width / 5, Screen.height / 14), "Guardar")) {
						if (validarusuario ()) {
								username = Usuario;
								string url = General.hosting + "registrar";
								WWWForm form = new WWWForm ();
								form.AddField ("username", username);
								form.AddField ("nombre", Nombre);
								form.AddField ("apellido", Apellido);
								WWW www = new WWW (url, form);
								StartCoroutine (registrarUser (www));
						}
				}
		}

		public IEnumerator comprobarUser (WWW www)
		{
				yield return www;
				if (www.error == null) {
						Debug.Log (www.text.Length);
						if (www.text.Length >= 1 && www.text.Length <= 3) {
								resultado = true;
						} else {
								mensaje = www.text;
								Debug.Log ("nombre de usuario o contraseña no son correctas");		
						}
				} else {
						Debug.Log (www.error);
						mensaje = "Verifica tu conexión de red";
				}
		}

		public IEnumerator registrarUser (WWW www)
		{
				yield return www;
				if (www.error == null) {
						if (www.text.Length == 2 || www.text.Length == 1) {
								mensaje = "El usuario ha sido creado";
								registrar = false;
								loginAutomatico = true;
						} else {
								
								mensaje = "No se logro crear tu cuenta";
								Debug.Log (www.text);
						}
				} else {
						Debug.Log (www.error);
						mensaje = www.error;
			
				}
		}

		private bool validarusuario ()
		{

				if (Nombre == "") {
						mensaje = "Por favor escribe tu Nombre";
						return false;
				} else if (Apellido == "") {
						mensaje = "Por favor escribe tu Apellido";
						return false;
				} else if (Usuario == "") {
						mensaje = "Por favor escribe tu Nomnbre de Usuario (username)";
						return false;
				} else {
						mensaje = "";	
						return true;
				}
		}
}
