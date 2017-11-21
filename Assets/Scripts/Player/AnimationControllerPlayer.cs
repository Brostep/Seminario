using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerPlayer : MonoBehaviour {
	//combo register
	//1 = Light Attack
	//2 = Heavy Attack
	//[HideInInspector]
	public List<int> actionRegister;
	List<int> actionsDone;
	List<int> lightAttacks;
	public float timeBeforeResetBasic = 2.5f;

	Animator anim;
	ThirdPersonController thirdPersonController;
	float timeBetweenAttacks;

	int currentAnim;
	int onRunnig;
	int onLightAttack1;
	int onLightAttack2;
	int onLightAttack3;
	int onHeavyAttack1;

	void Start () {
		actionRegister = new List<int>();
		actionsDone = new List<int>();
		lightAttacks = new List<int>();
		anim = GetComponent<Animator>();
		thirdPersonController = GetComponent<ThirdPersonController>();
		//hash actions
		onRunnig = Animator.StringToHash("Run");
		onLightAttack1 = Animator.StringToHash("OnAttack1");
		onLightAttack2 = Animator.StringToHash("OnAttack2");
		onLightAttack3 = Animator.StringToHash("OnAttack3");
		onHeavyAttack1 = Animator.StringToHash("OnHeavyAttack");
		lightAttacks.Add(onLightAttack1);
		lightAttacks.Add(onLightAttack2);
		lightAttacks.Add(onLightAttack3);
	}

	void Update ()
	{
		if (actionsDone.Count>0)
		{
			timeBetweenAttacks += Time.deltaTime;
			if (timeBetweenAttacks >= timeBeforeResetBasic)
			{
				anim.SetBool(actionsDone[0], false);
				actionsDone.RemoveAt(0);
				currentAnim = 0;
				timeBetweenAttacks = 0f;
			}
		}
	}
	public void EnterAnimationLightAttack()
	{	
		for (int i = 0; i < lightAttacks.Count; i++)
		{ 
			if (!anim.GetBool(lightAttacks[i]))
			{
				actionRegister.Add(lightAttacks[i]);
				currentAnim = i+1;
				break;		
			}
		}

		if (actionsDone.Count>0 && timeBetweenAttacks < timeBeforeResetBasic)
		{
			print("caca");
			for (int i = 0; i < currentAnim; i++)
			{
				print(i);
				anim.SetBool(actionsDone[i], true);
			}
		}

		if (!anim.GetBool(actionRegister[0]))
			anim.SetBool(actionRegister[0], true);

		timeBetweenAttacks = 0f;
	}
	/*void playNextAnim()
	{
		anim.SetBool(actionRegister.Dequeue(), false);
		anim.SetBool(actionRegister.Peek(), true);
	}*/
	void EndAttack()
	{
		timeBetweenAttacks = 0f;
		anim.SetBool(actionRegister[0], false);
		actionsDone.Add(actionRegister[0]);
		actionRegister.RemoveAt(0);
		thirdPersonController.movementSpeed = 8f;
	}
}
