using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Choza : MonoBehaviour {
	public bool cosntrullendo=false, activarBoton = false;
	public float tiempo = 0;
	public AnimationClip crearChoza;
	public GameObject ubicar_camara, chozaFinal;
	private GameObject player;
	bool crearChozaMulti = false;
	Transform posicionInstanciar, camaraOriginal;
	Animator playerAnimator;
	float tiempoAnimacion;

	// Use this for initialization
	void Start () {
		posicionInstanciar = transform;
			
	}
	
	// Update is called once per frame
	void Update () {
		if(cosntrullendo){
			tiempo -= Time.deltaTime;

			activarBoton = false;

			GameObject chozaLevel;
			if(tiempo > 28 && tiempo < 39)
			{
				chozaLevel = GameObject.Find("choza1");
				chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 1, posicionInstanciar.position.y, posicionInstanciar.position.z);
			}else if (tiempo > 20 && tiempo < 21){
				Destroy(GameObject.Find("choza1"));
				chozaLevel = GameObject.Find("choza2");
				chozaLevel.transform.position = chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 4, posicionInstanciar.position.y, posicionInstanciar.position.z);

			}else if(tiempo > 12 && tiempo < 13){
				if(crearChozaMulti){
					Destroy(GameObject.Find("choza2"));

					chozaLevel = GameObject.Find("choza3");
					chozaLevel.transform.localScale = new Vector3(4f,4f,4f);
					chozaLevel.transform.position = new Vector3(posicionInstanciar.position.x  - 4, posicionInstanciar.position.y, posicionInstanciar.position.z);

					NetworkView nw = Camera.main.GetComponent<NetworkView>();

					nw.RPC("crearChozaMultiplayer",RPCMode.OthersBuffered, player.name, posicionInstanciar.position, 2);
					playerAnimator.SetBool("construir",false);

					MoverMouse.cambioCamara = false;
					MoverMouse.movimiento = true;

					crearChozaMulti = false;
				}
			}
		}

		if(tiempo < 0){
			if(cosntrullendo){
				Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
				mision.procesoMision1(General.paso_mision);
				Camera.main.transform.rotation = camaraOriginal.rotation;
				MoverMouse.movimiento = true;
			}
			cosntrullendo = false;
		}
	}

	void OnGUI(){
		if(activarBoton){
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/16, Screen.height/2 - Screen.height/32,Screen.width/8,Screen.height/16),"Construir")){
				cosntrullendo = true;
				crearChozaMulti = true;
				tiempo = 30;
				camaraOriginal = Camera.main.transform;
				MoverMouse.cambioCamara = true;
				MoverMouse.movimiento = false;
				playerAnimator.SetBool("construir",true);
				posicionInstanciar = player.transform;

				Camera.main.transform.localPosition = new Vector3(1.71456f,3.25226f,1.54568f);
				Camera.main.transform.rotation = new Quaternion();
				Camera.main.transform.Rotate(50.517f,265.809f,14.878f);


			}
		}
		if(crearChozaMulti){
			GUI.Label(new Rect(Screen.width/2 - Screen.width/16, Screen.height/2 - Screen.height/32,Screen.width/3,Screen.height/16),"Construyendo...");
		}
	}

	public void OnTriggerEnter(Collider colision){
				if (colision.name == Network.player.ipAddress) {
			player = colision.gameObject;
			playerAnimator = colision.gameObject.GetComponent<Animator>();
			
			if(General.paso_mision == 7 && General.misionActual[0] == "1"){
				activarBoton = true;
			}
		}
	}

	public void OnTriggerExit(Collider colision){
				if (colision.name == Network.player.ipAddress) {
			if(General.paso_mision == 7 && General.misionActual[0] == "1"){
				player = colision.gameObject;
				activarBoton = false;
			}
		}
	}
}