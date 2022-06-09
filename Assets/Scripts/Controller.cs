using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    CanvasGroup canvas;

    public GameObject arena;

    public static GameObject[] Pickup;
    public static GameObject[] PickupBonus;
    public static GameObject[] loose;

    public Material PickupMat;
    public Material PickupGlowMat;

    public Material PickupBonusMat;
    public Material PickupBonusGlowMat;

    public Material looseMat;
    public Material looseGlowMat;

    public GameObject Scoreboard;

    public ParticleSystem FireworksParticle;
    public ParticleSystem MapleLeafPlarticle;

    public Text SongID;
    private int SongInt = 0;
    public bool birdsEyeview;
    public GameObject Player;
    public GameObject Ground;
    public GameObject Menu;

    public AudioSource audioSource;

    public AudioClip[] Music = new AudioClip[12];

    public GameObject audioSpectrum;

    public AudioSource Die;
    public AudioSource MerryGo;
    public AudioSource Gameover;

    public AudioSource beep;

    public Camera firstPersonCamera;
    public Camera overheadCamera;
    public Camera VideoCamera;
    public Camera FireworksCamera;

    public GameObject Torch;
    public GameObject MainTorch;

    public GameObject D1;
    public GameObject D2;
    public GameObject D3;
    public GameObject D4;
    public GameObject D5;
    public GameObject D6;

    private int pauseInt;
    public int PauseInt
    {
        get
        {
            return pauseInt;
        }
        set
        {
            pauseInt = value;
            if (Mouse)
            {
                if (pauseInt == 0)//Unpaused
                    SetCursorMode(false);
                else
                    SetCursorMode(true);
            }
        }
    }
    public GameObject PauseButton;
    public GameObject UnpauseButton;

    public GameObject RageQuit;

    GameObject[] Loose;
    public GameObject looseRain;
    [SerializeField]
    GameObject PlayButton;
    
    public GameObject CheckChall;
    public bool Chall;
    public GameObject CheckMouse;
    public bool Mouse;
    public GameObject CheckPGM;
    public bool PGM;

    public GameObject MuteButton;
    public GameObject X;

    private int SetSongEnabled = 1;

    private bool gameplay;

    private int ColorInt;

    public Material SkyBox1;
    public Material SkyBox2;
    public Material SkyBox3;
    public Material SkyBox4;
    public Material SkyBox5;
    public Material SkyBox6;
    public Material SkyBox7;
    public Material SkyBox8;
    public Material SkyBox9;
    public Material SkyBox10;

    public Light DirectionalLight;

    public GameObject[] Walls; public GameObject[] WallBarrier = new GameObject[4];
    public GameObject[] RampPlatform1; public GameObject[] RampPlatform2 = new GameObject[4];

    public Material GroundMat;
    public Material Spring;
    public Material StarryGalaxy;
    public Material Paradise;
    public Material AutumnLeaves;
    public Material Smile;
    public Material butterfly;
    public Material Fireworks;
    public Material Seagull;
    public Material MLG;

    public GameObject Fire;

    public Color Orange;
    public Color Purple;
    public Color Dark;
    public Color Blue;

    public Dropdown ScreenResolutionDropdown;

    [SerializeField]
    PlayerController PlayerScript;
    [SerializeField]
    VideoController VC;
    TextAnimation TA;

    public static float AspectRatio = 1.777777f;
    [SerializeField]
    Text fpsText;

    [SerializeField]
    Text LCDcontrastText;

    [SerializeField]
    GameObject video;
    /*
    [SerializeField]
    CanvasGroup canvas;*/

    public Arduino arduino;
    public Arduino arduinoLCD;
    public Arduino arduinoMusic;

    string serialOutput;

    int amountFail;

    public Text keycardNote;
    
    void Start()
    {
        canvas = GetComponent<CanvasGroup>();

        VideoCamera.enabled = false;
        RenderSettings.skybox = SkyBox1;
        DirectionalLight.color = Color.white;
        
        StartCoroutine(Startt());
        //StartCoroutine(VideoDestroyer()); Video Controller
        StartCoroutine(ClearKeycardText());

        Ground.GetComponent<Renderer>().material = GroundMat;
        
        TA = GetComponent<TextAnimation>();

        Loose = GameObject.FindGameObjectsWithTag("Loose");

        Pickup = GameObject.FindGameObjectsWithTag("Pickup");
        PickupBonus = GameObject.FindGameObjectsWithTag("Pickup B");
        loose = GameObject.FindGameObjectsWithTag("Loose");
    }
    
    T[] Concat<T>(T[] Left, T[] Right)
    {
        T[] Array = new T[Left.Length + Right.Length];
        for (int x = 0; x < Left.Length; x++)
        {
            Array[x] = Left[x];
        }
        for (int x = 0; x < Right.Length; x++)
        {
            Array[x + Left.Length] = Right[x];
        }
        return Array;
    }

    IEnumerator Startt()
    {
        yield return new WaitForSeconds(0.01f);
        MainTorch.SetActive(false);
        Fire.SetActive(false);
        yield return null;
        StartCoroutine(DelayUpdateScreen());
    }
    
    void Update()
    {
        if(arduino != null)
        {
            try
            {
                serialOutput = arduino.serialPort.ReadLine();
            }
            catch { };
        }

        if (Input.GetKeyDown(KeyCode.F2)) { SetResolution(0); }
        if (Input.GetKeyDown(KeyCode.F3)) { SetResolution(1); }
        if (Input.GetKeyDown(KeyCode.F4)) { SetResolution(2); }
        if (Input.GetKeyDown(KeyCode.F5)) { SetResolution(3); }
        if (Input.GetKeyDown(KeyCode.F6)) { SetResolution(4); }
        if (Input.GetKeyDown(KeyCode.F7)) { SetResolution(5); }
        if (Input.GetKeyDown(KeyCode.F8)) { SetResolution(6); }
        if (Input.GetKeyDown(KeyCode.F9)) { SetResolution(7); }
        if (Input.GetKeyDown(KeyCode.F10)) { SetResolution(8); }

        if (Input.GetKeyDown(KeyCode.F11)) { Screen.fullScreen = !Screen.fullScreen; }

        if (gameplay)//Playing Game
        {
            RageQuit.SetActive(true);
            Menu.SetActive(false);

            if (PlayerScript.points > 0)
            {
                MuteButton.SetActive(true);
            }
            else
            {
                MuteButton.SetActive(false);
            }

            if (PauseInt == 1)//Paused
            {
                UnpauseButton.SetActive(true);
                PauseButton.SetActive(false);
                Player.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    OKIWin();
                    PlayerScript.OKIWin();
                }

                AudioListener.volume = 0.3f;
            }
            else
            {
                AudioListener.volume = 1;
            }

            if (PauseInt == -1)//You Win/You loose
            {
                UnpauseButton.SetActive(false);
                PauseButton.SetActive(false);
                RageQuit.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                { Quit(); }

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    OKIWin();
                    PlayerScript.OKIWin();
                }
            }

            if (PauseInt == 0)//Unpaused
            {
                UnpauseButton.SetActive(false);
                if (PlayerScript.points > 0)
                { PauseButton.SetActive(true); }

                Player.SetActive(true);

                if (SetSongEnabled == 1)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button5))
                    {
                        SongPlusMinus(true);
                    }
                    if (Input.GetKeyDown(KeyCode.Joystick1Button4))
                    {
                        SongPlusMinus(false);
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        SongPlusMinus(true);
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        SongPlusMinus(false);
                    }
                    if (serialOutput == "prev")
                    {
                        SongPlusMinus(false);
                        serialOutput = "";
                    }
                    if (serialOutput == "next")
                    {
                        SongPlusMinus(true);
                        serialOutput = "";
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        SongPlusMinus(false);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        SongPlusMinus(true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.Joystick1Button8))
                { birdsEyeview = !birdsEyeview; }

                //Birds eyeview
                if (birdsEyeview)
                {
                    firstPersonCamera.enabled = false;
                    overheadCamera.enabled = true;
                }
                //First person view
                if (!birdsEyeview)
                {
                    firstPersonCamera.enabled = true;
                    overheadCamera.enabled = false;
                }

                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    ColorInt = 0;
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    ColorInt = 1;
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    ColorInt = 2;
                }
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    ColorInt = 3;
                }
                if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    ColorInt = 4;
                }
                if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    ColorInt = 5;
                }
                if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    ColorInt = 6;
                }
                if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    ColorInt = 7;
                }
                if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    ColorInt = 8;
                }
                if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                {
                    ColorInt++;
                }
            }
            if (Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6))
            { Pause(); }
            
            Scoreboard.SetActive(true);
            VideoCamera.enabled = false;
        }
        else //GamePlay False
        {
            if (!Chall)
            {
                CheckChall.SetActive(false);

                foreach (GameObject R in Loose)
                {
                    R.SetActive(false);
                }
            }
            else
            {
                CheckChall.SetActive(true);

                foreach (GameObject R in Loose)
                {
                    R.SetActive(true);
                }
            }

            if (!Mouse)
            {
                CheckMouse.SetActive(false);
            }
            else
            {
                CheckMouse.SetActive(true);
            }

            if (!PGM)
            {
                CheckPGM.SetActive(false);
                Walls[0].transform.localPosition = new Vector3(-10, 0.5f, 0);
                Walls[1].transform.localPosition = new Vector3(10, 0.5f, 0);
                Walls[2].transform.localPosition = new Vector3(0, 0.5f, 10);
                Walls[3].transform.localPosition = new Vector3(0, 0.5f, -10);

                WallBarrier[0].transform.localPosition = new Vector3(-10, 0, 0);
                WallBarrier[1].transform.localPosition = new Vector3(10, 0, 0);
                WallBarrier[2].transform.localPosition = new Vector3(0, 0, 10);
                WallBarrier[3].transform.localPosition = new Vector3(0, 0, -10);
                //Scale
                Walls[0].transform.localScale = new Vector3(0.5f, 2.5f, 19.5f);
                Walls[1].transform.localScale = new Vector3(0.5f, 2.5f, 19.5f);
                Walls[2].transform.localScale = new Vector3(20.5f, 2.5f, 0.5f);
                Walls[3].transform.localScale = new Vector3(20.5f, 2.5f, 0.5f);
                foreach (GameObject r in RampPlatform2)
                {
                    r.SetActive(false);
                }
            }
            else
            {
                CheckPGM.SetActive(true);
                Walls[0].transform.localPosition = new Vector3(-11, 1.5f, 0);
                Walls[1].transform.localPosition = new Vector3(11, 1.5f, 0);
                Walls[2].transform.localPosition = new Vector3(0, 1.5f, 11);
                Walls[3].transform.localPosition = new Vector3(0, 1.5f, -11);

                WallBarrier[0].transform.localPosition = new Vector3(-11, 0, 0);
                WallBarrier[1].transform.localPosition = new Vector3(11, 0, 0);
                WallBarrier[2].transform.localPosition = new Vector3(0, 0, 11);
                WallBarrier[3].transform.localPosition = new Vector3(0, 0, -11);
                //Scale
                Walls[0].transform.localScale = new Vector3(0.5f, 2.5f, 21.5f);
                Walls[1].transform.localScale = new Vector3(0.5f, 2.5f, 21.5f);
                Walls[2].transform.localScale = new Vector3(22.5f, 2.5f, 0.5f);
                Walls[3].transform.localScale = new Vector3(22.5f, 2.5f, 0.5f);
                foreach (GameObject r in RampPlatform2)
                {
                    r.SetActive(true);
                }
            }

            audioSource.Stop();
            MerryGo.Stop();

            Menu.SetActive(true);
            RageQuit.SetActive(false);
            PauseButton.SetActive(false);

            D1.SetActive(false);
            D2.SetActive(false);
            D3.SetActive(false);
            D4.SetActive(false);
            D5.SetActive(false);
            D6.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                Play();
                VC.Play();
                TA.enabled = true;
                TA.Play();
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button6))
            {
                Quit();
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                Challenge();
            }
            Player.SetActive(false);
            Fire.SetActive(false);
            Scoreboard.SetActive(false);
            PlayerScript.OK.SetActive(false);
            PlayerScript.RageQuit.SetActive(false);
            UnpauseButton.SetActive(false);
            StopCoroutine("WaitTillTroll");
        }

        if (PauseInt >= 2)
        {
            PauseInt = 0;
        }

        if (ColorInt >= 9)
        {
            ColorInt = 0;
        }
        if (SongInt != 1)
        {
            SetColor();
            StopCoroutine("StartRainBow");
            StopCoroutine("RainBow");
        }
    }

    IEnumerator StartRainBow()
    {
        Player.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);

        Ground.GetComponent<Renderer>().material.color = Color.red;
        Player.GetComponent<Renderer>().material.color = Orange;
        yield return new WaitForSeconds(0.25f);

        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.red; }
        Ground.GetComponent<Renderer>().material.color = Orange;
        Player.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(0.25f);

        if (SongInt == 1)
        { StartCoroutine("RainBow"); }
    }
    IEnumerator RainBow()
    {
        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.red; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Orange; }
        Ground.GetComponent<Renderer>().material.color = Color.yellow;
        Player.GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.25f);

        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Orange; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.yellow; }
        Ground.GetComponent<Renderer>().material.color = Color.green;
        Player.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(0.25f);

        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.yellow; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.green; }
        Ground.GetComponent<Renderer>().material.color = Color.blue;
        Player.GetComponent<Renderer>().material.color = Purple;
        yield return new WaitForSeconds(0.25f);
        
        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.green; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.blue; }
        Ground.GetComponent<Renderer>().material.color = Purple;
        Player.GetComponent<Renderer>().material.color = Color.magenta;
        yield return new WaitForSeconds(0.25f); 

        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.blue; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Purple; }
        Ground.GetComponent<Renderer>().material.color = Color.magenta;
        Player.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        
        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Purple; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.magenta; }
        Ground.GetComponent<Renderer>().material.color = Color.red;
        Player.GetComponent<Renderer>().material.color = Orange;
        yield return new WaitForSeconds(0.25f);
        
        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.magenta; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Color.red; }
        Ground.GetComponent<Renderer>().material.color = Orange;
        Player.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(0.25f);
        
        foreach (GameObject a in Walls)
        { a.GetComponent<Renderer>().material.color = Color.red; }
        foreach (GameObject a in RampPlatform1)
        { a.GetComponent<Renderer>().material.color = Orange; }
        Ground.GetComponent<Renderer>().material.color = Color.yellow;
        Player.GetComponent<Renderer>().material.color = Color.green;

        if (SongInt == 1)
        { StartCoroutine("RainBow"); }
    }

    IEnumerator WaitTillTroll()
    {
        yield return new WaitForSeconds(10);
        SetSongEnabled = 0;
        audioSource.Stop();
        yield return new WaitForSeconds(1);
        Die.Play();
        yield return new WaitForSeconds(5);
        MerryGo.Play();
        StartCoroutine(Troll());
    }
    IEnumerator Troll()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject GO = Instantiate(looseRain, new Vector3(Random.Range(-9, 9), 10, Random.Range(-9, 9)), new Quaternion());
        Destroy(GO, 1.5f);
        if (gameplay)
        { StartCoroutine(Troll()); }
        else
        { StopCoroutine(Troll()); }
    }

    void SetColor()
    {

        if (ColorInt == 0)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.white; }
            Player.GetComponent<Renderer>().material.color = Color.white;
        }
        if (ColorInt == 1)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.red; }
            Player.GetComponent<Renderer>().material.color = Color.red;
        }
        if (ColorInt == 2)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Orange; }
            Player.GetComponent<Renderer>().material.color = Orange;
        }
        if (ColorInt == 3)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.yellow; }
            Player.GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (ColorInt == 4)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.green; }
            Player.GetComponent<Renderer>().material.color = Color.green;
        }
        if (ColorInt == 5)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.cyan; }
            Player.GetComponent<Renderer>().material.color = Color.cyan;

            foreach (GameObject a in RampPlatform1)
            { a.GetComponent<Renderer>().material.color = Blue; }
        }
        else
        {
            foreach (GameObject a in RampPlatform1)
            { a.GetComponent<Renderer>().material.color = Color.cyan; }
        }
        if (ColorInt == 6)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.blue; }
            Player.GetComponent<Renderer>().material.color = Color.blue;
        }
        if (ColorInt == 7)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Purple; }
            Player.GetComponent<Renderer>().material.color = Purple;
        }
        if (ColorInt == 8)
        {
            foreach (GameObject a in Walls)
            { a.GetComponent<Renderer>().material.color = Color.magenta; }
            Player.GetComponent<Renderer>().material.color = Color.magenta;
        }
        

        if(SongInt == 0 || SongInt == 2)
        { Ground.GetComponent<Renderer>().material = GroundMat; }
    }

    void SetSong()
    {
        audioSource.clip = Music[SongInt];
        SongID.text = Music[SongInt].name;
        arduinoLCD.Send("/n" + Music[SongInt].name);

        if (SongInt == 0)
        {
            audioSource.time = 2.9f;

            RenderSettings.skybox = SkyBox1;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = GroundMat;
        }
        else
        {
            audioSource.time = 0.0f;
        }

        if (SongInt == 1)
        {
            D1.SetActive(true);
            D2.SetActive(true);
            D3.SetActive(true);
            D4.SetActive(true);
            D5.SetActive(true);
            D6.SetActive(true);

            RenderSettings.skybox = SkyBox2;
            DirectionalLight.color = new Color(0.75f, 0.75f, 0.75f);
            Ground.GetComponent<Renderer>().material = StarryGalaxy;
            StartCoroutine(StartRainBow());
        }
        else
        {
            D1.SetActive(false);
            D2.SetActive(false);
            D3.SetActive(false);
            D4.SetActive(false);
            D5.SetActive(false);
            D6.SetActive(false);
        }

        if (SongInt == 2)
        {
            RenderSettings.skybox = SkyBox2;
            DirectionalLight.color = Dark;
            Torch.SetActive(true);
            MainTorch.SetActive(true);
            Ground.GetComponent<Renderer>().material = GroundMat;
        }
        else
        {
            Torch.SetActive(false);
            MainTorch.SetActive(false);
        }

        if (SongInt == 3)
        {
            RenderSettings.skybox = SkyBox3;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = Spring;
        }

        if (SongInt == 4)
        {
            RenderSettings.skybox = SkyBox4;
            DirectionalLight.color = Color.grey;
            Ground.GetComponent<Renderer>().material = Paradise;
        }

        if (SongInt == 5)
        {
            RenderSettings.skybox = SkyBox5;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = AutumnLeaves;
            MapleLeafPlarticle.Play();
        }
        else
        {
            MapleLeafPlarticle.Stop();
        }

        if (SongInt == 6)
        {
            RenderSettings.skybox = SkyBox6;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = Smile;
        }

        if (SongInt == 7)
        {
            RenderSettings.skybox = SkyBox7;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = butterfly;
        }

        if (SongInt == 8)
        {
            RenderSettings.skybox = SkyBox8;
            DirectionalLight.color = Dark;
            Ground.GetComponent<Renderer>().material = Fireworks;
        }

        if (SongInt == 9)
        {
            RenderSettings.skybox = SkyBox9;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = Seagull;
        }

        if (SongInt == 10)
        {
            RenderSettings.skybox = SkyBox10;
            DirectionalLight.color = Color.white;
            Fire.SetActive(true);
            Ground.GetComponent<Renderer>().material = MLG;
        }
        else
        {
            Fire.SetActive(false);
        }

        if (SongInt >= 11)
        {
            RenderSettings.skybox = SkyBox10;
            DirectionalLight.color = Color.white;
            Ground.GetComponent<Renderer>().material = GroundMat;
        }

        audioSource.Play();
    }

    IEnumerator ClearKeycardText()
    {
        yield return new WaitUntil(() => serialOutput != "");
        yield return new WaitForSeconds(1);
        serialOutput = "";
        StartCoroutine(ClearKeycardText());
    }

    public void Keycard()
    {
        if(arduino.serialPort == null)
        {
            Play();
        }
        else
        {
            StartCoroutine(WaitForKeycard());
            PlayButton.SetActive(false);
        }
    }
    IEnumerator WaitForKeycard()
    {
        yield return new WaitUntil(() => serialOutput != "");
        if(serialOutput == " 84 B3 64 4F\n")
        {
            beep.pitch = 2f;
            beep.Play();
            keycardNote.text = "Access Granted!";
            keycardNote.color = new Color(0, 0.75f, 0);
            yield return new WaitForSeconds(2);
            keycardNote.color = Color.black;
            amountFail = 0;
            Play();
        }
        else
        {
            beep.pitch = 0.5f;
            beep.Play();
            keycardNote.text = "Access Denied!";
            keycardNote.color = Color.red;
            yield return new WaitForSeconds(2);
            keycardNote.color = Color.black;
            keycardNote.text = "Please Scan keycard to play";
            
            amountFail++;
            if (amountFail >= 3)
                Application.Quit();
            
            StartCoroutine(WaitForKeycard());
        }
        serialOutput = "";
    }

    public void Play()
    {
        arduinoLCD.Send("Roll The Ball");
        StartCoroutine(arduinoLCD.DelaySend(0.2f, "/nBy: Sean Wang"));

        keycardNote.text = "Please Scan keycard to play";
        
        VideoCamera.GetComponent<VideoController>().Play();
        GetComponent<TextAnimation>().enabled = true;
        GetComponent<TextAnimation>().Play();

        birdsEyeview = false;

        Player.SetActive(true);
        SongInt = 0;
        audioSource.time = 0;
        audioSource.clip = Music[0];
        audioSource.Play();
        gameplay = true;
        RenderSettings.skybox = SkyBox1;
        DirectionalLight.color = Color.white;
        MainTorch.SetActive(false);
        Torch.SetActive(false);
        Fire.SetActive(false);
        Ground.GetComponent<Renderer>().material = GroundMat;
        MapleLeafPlarticle.Stop();
        SetCursorMode(!Mouse);

        SongID.text = "Sky Fortress";

        if (PGM)
        { StartCoroutine("WaitTillTroll"); }
    }
    public void Pause()
    {
        if (PlayerScript.points > 0)
        { PauseInt = PauseInt + 1; }
    }

    public void ChangePauseInt(int Number)
    { PauseInt = Number; }

    public void OKIWin()
    {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;

        gameplay = false;
        PauseInt = 0;
        SetCursorMode(true);
        SetSongEnabled = 1;
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
        VC.StartCoroutine("Video");
        FireworksParticle.Stop();
        FireworksParticle.gameObject.SetActive(false);
        FireworksCamera.enabled = false;
        PlayButton.SetActive(true);
    }

    public void PauseIntminus1()
    { PauseInt = -1; }
    public void Challenge()
    { Chall = !Chall; }
    public void MouseControll()
    { Mouse = !Mouse; }
    public void ProGame()
    { PGM = !PGM; }

    public void Mute()
    {
        X.SetActive(!X.activeSelf);
        AudioListener.pause = X.activeSelf;
        audioSpectrum.SetActive(!X.activeSelf);
    }

    public void SongPlusMinus(bool Positive)
    {
		if (SetSongEnabled == 1)
		{
			if (Positive)
            {
                SongInt++;
                if (SongInt > Music.Length - 1) { SongInt = 0; }
            }
            else
            {
                SongInt--;
                if (SongInt < 0) { SongInt = Music.Length - 1; }
            }
			SetSong();
		}
    }
    int targetFPS;

    public void SetFrame(float fps)
    {
        if(fps == 0) { QualitySettings.vSyncCount = 1; targetFPS = 0; fpsText.text = "FPS: VSync"; }
        else
        {
            targetFPS = Mathf.RoundToInt(fps) * 5;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFPS;
            fpsText.text = "FPS: " + targetFPS;
        }
    }
    public void SetResolution(int Number)
    {
        if (Number == 0) { Screen.SetResolution(Mathf.RoundToInt(360 * AspectRatio), 360, Screen.fullScreen); }
        if (Number == 1) { Screen.SetResolution(Mathf.RoundToInt(480 * AspectRatio), 480, Screen.fullScreen); }
        if (Number == 2) { Screen.SetResolution(Mathf.RoundToInt(720 * AspectRatio), 720, Screen.fullScreen); }
        if (Number == 3) { Screen.SetResolution(Mathf.RoundToInt(768 * AspectRatio), 768, Screen.fullScreen); }
        if (Number == 4) { Screen.SetResolution(Mathf.RoundToInt(1080 * AspectRatio), 1080, Screen.fullScreen); }
        if (Number == 5) { Screen.SetResolution(Mathf.RoundToInt(1440 * AspectRatio), 1440, Screen.fullScreen); }
        if (Number == 6) { Screen.SetResolution(Mathf.RoundToInt(1520 * AspectRatio), 1520, Screen.fullScreen); }
        if (Number == 7) { Screen.SetResolution(Mathf.RoundToInt(1800 * AspectRatio), 1800, Screen.fullScreen); }
        if (Number == 8) { Screen.SetResolution(Mathf.RoundToInt(2160 * AspectRatio), 2160, Screen.fullScreen); }
        StartCoroutine(DelayUpdateScreen());
    }
    public void Ratio(int Number)
    {
        if (Number == 0) { AspectRatio = 1.777777f; }
        if (Number == 1) { AspectRatio = 1.333333f; }
        if (Number == 2) { AspectRatio = 1.25f; }
        if (Number == 3) { AspectRatio = 1.6f; }
        Screen.SetResolution(Mathf.RoundToInt(Screen.height * AspectRatio), Screen.height, Screen.fullScreen);
    }
    public void SetGraphics(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);
        if(targetFPS == 0) { QualitySettings.vSyncCount = 1; }
        else { QualitySettings.vSyncCount = 0; }
    }
    public void SetLCDcontrast(float contrast)
    {
        LCDcontrastText.text = "LCD Contrast: " + contrast;
        contrast = (100 - contrast);
        contrast = Mathf.Round(contrast * 2.25f);
        arduino.Send("C " + contrast);
    }
    IEnumerator DelayUpdateScreen()
    {
        yield return null;
        ScreenResolutionDropdown.transform.GetChild(0).GetComponent<Text>().text = Screen.height + "p";
        if (Screen.height == 360) { ScreenResolutionDropdown.value = 0; }
        else if (Screen.height == 480) { ScreenResolutionDropdown.value = 1; }
        else if (Screen.height == 720) { ScreenResolutionDropdown.value = 2; }
        else if (Screen.height == 768) { ScreenResolutionDropdown.value = 3; }
        else if (Screen.height == 1080) { ScreenResolutionDropdown.value = 4; }
        else if (Screen.height == 1440) { ScreenResolutionDropdown.value = 5; }
        else if (Screen.height == 1520) { ScreenResolutionDropdown.value = 6; }
        else if (Screen.height == 1800) { ScreenResolutionDropdown.value = 7; }
        else if (Screen.height == 2160) { ScreenResolutionDropdown.value = 8; }
    }
    IEnumerator Delay(float Seconds, IEnumerator coroutine)
    {
        yield return new WaitForSeconds(Seconds);
        StartCoroutine(coroutine);
    }
    public static IEnumerator Delay(float Seconds, UnityAction function)
    {
        yield return new WaitForSeconds(Seconds);
        function();
    }

    /* Video Controller
    IEnumerator VideoDestroyer()
    {
        yield return new WaitForSeconds(video.GetComponent<Destroyer>().Seconds);
        canvas.alpha = 1f; //this makes everything visible
        canvas.blocksRaycasts = true;
        overheadCamera.farClipPlane = 50;
        overheadCamera.clearFlags = CameraClearFlags.Skybox;
    }*/

    void SetCursorMode(bool state)
    {
        Cursor.visible = state;

        if (state)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }
    public void Quit()
    { Application.Quit(); }

    private void OnApplicationQuit()
    {
        arduinoMusic.Send("0");
        arduinoLCD.Send("/0");
    }
}