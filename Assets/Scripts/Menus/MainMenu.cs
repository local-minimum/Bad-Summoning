using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    bool running;

    [SerializeField]
    string PlayScene = "level";

    [SerializeField]
    SimpleButton PlayBtn;

    [SerializeField]
    SimpleButton ResumeBtn;

    [SerializeField]
    SimpleButton QuitBtn;

    bool QuittablePlatform => !Application.isEditor && Application.platform != RuntimePlatform.WebGLPlayer;

    private void OnEnable()
    {
        if (running)
        {
            Time.timeScale = 0f;
        }

        ResumeBtn.gameObject.SetActive(running);
        PlayBtn.Text = running ? "Restart" : "Play";

        Debug.Log(Application.isEditor);
        QuitBtn.gameObject.SetActive(QuittablePlatform);
    }

    public void ResumePlay()
    {
        Debug.Log("Resume Play...");
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void StartPlay()
    {
        Debug.Log("Start Play...");
        SceneManager.LoadScene(PlayScene);
    }

    public void DoQuit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void DoSettings()
    {
        Debug.Log("Settings...");
    }

    public void DoCredits()
    {
        Debug.Log("Credits...");
    }
}
