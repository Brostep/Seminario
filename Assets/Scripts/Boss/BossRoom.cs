using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {

	public GameObject bridge;
	public List<GameObject> spawnersLocation;


	void OnTriggerEnter(Collider c)
	{
		// is the player? set camera promedy on and turn off normal movement
		if (c.gameObject.layer == 8)
		{
			var playerController = FindObjectOfType<PlayerController>();
			// top down normal controller apagado
			playerController.topDownCamera.GetComponent<TopDownCameraController>().enabled=false;
			// habilito top down promedy
			playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled=true;
			// booleano que arreglaba el error de acercarte la camara cuando cambiabas..
			// deberiamos de sacarlo ya que forzamos al palyer fijate nose
			playerController.inBossFight = true;
			playerController.promedyTarget = true;
			// bools para detectar el cambio de camara en player controller.. llama a camara change 
			// cambia el field of view y todo la wea.. de movimientos
			playerController.cameraChange = true;
			PlayerController.inTopDown = !PlayerController.inTopDown;

			// start shooting
			FindObjectOfType<BossController>().BossIntro();

			// rompe el bridge y desavilita el trigger
			bridge.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
		}
	}


}
