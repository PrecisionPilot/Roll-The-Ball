using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour {
    [SerializeField]
    Text FPS;
    [SerializeField]
    Text Resolution;
    [SerializeField]
    Text GPU;
    [SerializeField]
    Text CPU;

    float fps;

    // Use this for initialization
    void Start()
    {
        GPU.text = "GPU: " + SystemInfo.graphicsDeviceName;

        //float processorFrequency = SystemInfo.processorFrequency;
        CPU.text = "CPU: " + SystemInfo.processorCount + "x " + SystemInfo.processorType/* + " @ " + processorFrequency / 1000 + "GHz"*/;

        StartCoroutine(DelayFrame());
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        if (fps > 30) { FPS.color = Color.green; } else { FPS.color = Color.yellow; }
        FPS.text = fps.ToString();

        Resolution.text = "Resolution: " + Screen.width + " × " + Screen.height;
    }
    IEnumerator Delay()
    {
        yield return null;
        Destroy(GPU.GetComponent<ContentSizeFitter>());
        Destroy(CPU.GetComponent<ContentSizeFitter>());
        GPU.transform.localPosition = new Vector3(GPU.rectTransform.rect.width / 2, GPU.transform.localPosition.y);
        CPU.transform.localPosition = new Vector3(CPU.rectTransform.rect.width / 2, CPU.transform.localPosition.y);
    }
    IEnumerator DelayFrame()
    {
        yield return new WaitForSeconds(0.5f);
        fps = Mathf.Round(1 / Time.deltaTime);
        StartCoroutine(DelayFrame());
    }
}
