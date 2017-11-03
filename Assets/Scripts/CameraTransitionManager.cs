using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CameraTransitionManager : MonoBehaviour {

	public GameObject thirdPersonCamera;
	public GameObject topDownBase;
	public GameObject cameraTransition;
	public PlayableDirector playableDirectorThirdPerson;
	bool inTransition;
	public GameObject topDownVCamera;
	public GameObject thirdPersonVCamera;
	PlayerController player;
	private void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("LButton") && !inTransition )
		{
			thirdPersonVCamera.transform.rotation = thirdPersonCamera.GetComponentInChildren<Camera>().transform.rotation;
			thirdPersonVCamera.transform.position = thirdPersonCamera.GetComponentInChildren<Camera>().transform.position;
			topDownVCamera.transform.position = topDownBase.transform.position;
			thirdPersonCamera.SetActive(false);
			cameraTransition.SetActive(true);
			cameraTransition.transform.rotation = thirdPersonVCamera.transform.rotation;
			cameraTransition.transform.position = thirdPersonVCamera.transform.position;
			topDownVCamera.SetActive(true);
			thirdPersonVCamera.SetActive(true);
			playableDirectorThirdPerson.Play();
			inTransition = true;
		}
		if (playableDirectorThirdPerson.time > playableDirectorThirdPerson.duration - 0.05f)
		{
			playableDirectorThirdPerson.Stop();
			thirdPersonCamera.transform.position = topDownVCamera.transform.position;
			thirdPersonCamera.transform.rotation = topDownVCamera.transform.rotation;
			PlayerController.inTopDown = !PlayerController.inTopDown;
			player.cameraChange = true;
			thirdPersonCamera.SetActive(true);
			cameraTransition.SetActive(false);
			inTransition = false;
		}
	}
}
