using UnityEngine;
using System.Collections;

public class ChiaPerseguir : MonoBehaviour {

	GameObject target;
	Light luz;
	public bool llegoChia;
	public string mensajeChia;
	private Animator animator;
	// Use this for initialization
	void Start () {
		target = GameObject.Find (Network.player.ipAddress);
				if (GameObject.Find ("Luz")) {
						luz = GameObject.Find ("Luz").GetComponent<Light> ();
				}
		llegoChia = false;
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Vector3.Distance(target.transform.position,transform.position) > 2){
			Camera.main.GetComponent<AudioSource>().enabled = true;

			Quaternion rotacion = Quaternion.LookRotation (target.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, 6.0f * Time.deltaTime);
			transform.Translate(0,0,12.0f * Time.deltaTime);
		}else{
			Destroy(gameObject,General.timepoChia);
			animator.SetBool("hablar", true);
			llegoChia = true;
			Camera.main.GetComponent<AudioSource>().enabled = false;
			General.timepo -= Time.deltaTime;
		}
	}

	void OnGUI(){
				GUIStyle style = new GUIStyle ();
				style = GUI.skin.GetStyle ("Box");
				style.alignment = TextAnchor.MiddleCenter;
				style.fontSize = (int)(20.0f );
				if(llegoChia)
				{
						if (int.Parse (General.misionActual [0]) >= 2) {
								if(luz != null)
									luz.intensity = 1.5f;
						} else {
								if(luz != null)
									luz.intensity = 0.5f;
						}
						GUI.Box(new Rect(Screen.width/10, 3*Screen.height/4, 2*(Screen.width/3),Screen.height/4),mensajeChia);
						MoverMouse.movimiento = true;
						MoverMouse.cambioCamara = false;
				}else{
						if(luz != null)
								luz.intensity = 0;
				}
		}
}
