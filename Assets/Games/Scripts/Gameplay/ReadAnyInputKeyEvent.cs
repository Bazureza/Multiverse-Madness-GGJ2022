using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReadAnyInputKeyEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onSuccessReadingInput;

    private bool readingActive;

    public void StartReading()
    {
        if (readingActive) return;
        readingActive = true;

        StartCoroutine(DOReadingInput());
    }

    private IEnumerator DOReadingInput()
    {
        while (!InputInfo.AnyInput.Held || Input.GetKey(KeyCode.Escape)) yield return null;
        onSuccessReadingInput?.Invoke();
        readingActive = false;
    }
}
