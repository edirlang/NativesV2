using UnityEngine;
using System.Collections;

public class barro : MonoBehaviour {
	public Texture contenidobarro;
	public bool tomabarro=false, instanciarVasija=false, cargarMaleta = false, actualizar = false;
	public float tiempo = 5, tiempoAnimacion=0;
	public GameObject vasija;
	public AnimationClip recojer;
	Animator playerAnimator;
	GameObject player;
	// Use this for initialization
	void Start () {
		if(General.paso_mision == 4 && General.misionActual[0] == "1" ){
			cargarMaleta = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(instanciarVasija){
			GameObject vasijaIns = (GameObject) Instantiate(vasija, player.transform.position,transform.rotation);
			vasijaIns.transform.parent = player.transform;
			vasijaIns.transform.Translate(0.1532699f,-0.3859406f,0f);
			vasijaIns.transform.rotation = new Quaternion();
			vasijaIns.transform.Rotate(270f,0f,0f);
			instanciarVasija = false;
		}

		tiempo -= Time.deltaTime;
		tiempoAnimacion -= Time.deltaTime;
		if(tiempo < 0){
			if(GameObject.Find("Vasija(Clone)"))
			{
				playerAnimator.SetBool("recojer",false);
				Destroy(GameObject.Find("Vasija(Clone)"));
			}

			if (tiempoAnimacion < 0 && tomabarro) {
				MoverMouse.movimiento = true;	
				MoverMouse.cambioCamara = false;
				if(actualizar && General.paso_mision == 6 && General.misionActual[0] == "1"){
					Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
					mision.procesoMision1(General.paso_mision);
					actualizar = false;
				}
			}
			tomabarro =false;
		}

		if(cargarMaleta){
			Maleta maleta = Camera.main.gameObject.GetComponent<Maleta>();
			maleta.agregarTextura(contenidobarro);
			cargarMaleta = false;
		}
	}

	void OnGUI(){
				if (tomabarro && Camera.main.GetComponent<NetworkView>().isMine) {
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.MiddleCenter;
			style = GUI.skin.GetStyle ("label");
			style.fontSize = (int)(20.0f );
			GUI.Label(new Rect(0,7*(Screen.height/8),Screen.width,Screen.height/16),"Haz recogido barro (arcilla)");
		}
	}
	public void OnTriggerEnter(Collider colision){
				if (colision.gameObject.name == Network.player.ipAddress) {
			player = colision.gameObject;
			playerAnimator = colision.gameObject.GetComponent<Animator>();
			playerAnimator.SetBool("recojer",true);

			tiempoAnimacion = recojer.length;
			tiempo = tiempoAnimacion + 1.5f;
			instanciarVasija = true;
			Maleta maleta = Camera.main.gameObject.GetComponent<Maleta>();
			maleta.agregarTextura(contenidobarro);

			tomabarro = true;
			MoverMouse.movimiento = false;
			MoverMouse.cambioCamara = true;
			actualizar = true;
		}
	}
}
