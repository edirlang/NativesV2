using UnityEngine;
using System.Collections;

public class AveMovimiento : MonoBehaviour {
	
	public float speed = 10.0f;
	private Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 89.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
		CharacterController controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(1, 0, 0);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		controller.Move(moveDirection * Time.deltaTime);

	}
}
