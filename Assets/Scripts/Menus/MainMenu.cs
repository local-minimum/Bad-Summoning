using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField]
    GameObject Credits;

    [SerializeField]
    GameObject Settings;

    [SerializeField]
    SimpleButtonGroup ButtonGroup;

    bool QuittablePlatform => !Application.isEditor && Application.platform != RuntimePlatform.WebGLPlayer;

    private void OnEnable()
    {
        if (running)
        {
            Time.timeScale = 0f;
        }

        ResumeBtn.gameObject.SetActive(running);
        PlayBtn.Text = running ? "Restart" : "Play";

        QuitBtn.gameObject.SetActive(QuittablePlatform);
        
        Show();
    }

    public void ResumePlay()
    {
        Debug.Log("Resume Play...");
        if (!running)
        {
            Debug.LogError("Should not be possible to resume");
            return;
        }
        BlockableActions.RemoveActionBlock(this);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void StartPlay()
    {
        BlockableActions.RemoveActionBlock(this);
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
        gameObject.SetActive(false);
        Settings.SetActive(true);
    }

    public void DoCredits()
    {
        Debug.Log("Credits...");
        gameObject.SetActive(false);
        Credits.SetActive(true);
    }

    public void Show()
    {
        BlockableActions.BlockAction(this);
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        Credits.SetActive(false);
        Settings.SetActive(false);
        ButtonGroup.SelectDefault();
    }

    public void Show(InputAction.CallbackContext context)
    {
        if (context.performed && !gameObject.activeSelf) Show();
    }
}
