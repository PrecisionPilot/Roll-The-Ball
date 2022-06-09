using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(EditorModifier))]
public class EditorExample : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorModifier TargetScript = (EditorModifier)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Revert"))
        {
            GameObject[] GO = GameObject.FindGameObjectsWithTag(TargetScript.Tag);
            if (GO.Length > 0)
            {
                foreach (GameObject r in GO) { PrefabUtility.RevertPrefabInstance(r); }
            }
            else { Debug.Log("That tag doesn't exist"); }
        }
        if (GUILayout.Button("Rename"))
        {
            GameObject[] GO = GameObject.FindGameObjectsWithTag(TargetScript.Tag);
            if (GO.Length > 0)
            {
                string Name = GO[0].name;
                for (int i = 0; i < GO.Length - 1; i++)
                {
                    GO[i + 1].name = Name + (i + 1);
                }
            }
            else { Debug.Log("That tag doesn't exist"); }
        }
        GUILayout.EndHorizontal();
    }
}