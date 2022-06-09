using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoController : MonoBehaviour {
    
    public Controller MainScript;
    public GameObject VideoCamera;
    
    int T1int;
    int T2int;
    int T3int;
    int T4int;
    int T4int2;
    int T5int;
    int T5int2;
    int T5int3;
    int T5int4;

    // Use this for initialization
    void Start () {
        StartCoroutine("Video");
	}
	
	// Update is called once per frame
	void Update () {
        if (T2int == 1)
        {
            VideoCamera.transform.Translate(new Vector3(0, -10, 10) * Time.deltaTime);

            if (transform.position.y <= 1)
            {
                transform.position = new Vector3(0, 1, -1);
                T3int = 1;
                T2int = 0;
            }
        }
        
        if (T3int == 1)
        {
            VideoCamera.transform.Translate(new Vector3(0, 0, -5) * Time.deltaTime);

            if (transform.position.z <= -9)
            {
                transform.position = new Vector3(-0.01f, 1, -9);
                T4int = 1;
                T3int = 0;
            }
        }

        if (T4int == 1)
        {
            transform.Translate(new Vector3(-5, 0.25f, 0) * Time.deltaTime);

            if (transform.position.x > 0)
            {
                T4int2 = 1;
                T4int = 0;
            }
        }

        if (T4int2 == 1)
        {
            transform.Translate(new Vector3(-5, 0.5f, 0) * Time.deltaTime);

            if (transform.position.x < 0)
            {
                transform.position = new Vector3(0, transform.position.y, -8);
                T5int = 1;
                T4int2 = 0;
            }
        }

        if (T5int == 1)
        {
            VideoCamera.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);

            if (transform.position.y > 20)
            {
                T5int2 = 1;
                T5int = 0;
            }
        }

        if (T5int2 == 1)
        {
            transform.Translate(new Vector3(7.5f, 0, 0) * Time.deltaTime);

            if (transform.position.x < 0)
            {
                T5int3 = 1;
                T5int2 = 0;
            }
        }

        if (T5int3 == 1)
        {
            transform.Translate(new Vector3(2, 0, 0) * Time.deltaTime);
        }
    }
    void LateUpdate()
    { transform.LookAt(new Vector3(0, 0, 0)); }

    IEnumerator Video()
    {
        yield return new WaitForSeconds(10);
        T2int = 1;
        MainScript.firstPersonCamera.enabled = false;
        MainScript.overheadCamera.enabled = false;
        MainScript.VideoCamera.enabled = true;
    }

    public void Play()
    {
        StopCoroutine("Video");
        //T1int = 0;
        T2int = 0;
        T3int = 0;
        T4int = 0;
        T4int2 = 0;
        T5int = 0;
        T5int2 = 0;
        T5int3 = 0;
        VideoCamera.transform.position = new Vector3(0, 20, -20);
        transform.position = new Vector3(0, 20, -20);
    }
}
