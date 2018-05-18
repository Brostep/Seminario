using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject bridge;

    public List<GameObject> spawnersLocation;

    private void OnTriggerEnter(Collider c)
    {
        // is the player? set camera promedy on and turn off normal movement
        if (c.gameObject.layer == 8)
        {
            var playerController = FindObjectOfType<PlayerController>();
            // top down normal controller apagado
           // playerController.topDownCamera.GetComponent<TopDownCameraHolder>().enabled = false;
            // habilito top down promedy
           // playerController.topDownCamera.GetComponent<TopDownAverageTarget>().enabled = true;
            // booleano que arreglaba el error de acercarte la camara cuando cambiabas..
            // deberiamos de sacarlo ya que forzamos al palyer fijate nose
            playerController.inBossFight = true;
            playerController.promedyTarget = true;
            // bools para detectar el cambio de camara en player controller.. llama a camara change 
            // cambia el field of view y todo la wea.. de movimientos
          //  playerController.cameraChange = true;
          //  PlayerController.inTopDown = !PlayerController.inTopDown;

            FindObjectOfType<BossController>().BossIntro();

            bridge.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
