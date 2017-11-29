using UnityEngine;
using System.Collections;

public class MensajesCuriosos : MonoBehaviour {
		string[] mensajes;
		string mensaje;
		public int numeroMensaje;
		bool ver_mensaje;
		float tiempo;
	// Use this for initialization
	void Start () {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
				ver_mensaje = false;
				mensajes = new string[15];
				mensajes[0] = "Las hojas de palma boba son palmas los cuales crecen en zonas frías,\n son originarias de la región del Sumapaz ";
				mensajes[1] = "El barro es usado en nuestro pueblo como cemento \n para unir las distintas partes de la casa ";
				mensajes[2] = "Esta iglesia la han traído los españoles, donde \n nos enseñaran a practicar su religión católica.";
				mensajes[3] = "Sabias que el pago en monedas de oro como único \n intercambio llego con los españoles.";
				mensajes[4] = "Iglesia reconstruida en 1776, \n mostrando la construcción de la nueva ciudad.";
				mensajes[5] = "La evolución llega también a las viviendas \n hecha de madera, piedra";
				mensajes[6] = "Primera casona creada en 1850, traída por la cultura española. \n Fue usadas como un Casino por diversión del pueblo.";
				mensajes[7] = "Te cuento, esta iglesia fue reconstruida en 1776, \n mostrando la construcción de la nueva ciudad. Desde aquí tu comunidad y tu empiezan a tomar la religión católica.";
				mensajes[8] = "Esta casona fue usada en 1875, para firmar \n varios artículos presidenciales.";
				mensajes[9] = "El café fue uno de los principales productos que se producen en la región del Sumapaz.";
				mensajes[10] = "Con la llegada de los españoles, nuestros indígenas fueron forzados a salir \n de su zona y llevados hacia Altagracia de Sumapaz. (Ahora conocido como Pasca).";
	}
	
	// Update is called once per frame
	void Update () {
				tiempo -= Time.deltaTime;
				if (tiempo < 0) {
						ver_mensaje = false;
				}

				if(General.paso_mision <= 10 && General.misionActual[0] == "3" && numeroMensaje == 7){
						Destroy (gameObject);
				}
	}

		void OnGUI(){
				if (ver_mensaje && Camera.main.GetComponent<NetworkView>().isMine) {
						GUIStyle style = new GUIStyle ();
						style.alignment = TextAnchor.MiddleCenter;
						style = GUI.skin.GetStyle ("Box");
						style.fontSize = (int)(20.0f);

						GUI.Box (new Rect (0, 2 * Screen.height / 4, Screen.width, Screen.height / 4), mensajes[numeroMensaje-1]);

				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.gameObject.name == Network.player.ipAddress) {
						ver_mensaje = true;
						tiempo = 10;
						if (numeroMensaje == 11) {
								Destroy (gameObject, tiempo);
						}
				}
		}
}
