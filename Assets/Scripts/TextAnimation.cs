using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour {
    [SerializeField]
    PlayerController PlayerScript;
    public RectTransform Intro1;
    public RectTransform Intro2;
    float IntroPosX;
	void Update () {
        Intro1.position = new Vector2(-Intro1.rect.width / 2 + IntroPosX, Screen.height * 0.75f);
        Intro2.position = new Vector2(Screen.width + Intro2.rect.width / 2 - IntroPosX, Intro1.position.y - Intro1.rect.height);

        if (PlayerScript.points < 1)
        { IntroPosX = IntroPosX + Screen.width * Time.deltaTime * 0.25f; }
        else
        { IntroPosX = IntroPosX + Screen.width * Time.deltaTime * 2; }

        if (IntroPosX > Screen.width + Intro1.rect.width)
        {
            Intro1.gameObject.SetActive(false); Intro2.gameObject.SetActive(false);
            this.enabled = false;
        }
    }
    public void Play()
    {
        Intro1.gameObject.SetActive(true);
        Intro2.gameObject.SetActive(true);
        IntroPosX = 0;
    }
}
