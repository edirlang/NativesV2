using UnityEngine;
using System.Collections;

public class Caida : MonoBehaviour {

	public float velocidad = 1;
	public GameObject cofre;
	private Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		cofre = GameObject.Find ("Cofre");
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.Find ("Cofre"))
		{
			cofre.transform.position = new Vector3(cofre.transform.position.x,cofre.transform.position.y - velocidad,cofre.transform.position.z); 
			cofre.transform.Rotate(Vector3.up, - 100 * Time.deltaTime);

			if(cofre.transform.rotation.x != 0)
			{
				cofre.transform.rotation = Quaternion.Euler(Vector3.zero);
			}
		}

		if (Input.GetMouseButton(0)) {
			
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                            
			
			//Hace tranza una linea desde el lugar la camara hasta la poscion del raton.
			if (Physics.Raycast(ray, out hit)){       
				if (hit.rigidbody != null){                                                                       
					//Si el objeto con el que colisiona es de tipo rigidbody
					movimiento.posicion = hit.point;                                                           
					//Cambia la variable posicion de la clase Movimiento Continuo
				}
			}
		}
	}


	void OnCollisionEnter(Collision collision) {
		GameObject objeto =  collision.gameObject;
		if(objeto.name == "Cofre")
		{
			velocidad = 0;
			Destroy(cofre.GetComponent<Rigidbody>());
			cofre.transform.position = new Vector3(cofre.transform.position.x,cofre.transform.position.y + 1,cofre.transform.position.z); 
			cofre.transform.Rotate(Vector3.zero);
		}
		if (objeto.name == "AvePrefab(Clone)")
		{
			CharacterController controller = collision.gameObject.GetComponent<CharacterController>();
			moveDirection = new Vector3(0,2,0);
			moveDirection = collision.gameObject.transform.TransformDirection(moveDirection);
			moveDirection *= 10f;
			
			controller.Move(moveDirection * Time.deltaTime);
		}
	}

	void OnCollisionStay(Collision collision) {
		GameObject objeto =  collision.gameObject;
		if (objeto.name == "AvePrefab(Clone)")
		{
			CharacterController controller = objeto.GetComponent<CharacterController>();
			moveDirection = new Vector3(0,2,0);
			moveDirection = objeto.transform.TransformDirection(moveDirection);
			moveDirection *= 10f;
			
			controller.Move(moveDirection * Time.deltaTime);
		}
	}

	void OnCollisionExit(Collision collision) {
		GameObject objeto =  collision.gameObject;
		if (objeto.name == "AvePrefab(Clone)")
		{
			objeto.transform.rotation.Set(0,0,0,0);
		}
	}
}
