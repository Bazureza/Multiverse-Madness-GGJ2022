using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEvent changeWhiteTime;
    [SerializeField] private UnityEvent changeBlackTime;
    [SerializeField] private float changeTime;
    [SerializeField, ReadOnly] private bool dayTime = true;

    private void Start()
    {
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_OUT, 1);
        StartCoroutine(ChangeScenery());
    }

    private void ChangeCamera()
    {
        if (dayTime) changeWhiteTime?.Invoke();
        else changeBlackTime?.Invoke();
    }

    private IEnumerator ChangeScenery()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeTime);
            dayTime = !dayTime;
            ChangeCamera();
        }
    }

    public void StartGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
