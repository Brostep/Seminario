using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPattern : MonoBehaviour
{
	public BossController boss;
	void EndThirdPersonBulletPattern()
	{
		boss.endPattern = true;
	}
}
