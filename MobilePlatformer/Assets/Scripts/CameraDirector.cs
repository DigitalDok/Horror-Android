using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour {

    public static CameraDirector TheCameraDirector;
    private GameObject MasterCamera;
    
    public GameObject target;
    public float CameraSpeed;

    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    

    public float OriginalSize;
    public float ZoomSize;
    private float ZPOS= -9.08f;

    void Start ()
    {
        TheCameraDirector = this;
        MasterCamera = gameObject;
        OriginalSize = MasterCamera.GetComponent<Camera>().orthographicSize;
        
	}

    void FixedUpdate()
    {
        if (!target)
            target = GameObject.Find("MainChar");

       
            Vector2 pos = Vector2.Lerp((Vector2)transform.position, (Vector2)target.transform.position, Time.fixedDeltaTime * CameraSpeed);
            transform.localPosition = new Vector3(pos.x, pos.y, ZPOS);
        
            float NewSize = Mathf.Lerp(MasterCamera.GetComponent<Camera>().orthographicSize, OriginalSize, Time.fixedDeltaTime*2);
            MasterCamera.GetComponent<Camera>().orthographicSize = NewSize;
        
    }

    private void LateUpdate()
    {
        transform.position =
            new Vector3(Mathf.Clamp(transform.position.x, MinX, MaxX),
            Mathf.Clamp(transform.position.y, MinY, MaxY),
            ZPOS);
        
    }

}

