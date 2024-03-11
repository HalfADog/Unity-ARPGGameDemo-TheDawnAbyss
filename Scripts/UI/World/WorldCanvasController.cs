using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasController : MonoBehaviour
{
    private Camera mainCamera;
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = mainCamera.transform.forward;
        float dis = Vector3.Distance(transform.position, mainCamera.transform.position);
        canvasGroup.alpha =  Mathf.Clamp01((15 - dis) / 10);
    }
}
