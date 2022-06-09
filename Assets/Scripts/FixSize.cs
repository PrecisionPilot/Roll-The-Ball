using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSize : MonoBehaviour {
    public bool Height;
    RectTransform RT;
    public float RectLength;

    Vector3 offset;
    Vector3 anchoredPosition;
	void Start () {
        RT = GetComponent<RectTransform>();
        anchoredPosition = new Vector3(Screen.width * RT.anchorMin.x, Screen.height * RT.anchorMin.y);
        offset = transform.position - anchoredPosition;
	}
    void Update()
    {
        anchoredPosition = new Vector3(Screen.width * RT.anchorMin.x, Screen.height * RT.anchorMin.y);

        float Size;
        if (!Height)
        { Size = Screen.width / RectLength / RT.rect.width; }
        else
        { Size = Screen.height / RectLength / RT.rect.height; }
        
        RT.localScale = new Vector3(Size, Size);
        RT.position = anchoredPosition + offset * RT.localScale.x;
    }
}