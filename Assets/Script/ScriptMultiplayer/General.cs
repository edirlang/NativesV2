using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class General : MonoBehaviour {

	public static int salud, monedas;
	public static string username="",nickname="";
	public static GameObject chia;
	public GameObject chiaPrefab;
	public static GameObject personaje;
	public static Vector3 posicionIncial;
	public static int idPersonaje, paso_mision=1;
		public static string hosting = "http://nativesudec.000webhostapp.com/API/index.php/";
	//public static string hosting = "localhost/API/index.php/";

		public static bool conectado = false, bono=false, mensajeRecojer = false;
	public static string[] misionActual = new string[3];
	public static float timepoChia=10, timepo=-1;
		public Texture mapa;
		bool gameOvwer=false;
		float tiempoOvwer;
	// Use this for initialization
	void Start () {
				
				salud = 1;
		chia = chiaPrefab;
		PlayerPrefs.GetInt ("salud",3);
		//personaje = personajeDefault;
		DontDestroyOnLoad (this);
				gameOvwer = false;
	}
	
	// Update is called once per frame
	void Update () {
				DontDestroyOnLoad (this);
				PlayerPrefs.SetInt ("salud", salud);

				if (salud <= 0) {
						MoverMouse.cambioCamara = false;
						SceneManager.LoadScene("gameOver");
						General.paso_mision = 1;
						General.salud = 3;
				}
				if (tiempoOvwer > 0) {
						tiempoOvwer -= Time.deltaTime;
						Camera.main.clearFlags = CameraClearFlags.Color;
						Camera.main.backgroundColor = Color.black;

				} else {
						//Camera.main.clearFlags = CameraClearFlags.Skybox;
				}

				if (gameOvwer && tiempoOvwer <= 0) {
						SceneManager.LoadScene("level1");
						gameOvwer = false;

						Destroy (GameObject.Find ("Main Camera"));
						GameObject.Find ("Main Camera2").name = "Main Camera";
						MoverMouse.cambioCamara = false;
				}

		}

	

	public static IEnumerator consultarPersonajeUsername(WWW www){
		yield return www;
		if(www.error == null){
						Debug.Log (www.text);
			General.idPersonaje = int.Parse(www.text);
		}else{
			Debug.Log(www.error);
		}
	}

	public static IEnumerator actualizarUser(){
		string url = General.hosting + "logout";
		WWWForm form = new WWWForm ();
		form.AddField ("username", General.username);
		form.AddField("mision",General.misionActual[0] + "");
		form.AddField("pos_x", General.posicionIncial.x + "");
		form.AddField("pos_y", (General.posicionIncial.y + 2) + "");
		form.AddField("pos_z", General.posicionIncial.z + "");
		form.AddField("vidas", General.salud + "");
		form.AddField("monedas", General.monedas + "");
		form.AddField("bono", General.bono + "");
		form.AddField("paso", General.paso_mision + "");
		WWW www = new WWW (url, form);
		yield return www;
		if(www.error == null){
			Debug.Log(www.text);
		}else{
			Debug.Log(www.error);
		}
	}

	public static IEnumerator cambiarMision(){
		string url = General.hosting + "subirLevel";
		WWWForm form = new WWWForm ();
		form.AddField ("username", General.username);
		form.AddField("mision",General.misionActual[0] + "");
		form.AddField("x", General.posicionIncial.x + "");
		form.AddField("y", General.posicionIncial.y + "");
		form.AddField("z", General.posicionIncial.z + "");
		WWW www = new WWW (url, form);
		yield return www;
		if(www.error == null){
			Misiones.cambio_mapa = true;
						if(General.misionActual[0]=="5"){
								General.misionActual[0] = "1";
						}
		}else{
			Debug.Log(www.error);
		}
	}
}
