using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private Rigidbody rb;
    public int points;
    private int SongInt;
    public Text PointsText;
    public Text Win;
    public Transform IntroFTP;
    public GameObject OKILooseText;
    GameObject Player;
    public GameObject[] Loose;
    public AudioSource coll;
    public GameObject OK;
    public GameObject RageQuit;
    public AudioSource Clap;
    public int SpeedInt = 20;
    public Controller MainScript;
    public Camera OverheadCamera;
    public Arduino arduino;

    void Start()
    {
        SetPointsText();

        Loose = GameObject.FindGameObjectsWithTag("Loose");
        Player = gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Space))
        {
            SpeedInt = 10;
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            SpeedInt = 40;
        }
        if (Input.GetKeyUp(KeyCode.Equals) || Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Keypad0) || Input.GetKeyUp(KeyCode.Space))
        { SpeedInt = 20; }

        if (points >= 1)
        {
            if (!MainScript.Mouse)
            {
                rb.drag = 1;
            }
            else
            {
                rb.drag = 2;
            }
        }
        else
        {
            rb.drag = 1;
        }
    }

    void FixedUpdate()
    {
        if (points > 0)
        {
            float moveHorizontal;
            float moveVertical;
            /*
            string output = MainScript.arduinoLCD.serialPort.ReadLine();
            int x = int.Parse(output.Split(' ')[0]);*/

            if (!MainScript.Mouse)
            {
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
            }
            else
            {
                moveHorizontal = Input.GetAxis("Mouse X") * 2;
                moveVertical = Input.GetAxis("Mouse Y") * 2;
            }
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            rb.AddForce(movement * SpeedInt);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            points = points + 1;
            SetPointsText();
            coll.Play();
            arduino.Send("1");
        }

        if (other.gameObject.CompareTag("Pickup B"))
        {
            other.gameObject.SetActive(false);
            points = points + 3;
            SetPointsText();
            coll.Play();
            arduino.Send("2");
        }

        if (other.gameObject.CompareTag("Loose"))
        {
			OKILooseText.SetActive (true);
            OK.SetActive(true);
            MainScript.ChangePauseInt(-1);
            RageQuit.SetActive(true);

            MainScript.StopCoroutine("RainBow");
            MainScript.StopCoroutine("StartRainBow");
            MainScript.MainTorch.SetActive(false);
            MainScript.Fire.SetActive(false);
            MainScript.audioSource.Stop();
            MainScript.Die.volume = 0.5f;

            MainScript.Gameover.Play();
            arduino.Send("3");
            Player.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Boundry")
        {
            Player.transform.position = new Vector3(0, 0.5f, 0);
        }
    }

    void SetPointsText()
    {
        PointsText.text = "Points: " + points.ToString();
        if (points == 1)
            StartCoroutine(MainScript.arduinoLCD.DelaySend(0.1f, "/nSky Fortress"));

        if (points >= 100)
        {
            MainScript.arduinoLCD.Send("/c    You Win!");
            Win.gameObject.SetActive(true);
            Clap.Play();
            rb.isKinematic = true;
            OK.SetActive(true);
            MainScript.ChangePauseInt(-1);

            MainScript.FireworksParticle.gameObject.SetActive(true);
            MainScript.FireworksParticle.Play();
            MainScript.FireworksCamera.enabled = true;
            MainScript.StopCoroutine("RainBow");
            MainScript.StopCoroutine("StartRainBow");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Win.gameObject.SetActive(false);
            if (points != 0)
            {
                MainScript.arduinoLCD.Send("Points: " + points + "/100  ");
            }
        }
    }

	public void OKIWin()
	{
		foreach (GameObject r in Controller.Pickup)
		{
            r.SetActive(true);
            r.transform.localRotation = Quaternion.Euler(new Vector3(45, 45, 45));
        }
		foreach (GameObject r in Controller.PickupBonus) 
		{
			r.SetActive(true);
            r.transform.localRotation = Quaternion.Euler(new Vector3(45, 45, 45));
        }
        
        points = 0;
		SetPointsText();

        OK.SetActive(false);

        transform.position = new Vector3 (0, 20f, 0);

        SpeedInt = 20;
        
		OKILooseText.SetActive(false);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;

        Player.SetActive(false);
    }
}