using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This component can be used to access methods for changing the scene, and quitting the game
// It can be used together with UnityEventOnTrigger, or UI-button-events, to decide when a scene should be changed or the game should be closed
public class SceneChangeHandler : MonoBehaviour
{
    private ScreenFade fader;

    private void Start()
    {
        fader = GetComponentInChildren<ScreenFade>();
        StartCoroutine(fader.FadeIn(3));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(ChangeScene(sceneIndex));
    }

    private IEnumerator ChangeScene(int sceneIndex)
    {
        StartCoroutine(fader.FadeOut(3));
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartQuit()
    {
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        StartCoroutine(fader.FadeOut(3));
        yield return new WaitForSeconds(3);
        Application.Quit();
    }
}
