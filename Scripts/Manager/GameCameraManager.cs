using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraManager
{
    private Dictionary<string, GameObject> virtualCameras = new Dictionary<string, GameObject>();
	public void UpdateCameras()
    {
        virtualCameras.Clear();
        foreach (var item in Object.FindObjectsOfType<CinemachineVirtualCameraBase>(true))
        {
            virtualCameras[item.gameObject.name] = item.gameObject;
        }
    }
    public GameObject GetCamera(string name) 
    {
        if (virtualCameras.Count == 0)
        {
            UpdateCameras();
        }
        
        return virtualCameras[name];
    }

    public void EnableCamera(string name,bool inputProvider = false)
    {
        GameObject cam = GetCamera(name);
        if (inputProvider) 
        {
            cam.GetComponent<CinemachineInputProvider>().enabled = true;
            return;
        }
        GetCamera(name).SetActive(true);
    }
    public void DisableCamera(string name, bool inputProvider = false) 
    {
		GameObject cam = GetCamera(name);
		if (inputProvider)
		{
			cam.GetComponent<CinemachineInputProvider>().enabled = false;
			return;
		}
		GetCamera(name).SetActive(false);
    }

    public void CameraShake(CinemachineImpulseSource impulseSource,float force)
    {
        impulseSource.GenerateImpulseWithForce(force);
    }
}
