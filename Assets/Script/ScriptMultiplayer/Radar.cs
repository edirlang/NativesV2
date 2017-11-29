using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour 
{

		// radar! by oPless from the original javascript by PsychicParrot, 
		// who in turn adapted it from a Blitz3d script found in the
		// public domain online somewhere ....
		//

		public Texture ciudades, trasportadores, puntoClave;
		public Texture radarBG;

		public Transform centerObject ;
		public float mapScale = 0.3f;
		public Vector2 mapCenter = new Vector2(50,50);
		public float maxDist = 200;

		void Update(){
				if(Network.peerType != NetworkPeerType.Disconnected){
						if (GameObject.Find (Network.player.ipAddress)) {
								centerObject = GameObject.Find (Network.player.ipAddress).transform;
						}else{
								centerObject = GameObject.Find ("PlayerJuego").transform;
						}
				}
		}
		void OnGUI() 
		{

				//	GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, Vector3(Screen.width / 600.0, Screen.height / 450.0, 1));

				// Draw player blip (centerObject)
				//		float bX=centerObject.transform.position.x * mapScale;
				//	    float bY=centerObject.transform.position.z * mapScale;
				if (centerObject == null) {
						return;
				}
				if (Network.peerType != NetworkPeerType.Disconnected && GameObject.Find(Network.player.ipAddress).GetComponent<NetworkView>().isMine) {
						
						GUI.DrawTexture(new Rect (mapCenter.x - Screen.height/8, mapCenter.y - Screen.height/8, Screen.height/4, Screen.height/4), radarBG);
						DrawBlipsFor ();
				}

		}

		private void DrawBlipsFor()
		{
				
				// Find all game objects with tag 
				GameObject[] gos = GameObject.FindGameObjectsWithTag("ciudad"); 

				// Iterate through them
				foreach (GameObject go in gos)  
				{ 
						drawBlip(go,ciudades);
				}

				GameObject[] got = GameObject.FindGameObjectsWithTag("trasnportador"); 

				// Iterate through them
				foreach (GameObject go in got)  
				{ 
						drawBlip(go,trasportadores);
				}

				GameObject[] goM = GameObject.FindGameObjectsWithTag("ObjetoMision"); 

				// Iterate through them
				foreach (GameObject go in goM)  
				{
						drawBlip(go,puntoClave);
				}
		}

		private void drawBlip(GameObject go,Texture aTexture)
		{
				Vector3 centerPos=centerObject.position;
				Vector3 extPos=go.transform.position;

				// first we need to get the distance of the enemy from the player
				float dist=Vector3.Distance(centerPos,extPos);

				float dx=centerPos.x-extPos.x; // how far to the side of the player is the enemy?
				float dz=centerPos.z-extPos.z; // how far in front or behind the player is the enemy?

				// what's the angle to turn to face the enemy - compensating for the player's turning?
				float deltay=Mathf.Atan2(dx,dz)*Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;

				// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
				float bX=dist*Mathf.Cos(deltay * Mathf.Deg2Rad);
				float bY=dist*Mathf.Sin(deltay * Mathf.Deg2Rad);

				bX=bX*mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
				bY=bY*mapScale; // scales down the y-coordinate by half so that the plot stays within our radar

				if(dist<= maxDist)
				{ 
						// this is the diameter of our largest radar circle
						float tamaño = Screen.height/48;
						if (tamaño < 5) {
								tamaño = 5;
						}
						GUI.DrawTexture(new Rect(mapCenter.x+bX,mapCenter.y+bY,tamaño,tamaño),aTexture);
				}

		}


}