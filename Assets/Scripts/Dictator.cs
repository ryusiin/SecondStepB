using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Dictator : MonoBehaviour
{
    // :: Status : Audio
    [Header("Audio")]
    public AudioClip BGM_Base;
    public AudioClip BGM_InHeist;
    private AudioSource AUDIOPlyaer;

    // : 0 Awake
    private void Awake()
    {
        // :: Overlap Check
        GameObject overlapApp = GameObject.FindObjectOfType<Dictator>().gameObject;
        if (overlapApp.GetInstanceID() != this.gameObject.GetInstanceID())
            UnityEngine.Object.Destroy(this.gameObject);

        // :: Only
        DontDestroyOnLoad(this.gameObject);

        // :: Audio
        this.AUDIOPlyaer = this.GetComponent<AudioSource>();

        // :: Load Scene
        this.LoadScene(EnumAll.eScene.INTRO);
    }

    // : 1 Start
    private void Start()
    {
    }

    // : Status
    public static string Nickname { get; set; }
    public static EnumAll.eTeam eTeam = EnumAll.eTeam.YELLOW;
    public static EnumAll.eResult eResult = EnumAll.eResult.LOSE;
    public static Color GetColor(EnumAll.eTeam eTeam)
    {
        switch(eTeam)
        {
            case EnumAll.eTeam.RED:
                return Controller_Color.GetColor(255, 85, 85);
            case EnumAll.eTeam.BLUE:
                return Controller_Color.GetColor(97, 175, 255);
            default:
                return Controller_Color.GetColor(124, 124, 124);
        }
    }

    public static bool CheckDictator()
    {
        Dictator dictator = GameObject.FindObjectOfType<Dictator>();
        if (dictator == null)
        {
            SceneManager.LoadScene(0);
            return false;
        }

        return true;
    }
    // : Load
    public void LoadScene(EnumAll.eScene eScene)
    {
        switch (eScene)
        {
            case EnumAll.eScene.INTRO:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("Intro");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
                            // :: Audio
                            this.PlayAudio(BGM_Base);

                            var ruler = FindObjectOfType<Intro_Ruler>();
                            ruler.Init();
                            ruler.Please_MoveScene = (scene) =>
                            {
                                this.LoadScene(scene);
                            };
                        }
                    };
                }
                break;
            case EnumAll.eScene.LOBBY:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("Lobby");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
                            // :: Audio
                            this.PlayAudio(BGM_Base);

                            var ruler = FindObjectOfType<Lobby_Ruler>();
                            ruler.Init();
                            ruler.Please_MoveScene = (scene) =>
                            {
                                this.LoadScene(scene);
                            };
                        }
                    };
                }
                break;
            case EnumAll.eScene.IN_HEIST_LOBBY:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("InHeist_Lobby");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
                            // :: Audio
                            this.PlayAudio(BGM_InHeist);

                            var ruler = FindObjectOfType<InHeist_Lobby_Ruler>();
                            ruler.Init();
                            ruler.Please_MoveScene = (scene) =>
                            {
                                this.LoadScene(scene);
                            };
                        }
                    };
                }
                break;
            case EnumAll.eScene.IN_HEIST:
                {
                    var ruler = FindObjectOfType<InHeist_Ruler>();
                    ruler.Init();
                    ruler.Please_MoveScene = (scene) =>
                    {
                        this.LoadScene(scene);
                    };
                    ruler.Set_Please();
                }
                break;
            case EnumAll.eScene.RESULT:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("Result");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
                            // :: Audio
                            this.PlayAudio(BGM_Base);

                            var ruler = FindObjectOfType<Result_Ruler>();
                            ruler.Init();
                            ruler.Please_MoveScene = (scene) =>
                            {
                                this.LoadScene(scene);
                            };
                        }
                    };
                }
                break;
        }
    }
    private void PlayAudio(AudioClip clip)
    {
        if (this.AUDIOPlyaer.clip == clip)
            return;

        this.AUDIOPlyaer.Stop();
        this.AUDIOPlyaer.clip = clip;
        this.AUDIOPlyaer.Play();
    }

    // : Debug
    private static int debugNo = 0;
    public static Dictionary<int, string> DictDebug { get; private set; }
    public static void Debug_Init(string name)
    {
        // :: for Use
        if (DictDebug == null) DictDebug = new Dictionary<int, string>();

        // :: Remember
        string note = string.Format(":: [Init] {0} : {1} Initialise Complete", SceneManager.GetActiveScene().name, name);
        DictDebug.Add(debugNo, note);
        debugNo += 1; // :: Debug Number +1

        // :: Show
        //Debug.Log(note);
    }
    public static void Debug_All(string name, string rawNote)
    {
        // :: for Use
        if (DictDebug == null) DictDebug = new Dictionary<int, string>();

        // :: Remember
        string note = string.Format(":: [Debug] {0} : {1} {2}", SceneManager.GetActiveScene().name, name, rawNote);
        DictDebug.Add(debugNo, note);
        debugNo += 1; // :: Debug Number +1

        // :: Show
        Debug.Log(note);
    }
    public static void Check_Null(string name, params GameObject[] gameObjects)
    {
        // :: for Use
        if (DictDebug == null) DictDebug = new Dictionary<int, string>();

        // :: Find
        foreach(var itm in gameObjects)
        {
            if(itm == null)
            {
                // :: Remember
                string note = string.Format(":: [Init] {0} : {1} has NULL Object", SceneManager.GetActiveScene().name, name);
                DictDebug.Add(debugNo, note);
                debugNo += 1; // :: Debug Number +1

                // :: Show
                Debug.Log(note);

                // :: EXIT
                return;
            }
        }
    }
}
