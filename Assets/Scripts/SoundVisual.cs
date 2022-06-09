using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;

public class SoundVisual : MonoBehaviour
{
    public enum SoundRespondType { rms, db, pitch }
    const int SAMPLE_SIZE = 1024;
    private static float rmsValue;
    private static float dbValue;
    private static float pitchValue;
    public static float PitchValue
    {
        get
        {
            return pitchValue;
        }
    }

    public static float DbValue
    {
        get
        {
            return dbValue;
        }
    }

    public static float RmsValue
    {
        get
        {
            return rmsValue;
        }
    }

    public float maxVisualScale = 25.0f;
    public float visualModifier = 50.0f;
    public float smoothSpeed = 10.0f;
    public float keepPercentage = 0.5f;
    [HideInInspector]
    public Transform[] visualList;
    float[] visualScale;
    public int amnVisual = 10;
    
    public AudioSource source;

    float[] samles;
    float[] spectrum;
    float sampleRate;
    
    public bool isBackground = true;
    [SerializeField]
    GameObject backgroundCheck;

    public Camera mainCamera;
    private Color orgBackgroundColor;
    public Color OrgBackgroundColor
    {
        get
        {
            return orgBackgroundColor;
        }

        set
        {
            orgBackgroundColor = value;
        }
    }

    public float value1;
    public float value2;

    [SerializeField]
    Arduino arduino1;

    void Start()
    {
        samles = new float[1024];
        spectrum = new float[1024];
        sampleRate = AudioSettings.outputSampleRate;

        SpawnLine();
    }

    void Update()
    {
        AnalyzeSound();
        UpdateVisual();

        if (isBackground)
        { value1 = Mathf.Round(visualScale[0] * 10) * 2; }
        else { value1 = Mathf.Round(RmsValue * 750); }

        if (value1 < 0) { value1 = 0; }
        if (value1 > 255) { value1 = 255; }
        /*value2 = Mathf.Round(RmsValue * 750);
        if (value2 < 0) { value2 = 0; }
        if (value2 > 255) { value2 = 255; }*/

        if (source.isPlaying)
        {
            if(value1 < 10)
                arduino1.SendLine("  " + value1 + "255");
            else if (value1 < 100)
                arduino1.SendLine(" " + value1 + "255");
            else
                arduino1.SendLine(value1 + "255");
        }
        mainCamera.backgroundColor = OrgBackgroundColor * rmsValue * 2;
    }
    void SpawnLine()
    {
        GameObject audioSpectrum = GameObject.FindGameObjectWithTag("AudioSpectrum");

        visualScale = new float[amnVisual];
        visualList = new Transform[amnVisual];

        for (int i = 0; i < amnVisual; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<Image>();
            go.transform.SetParent(audioSpectrum.transform);
            go.name = i.ToString();
            visualList[i] = go.transform;
            go.GetComponent<Image>().color = new Color(1, 1, 1, 0.30f);
            go.GetComponent<Image>().raycastTarget = false;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / (float)amnVisual, 10);
            visualList[i].position = new Vector3(Screen.width / (float)amnVisual * i + (Screen.width / (float)amnVisual) / 2, Screen.height / 4);
        }
    }
    void UpdateVisual()
    {
        int visualIndex = 0;
        int spectrumIndex = 0;
        float averageSize = (SAMPLE_SIZE * keepPercentage) / amnVisual;

        while (visualIndex < amnVisual)
        {
            int i = 0;
            float sum = 0;

            while (i < averageSize)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
                i++;
            }

            float scaleY = sum / averageSize * visualModifier;
            visualScale[visualIndex] -= Time.deltaTime * smoothSpeed;
            if (visualScale[visualIndex] < scaleY)
                visualScale[visualIndex] = scaleY;

            if (visualScale[visualIndex] > maxVisualScale)
                visualScale[visualIndex] = maxVisualScale;

            visualList[visualIndex].localScale = Vector3.one + Vector3.up * visualScale[visualIndex];

            visualIndex++;
        }
    }

    void AnalyzeSound()
    {
        source.GetOutputData(samles, 0);

        // Get RMS
        float sum = 0;
        for (int i = 0; i < SAMPLE_SIZE; i++)
        {
            sum += samles[i] * samles[i];
        }
        rmsValue = Mathf.Sqrt(sum / SAMPLE_SIZE);

        // Get the DB value
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);

        // Get Sound Spectrum
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    public void SetBackgroundMusic()
    {
        isBackground = !isBackground;
        backgroundCheck.SetActive(isBackground);
    }
}