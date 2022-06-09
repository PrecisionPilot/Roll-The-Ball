using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {
    RectTransform rectTransform;
    RectTransform parentTransform;
    void Start () {
        rectTransform = GetComponent<RectTransform>();
        parentTransform = transform.parent.GetComponent<RectTransform>();
        transform.parent = transform.parent.parent;
        transform.SetSiblingIndex(0);
        StartCoroutine(Delay());
	}
    IEnumerator Delay()
    {
        yield return null; yield return null;
        rectTransform.anchorMin = parentTransform.anchorMin;
        rectTransform.anchorMax = parentTransform.anchorMax;
        rectTransform.localPosition = parentTransform.localPosition;
        rectTransform.sizeDelta = parentTransform.sizeDelta;
        Destroy(this);
    }
}
