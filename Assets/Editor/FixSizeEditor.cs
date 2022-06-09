using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(FixSize))]
public class FixSizeEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FixSize TargetScript = (FixSize)target;
        GameObject This = TargetScript.gameObject;
        RectTransform RT = This.GetComponent<RectTransform>();
        
        Vector3 anchoredPosition = new Vector3(Screen.width * This.GetComponent<RectTransform>().anchorMin.x, Screen.height * This.GetComponent<RectTransform>().anchorMin.y);
        Vector3 offset = This.transform.position - anchoredPosition;
        Vector2 screen = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.size;
        if (GUILayout.Button("Set Position"))
        {
            screen = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.size;
            float Size;
            if (!TargetScript.Height)
            { Size = screen.y / TargetScript.RectLength / RT.rect.width; }
            else
            { Size = screen.x / TargetScript.RectLength / RT.rect.height; }

            RT.localScale = new Vector3(Size, Size);
            RT.position = anchoredPosition + offset * RT.localScale.x;
        }
    }
}