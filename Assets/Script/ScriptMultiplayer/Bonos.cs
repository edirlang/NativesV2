using UnityEngine;
using System.Collections;

public class Bonos : MonoBehaviour {
	float minutos=-1, segundos=0, tiempoMensaje=20;
	bool tieneBono = false;
	GameObject bono, cofre;
	int bonoJugador, opciones = 0;
	string bonoTexto;

	// Use this for initialization
	void Start () {
		bonoJugador = Random.Range (4,1);
		cofre = GameObject.Find("Cofre");
		string url2 = General.hosting+"TiempoMision";
		WWWForm form2 = new WWWForm();
		form2.AddField("username", General.username);
		WWW www2 = new WWW(url2, form2);
		StartCoroutine(consultarBono(www2));
		bono = GameObject.Find ("Bono");
		gameObject.transform.position = new Vector3 (Random.Range(10,2800),100,Random.Range(10,1800));
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.bono)
		{
			tieneBono = false;
			if(minutos > 0)
				opciones =5;
			else
				opciones = 1;
		}else{
			opciones = 0;
		}

		if(tieneBono){
			segundos -= Time.deltaTime;
			if(segundos < 0)
			{
				minutos --;
				segundos = 59;
			}
			gameObject.GetComponent<BoxCollider>().enabled = true;
			bono.SetActive(true);
			cofre.SetActive(true);
		}else{
			gameObject.GetComponent<BoxCollider>().enabled = false;
			bono.SetActive(false);
			cofre.SetActive(false);
		}
	}

	void OnGUI(){
		GUIStyle style = new GUIStyle ();
		if(minutos < 0)
		{
			opciones = 5;
			tieneBono = false;
		}
		if (tiempoMensaje < 0)
			opciones = 5;
		switch(opciones)
		{
			case 0:
				
				tieneBono = true;
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f );
				style.alignment = TextAnchor.UpperCenter;
				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(15.0f );
				style.alignment = TextAnchor.UpperCenter;
					
				GUI.Box(new Rect(Screen.width/2 - Screen.width/12,0,Screen.width/6,2 * (Screen.height/12)),"Bono disponible");
				GUI.Label (new Rect(Screen.width/2 - Screen.width/12, (Screen.height/16),Screen.width/6,Screen.height/12),minutos.ToString("f0") + ":" + segundos.ToString("f0"));
				style.alignment = TextAnchor.UpperLeft;
				break;
			case 1:
				tiempoMensaje -= Time.deltaTime;
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f );
				style.alignment = TextAnchor.UpperCenter;
				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(15.0f );
				style.alignment = TextAnchor.UpperCenter;
					
				GUI.Box(new Rect(Screen.width/2 - Screen.width/12,0,Screen.width/6,2 * (Screen.height/12)),"Has Obtenido");
				GUI.Label (new Rect(Screen.width/2 - Screen.width/12, (Screen.height/16),Screen.width/6,Screen.height/12),bonoTexto);
				
				style.alignment = TextAnchor.UpperLeft;
				break;
			}

	}
	public IEnumerator consultarBono(WWW www){
		yield return www;
		if(www.error == null){
			int tiempo = int.Parse(www.text);
			if(tiempo <= 30){
				minutos = 29 - tiempo;
			}else
			{
				tieneBono = false;
			}

		}else{
			Debug.Log(www.error);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.name == Network.player.ipAddress) {
			General.bono = false;
			switch(bonoJugador)
			{
			case 1:
				General.monedas = General.monedas + 10;
				bonoTexto = "10 Monedas";
				break;
			case 2:
				General.salud +=1;
				bonoTexto = "1 Vida";
				break;
			case 3:
				General.monedas += 5;
				bonoTexto = "5 Monedas";
				break;
			case 4: 
				General.salud +=1;
				General.monedas +=5;
				bonoTexto = "SuperBono /n 1 vida, 5 Monedas";
				break;
			}
			StartCoroutine (General.actualizarUser ());
			tieneBono = false;
			minutos = segundos = 0;
			opciones = 1;
		}
	}

	public IEnumerator actualizarUser(){
		string url = General.hosting + "logout";
		WWWForm form = new WWWForm ();
		form.AddField ("username", General.username);
		form.AddField("mision",General.misionActual[0] + "");
		form.AddField("pos_x", General.posicionIncial.x + "");
		form.AddField("pos_y", General.posicionIncial.y + "");
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
}
