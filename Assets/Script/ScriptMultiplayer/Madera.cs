using UnityEngine;
using System.Collections;

public class Madera : MonoBehaviour {
	public Texture madera;
	public bool tomaMadera=false, actualizar = false;
	public float tiempo = 0;
	public AnimationClip recojerAnimacion;
	Animator playerAnimator;
	float tiempoAnimacion;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		tiempo -= Time.deltaTime;
		tiempoAnimacion -= Time.deltaTime;
		if(tiempo < 0){
			tomaMadera =false;
		}
		if (tiempoAnimacion < 0) {
			if(playerAnimator != null){
				playerAnimator.SetBool("recojer",false);
				transform.position = new Vector3(-10,-10,-10);
				MoverMouse.movimiento = true;
				MoverMouse.cambioCamara = false;
				if(actualizar && General.paso_mision == 3 && General.misionActual[0] == "1"){
					Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
					mision.procesoMision1(General.paso_mision);
					gameObject.GetComponent<MeshRenderer> ().enabled = false;
					gameObject.GetComponent<BoxCollider> ().enabled = false;
					actualizar = false;
				}
			}
		}

		if(General.misionActual[0] == "1" && General.paso_mision == 3){

		} else{
			if(General.paso_mision > 3 && General.misionActual[0] == "1"){
				Maleta maleta = Camera.main.gameObject.GetComponent<Maleta>();
				maleta.agregarTextura(madera);
				Destroy(gameObject);
			}
			if(playerAnimator != null){
				playerAnimator.SetBool("recojer",false);
			}

			
		}
	}

	void OnGUI(){
				if (tomaMadera && Camera.main.GetComponent<NetworkView>().isMine) {
			GUIStyle style = new GUIStyle ();
			style.alignment = TextAnchor.MiddleCenter;
			style = GUI.skin.GetStyle ("label");
			style.fontSize = (int)(20.0f );
			GUI.Label(new Rect(0,7*(Screen.height/8),Screen.width,Screen.height/16),"Haz recogido 1 trozo de madera");		
		}
	}

	public void OnTriggerEnter(Collider colision){
				if (colision.gameObject.name == Network.player.ipAddress) {
			actualizar = true;

			playerAnimator = colision.gameObject.GetComponent<Animator>();
			playerAnimator.SetBool("recojer",true);

			Maleta maleta = Camera.main.gameObject.GetComponent<Maleta>();
			maleta.agregarTextura(madera);

			MoverMouse.movimiento = false;
			MoverMouse.cambioCamara = true;

			tomaMadera = true;
			tiempoAnimacion = recojerAnimacion.length;
			Destroy(gameObject,5);		
			tiempo = 5;
		}
	}
}
