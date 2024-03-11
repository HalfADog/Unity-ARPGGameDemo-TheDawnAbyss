using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoSingleton<MiniMapManager>
{
    private Transform playerTransform;
	private void Awake()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerController>().transform;
	}

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, 100, playerTransform.position.z);
        }
    }
}
