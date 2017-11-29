using UnityEngine;
using System.Collections;

public class MovimientoAnimales : MonoBehaviour {
	GameObject personaje;
	private int estado = 0, numeroAtaques=0;
	public int moveDir = 1, speed=6, VelMov= 6;
	public float gravity = 100F, RotSpeed=10, DistEnemAtaque = 0.5f, DistEnem = 20f, contador=5, tiempo,tiempo2,moveSpeed = 6.0f;
	public bool ataca;
	public GameObject sensorDelantero;
	CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	Animator animator;
	bool Walk = false;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		tiempo = Random.Range(0, 1000);
		tiempo2 = Random.Range(0, 1000);
		numeroAtaques = 0;
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		switch (estado) {
				case 0:
						animalEsperando ();
						break;
				case 1:
						animator.SetInteger ("estate", 2);
						float DistanciaCont = Vector3.Distance (personaje.transform.position, transform.position);

						if (DistanciaCont <= DistEnem && DistanciaCont >= DistEnem / 2)
								speed = VelMov;
						else
								speed = VelMov + VelMov / 2;

						Quaternion rotacion = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (personaje.transform.position - transform.position), RotSpeed * Time.deltaTime);
						transform.rotation = new Quaternion (transform.rotation.x, rotacion.y, transform.rotation.z, rotacion.w);
						controller.Move (transform.forward * RotSpeed * Time.deltaTime);
						controller.Move (transform.up * -gravity * Time.deltaTime);
				
						moveDirection.y -= gravity * Time.deltaTime;
						if (DistanciaCont <= DistEnemAtaque) {
								estado = 2;
						}
				
						if (DistanciaCont > DistEnem * 1.5)
								estado = 0;
				
						break;
				case 2:
						if (ataca){
							DistanciaCont = Vector3.Distance (personaje.transform.position, transform.position);
							if (DistanciaCont > DistEnemAtaque) { 
									estado = 1; //Pasa al estado de perseguir.
									animator.SetInteger ("estate", 2);
							} else {
									estado = 3;//Pasa al estado de Atacar.
									contador = Time.time + 1;//(animation[AttackAnim.name].clip.length * 1.2);
									animator.SetInteger ("estate", 3);		 
							}
						}else{
							estado=1;
						}
					break;

				case 3:
					if(!ataca){
						animalAtaque();
					}else{
						estado = 2;
					}
				break;
		}
	}

	void animalEsperando()
	{	
		if(!Physics.Raycast(transform.position, transform.forward, 5))
		{
			moveDirection = new Vector3(0, 0, moveSpeed);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

			//transform.Translate(Vector3.forward * moveSpeed * Time.smoothDeltaTime);
			moveDirection.y -= gravity * Time.deltaTime;

		}
		else
		{
			if(Physics.Raycast(transform.position, - transform.right, 1))
			{
				moveDir = 1;
			}
			else if(Physics.Raycast(transform.position, transform.right, 1))
			{
				moveDir = -1;
			}
			moveDirection.y = 1f;
			transform.Rotate(Vector3.up, 45 * RotSpeed * Time.smoothDeltaTime * moveDir);
		}
		controller.Move(moveDirection * Time.deltaTime);
		tiempo -= Time.deltaTime * 1;
		tiempo2 -= Time.deltaTime * 1;
		
		if (tiempo <= 0){
			tiempo = Random.Range(0, 1000);
		}
		
		if (tiempo2 <= 0){
			tiempo2 = Random.Range(0, 1000);
		}

		if (tiempo > 500){
			Walk = true;
			moveSpeed = 0.2f;
		}
		if (tiempo < 300){
			Walk = false;
			moveSpeed = 0;
		}

		if(moveSpeed > 0)
		{
			animator.SetInteger("estate", 2);
		}else
		{
			animator.SetInteger("estate", 1);
		}
		if (tiempo2 < 75 && Walk == true){
			transform.Rotate(Vector3.up, 45 * moveSpeed * Time.smoothDeltaTime * moveDir);
			tiempo2 = Random.Range(0, 1000);
		}
		if (tiempo2 > 925 && Walk == true){
			transform.Rotate(Vector3.up, -45 * moveSpeed * Time.smoothDeltaTime * moveDir);
			tiempo2 = Random.Range(0, 1000);
		}

	}

	void animalAtaque(){

		if (Time.time > contador)
		{
			numeroAtaques++;
			Debug.Log("Ataque " + numeroAtaques);
			estado = 2;
			animator.SetInteger("estate", 2);
		}
		if(numeroAtaques >=5){
			personaje.transform.position = new Vector3(0,0,0);
			CharacterController controllerpersonaje = personaje.GetComponent<CharacterController>();
			controllerpersonaje.enabled = false;
			personaje.transform.position = GameObject.Find("PlayerJuego").transform.position;
			controllerpersonaje.enabled = true;

			General.salud--;
			StartCoroutine(General.actualizarUser());

			General.timepoChia = 10;
			GameObject chia = Instantiate (General.chia,  personaje.transform.position, personaje.transform.rotation) as GameObject;
			chia.GetComponent<ChiaPerseguir>().mensajeChia = "Haz perdido una vida \nTen cuidado la proxima vez";
			chia.transform.parent = personaje.transform;
			chia.transform.localPosition = new Vector3(0f, 5f,11f);
		}
	}

	void DoActivateTrigger(string playerName) {
		GameObject player = GameObject.Find (playerName);

		if(player.tag == "Player")
		{
			estado = 1;
			personaje = player;
			numeroAtaques=0;
		}
	}

	void DoDesactiveTrigger(){
		estado = 0;
	}

}
