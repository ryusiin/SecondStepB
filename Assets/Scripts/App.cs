using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    // : 0 Awake
    private void Awake()
    {
        // :: Overlap Check
        GameObject overlapApp = GameObject.FindObjectOfType<App>().gameObject;
        if (overlapApp.GetInstanceID() != this.gameObject.GetInstanceID())
            UnityEngine.Object.Destroy(this.gameObject);

        // :: Only
        DontDestroyOnLoad(this.gameObject);

        // :: Load Scene
        this.LoadScene(EnumAll.eScene.INTRO);
    }

    // : Load
    private void LoadScene(EnumAll.eScene eScene)
    {
        switch(eScene)
        {
            case EnumAll.eScene.INTRO:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("Intro");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
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
            case EnumAll.eScene.IN_HEIST:
                {
                    AsyncOperation sync = SceneManager.LoadSceneAsync("InHeist");
                    sync.completed += loadScene =>
                    {
                        if (loadScene.isDone)
                        {
                            var ruler = FindObjectOfType<InHeist_Ruler>();
                            ruler.Init();
                        }
                    };
                }
                break;
        }
    }
}
