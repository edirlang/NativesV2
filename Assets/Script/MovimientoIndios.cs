using UnityEngine;
using System.Collections;

public class MovimientoIndios : MonoBehaviour {
		public Transform[] points;
		public int destPoint = 0;
		private NavMeshAgent agent;
		float tiempo=-1;
		public int numeroPuntos;
		Animator animator;

		void Start () {
				tiempo = 0;
				agent = GetComponent<NavMeshAgent>();
				animator = GetComponent<Animator> ();
				numeroPuntos = 1;
				// Disabling auto-braking allows for continuous movement
				// between points (ie, the agent doesn't slow down as it
				// approaches a destination point).
				agent.autoBraking = false;

				GotoNextPoint();
		}


		void GotoNextPoint() {
				// Returns if no points have been set up
				if (points.Length == 0)
						return;

				// Set the agent to go to the currently selected destination.
				agent.destination = points[destPoint].position;

				// Choose the next point in the array as the destination,
				// cycling to the start if necessary.

				if (destPoint >= (points.Length-1)) {
						numeroPuntos = -1;
				}

				if(destPoint <= 0){
						numeroPuntos = 1;
				}

				destPoint = (destPoint + numeroPuntos);
		}


		void Update () {
				// Choose the next destination point when the agent gets
				// close to the current one.
				if (agent.remainingDistance < 0.5f) {
						tiempo -= Time.deltaTime;
						animator.SetFloat ("speed", 0.0f);
						agent.speed = 0f;
						if (tiempo < 0) {
								GotoNextPoint ();
								agent.speed = 3f;
								tiempo = 10;
						}
					
				} else {
						animator.SetFloat ("speed", 1.0f);
						agent.speed = 3f;
				}
		}
}
