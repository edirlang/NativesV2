using UnityEngine;
using System.Collections;

public class inicio : MonoBehaviour {
	public Texture BoxTexture;
	public Material materialselecionarPersonaje, materialCargando;
	public Texture cuadroTexture;
	public Texture pj1Texture;
	public Texture pj2Texture;
	public Texture pj3Texture;
	public GameObject pj1,pj2,pj3, objetoInstanciar, pj1_mostar, pj2_mostar, pj3_mostar;
	private string username = General.username, caracteristicas = "";
	private bool continuar = false, correcto = false;
	private int tienePersonaje = 0;

	// Use this for initialization
	void Start () {
		if(General.username == "")
		{
			Application.LoadLevel("main");
		}


		string url = General.hosting+"consultarPersonaje";
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		WWW www = new WWW(url, form);
		StartCoroutine(consultarSitienePersonaje(www));

		string url2 = General.hosting+"consultarPersonajeId";
		WWWForm form2 = new WWWForm();
		form2.AddField("username", General.username);
		WWW www2 = new WWW(url2, form2);
		StartCoroutine(General.consultarPersonajeUsername(www2));
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindGameObjectWithTag ("Player"))
		{
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.transform.Rotate(Vector3.up, Time.deltaTime * 30, Space.World);
		}
	}
	void OnGUI(){
		if(tienePersonaje == 0){
			continuar = true;
		}else{
			continuar = false;
		}

		if (tienePersonaje == 2 || correcto)
		{
			if(General.idPersonaje == 1)
			{
				General.personaje = pj1;
			}else if (General.idPersonaje == 2)
			{
				General.personaje = pj2;
			}else if(General.idPersonaje == 3)
			{
				General.personaje = pj3;
			}

			continuar = true;
			if(General.personaje != null)
				Application.LoadLevel ("menu");
		}
		if(!continuar)
		{
			if (GUI.Button (new Rect (Screen.width / 9,(Screen.height/16), Screen.width / 10, Screen.height/4), pj1Texture)) {
				General.idPersonaje = 1;
				General.personaje = pj1;
				GameObject otro = GameObject.FindGameObjectWithTag ("Player");
				Destroy (otro);

				GameObject personaje = Instantiate (pj1_mostar, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
				personaje.tag = "Player";

				string url = General.hosting+"consultarPersonajeCaracteristicas";
				WWWForm form = new WWWForm();
				form.AddField("id", 1);
				WWW www = new WWW(url, form);
				StartCoroutine(consultarPersonaje(www));
			}
			
			if (GUI.Button (new Rect (Screen.width / 9,5*(Screen.height/16), Screen.width / 10, Screen.height/4), pj2Texture)) {
				General.idPersonaje = 2;
				General.personaje = pj2;
				GameObject otro = GameObject.FindGameObjectWithTag ("Player");
				Destroy (otro);

				GameObject personaje = Instantiate (pj2_mostar, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
				personaje.tag = "Player";

				string url = General.hosting+"consultarPersonajeCaracteristicas";
				WWWForm form = new WWWForm();
				form.AddField("id", 2);
				WWW www = new WWW(url, form);
				StartCoroutine(consultarPersonaje(www));
			}
			
			if (GUI.Button (new Rect (Screen.width / 9,9*(Screen.height/16), Screen.width / 10, Screen.height/4), pj3Texture)) {
				General.idPersonaje = 3;
				General.personaje = pj3;
				GameObject otro = GameObject.FindGameObjectWithTag ("Player");
				Destroy (otro);

				GameObject personaje = Instantiate (pj3_mostar, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
				personaje.tag = "Player";

				string url = General.hosting+"consultarPersonajeCaracteristicas";
				WWWForm form = new WWWForm();
				form.AddField("id", 3);
				WWW www = new WWW(url, form);
				StartCoroutine(consultarPersonaje(www));
				System.Threading.Thread.Sleep(1000);
			}

			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;

			GUI.color = Color.black;
			GUIStyle styleLabel = GUI.skin.GetStyle ("label");
			styleLabel.fontSize = (int)(40.0f );
			styleLabel.alignment = TextAnchor.UpperCenter;

			GUI.Label(new Rect(0, Screen.height/64, Screen.width, Screen.height/2),"ELIGE TU PERSONAJE");
			styleLabel.fontSize = (int)(30.0f );
						GUI.Label(new Rect(6*(Screen.width/9), Screen.height/6, 3*(Screen.width/9), Screen.height/2),"Características");

			GUI.color = Color.white;
			GUI.Label(new Rect(6*(Screen.width/9), 2*(Screen.height/7), 3*(Screen.width/9), Screen.height/2),cuadroTexture);
			GUI.color = Color.blue;
			GUI.Label(new Rect(6*(Screen.width/9), 2*(Screen.height/7), 3*(Screen.width/9), Screen.height/2),caracteristicas);


			GUI.color = Color.white;
			GUIStyle styleButton = GUI.skin.GetStyle ("Button");
			styleButton.fontSize = (int)(30.0f );
			if (GUI.Button (new Rect (3*(Screen.width / 8), 20 *(Screen.height/24), Screen.width / 5, Screen.height/12), "Crear")) {
				if(validarPersonaje())
				{
					string url = General.hosting+"crearPersonaje";
					WWWForm form = new WWWForm();
					form.AddField("username", General.username);
					form.AddField("id", General.idPersonaje);
					WWW www = new WWW(url, form);
					StartCoroutine(crearPersonaje(www));
				}
			}
		}else{
			GUI.Box(new Rect(0,0, Screen.width, Screen.height), BoxTexture);
			GUI.Label(new Rect(Screen.width - Screen.width/4 , Screen.height-Screen.height/6, Screen.width/4, Screen.height/6),"Cargando...");
		}
	}

	public IEnumerator consultarSitienePersonaje(WWW www){
		yield return www;
		if(www.error == null){

			if (int.Parse(www.text) ==  1) {
				tienePersonaje = 2;
			} else {
				tienePersonaje = 1;
				Debug.Log (www.text);
			}

		}else{
			Debug.Log(www.error);
		}
	}

	public IEnumerator crearPersonajeUser(WWW www){
		yield return www;
		if(www.error == null){
			
			if (int.Parse(www.text) ==  1) {
				tienePersonaje = 2;
			} else {
				tienePersonaje = 1;
				Debug.Log (www.text);
			}
			
		}else{
			Debug.Log(www.error);
		}
	}

	public IEnumerator consultarPersonaje(WWW www){
		yield return www;
		if(www.error == null){
			Debug.Log(www.text);
			caracteristicas = www.text;
		}else{
			Debug.Log(www.error);
		}
	}

	private IEnumerator crearPersonaje(WWW www){
		yield return www;
		if(www.error == null){
			Debug.Log(www.text);
			correcto = true;
		}else{
			Debug.Log(www.error);
		}
	}

	private bool validarPersonaje()
	{
		if (General.idPersonaje == 0)
			return false;
		else
			return true;
	}
}