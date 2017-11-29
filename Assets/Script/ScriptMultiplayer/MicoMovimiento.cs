using UnityEngine;
using System.Collections;

public class MicoMovimiento : MonoBehaviour {
	public float speed, tiempo = 1;
	public GameObject sensorDelantero, obstaculo;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direcion = obstaculo.transform.position - gameObject.transform.position;
		Ray rayMico = new Ray (gameObject.transform.position, sensorDelantero.transform.position);


		RaycastHit[] hitMico = Physics.RaycastAll(rayMico);

		if (hitMico.Length > 0) {
			foreach(RaycastHit hit in hitMico)
			{
				obstaculo = hit.transform.gameObject;
				direcion = obstaculo.transform.position - gameObject.transform.position;

				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direcion),speed * Time.deltaTime);

				transform.position += transform.forward * speed *Time.deltaTime;
				Debug.DrawRay (rayMico.origin, direcion * 1.0f, Color.red);
				Debug.Log(obstaculo.name);
				break;	
			}
		}

	}
}
