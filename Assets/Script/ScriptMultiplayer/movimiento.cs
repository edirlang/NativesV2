using UnityEngine;
using System.Collections;

public class movimiento : MonoBehaviour {

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	private GameObject camara;
	private NetworkView nw;
	private Animator animator;
	public static Vector3 posicion = General.posicionIncial;

	public Vector3 posicionInicial;  

	void Start()
	{
				if (Application.isMobilePlatform) {
						this.enabled = false;
				}
		nw = GetComponent<NetworkView> ();

	}
	
	void Update() {
		animator = GetComponent<Animator> ();
		nw = GetComponent<NetworkView> ();
		if (nw.isMine )
		{
						
			controlMovimiento ();
			//moverClic();
		}
	}
	
	void controlMovimiento()
	{
		GameObject player = GameObject.Find (Network.player.ipAddress);
		CharacterController controller = player.GetComponent<CharacterController>();

				if (!MoverMouse.movimiento) {
						animator.SetFloat ("speed", 0.0f);
						return;
				}
					
			moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
			moveDirection = player.transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			
			transform.Rotate(0,Input.GetAxis("Horizontal")*speed,0);

			float f_hor = Input.GetAxis("Horizontal");
			float f_ver = Input.GetAxis("Vertical");
			animator.SetFloat("speed", f_hor*f_hor+f_ver*f_ver);
			nw.RPC("activarCaminar",RPCMode.Others, f_hor*f_hor+f_ver*f_ver);

			if (Input.GetButton("Jump"))
						moveDirection.y = jumpSpeed;
			
				moveDirection.y -= gravity * Time.deltaTime;
			controller.Move(moveDirection * Time.deltaTime);
	}
	
	[RPC]
	public void activarCaminar(float valor)
	{
		animator.SetFloat("speed", valor);
	}
}