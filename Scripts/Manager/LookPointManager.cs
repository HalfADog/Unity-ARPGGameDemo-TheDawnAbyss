using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointManager : MonoSingleton<LookPointManager>
{
    public Vector3 lookTargetPoint;
    public Transform lookTargetTransfrom;

	private void Update()
	{
		if (lookTargetTransfrom != null) 
		{
			lookTargetPoint = lookTargetTransfrom.position;
			lookTargetPoint.y = 0f;
			transform.position = lookTargetTransfrom.position;
		}
	}

	public void StartLooking(Transform transform) 
	{
		lookTargetTransfrom = transform;
	}

	public void StopLooking() 
	{
		lookTargetPoint = Vector3.zero;
		lookTargetTransfrom = null;
	}
}
