using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] private Transform clone;
    [SerializeField] private int frameRecord;
    [SerializeField] private int numberOfClone;
    [SerializeField, ReadOnly] private Transform target;
    [SerializeField, ReadOnly] private List<RecordData> recordedData = new List<RecordData>();

    private Coroutine record;
    private Coroutine playing;
    private Coroutine[] iteratorClonePlaying;

    private float firstRecord;
    private float lastRecord;

    [Button]
    public void StartRecording(Transform target)
    {
        this.target = target;
        firstRecord = Time.time;
        if (record != null) StopCoroutine(record);
        record = StartCoroutine(DoRecordTape());
    }

    [Button]
    public void StopRecording()
    {
        lastRecord = Time.time;
        if (record != null) StopCoroutine(record);
    }

    [Button]
    public void PlayingTape()
    {
        if (playing != null) StopCoroutine(playing);
        playing = StartCoroutine(DoPlayingTape());
    }

    [Button]
    public void StopTape()
    {
        if (playing != null) StopCoroutine(playing);
        foreach (Coroutine coroutineClone in iteratorClonePlaying)
        {
            if (coroutineClone != null) StopCoroutine(coroutineClone);
        }
    }

    private IEnumerator DoRecordTape()
    {
        int frameCount = 0;
        
        while (true)
        {
            frameCount++;
            yield return null;
            if (frameCount == frameRecord)
            {
                frameCount = 0;

                RecordData data = new RecordData();
                data.position = target.position;
                data.time = Time.time;
                recordedData.Add(data);
            }
        }
    }

    private IEnumerator DoPlayingTape()
    {
        iteratorClonePlaying = new Coroutine[numberOfClone];
        for (int i = 0; i < numberOfClone; i++)
        {
            GameObject objSpawned = Instantiate(clone.gameObject);
            iteratorClonePlaying[i] = StartCoroutine(DoPlayingTape(objSpawned.transform));
            yield return new WaitForSeconds((lastRecord - firstRecord) / numberOfClone);
        }
    }

    private IEnumerator DoPlayingTape(Transform clone)
    {
        while (true)
        {
            clone.position = recordedData[0].position;
            Tween t;

            for (int i = 0; i < recordedData.Count - 1; i++)
            {
                t = clone.DOMove(recordedData[i].position, recordedData[i + 1].time - recordedData[i].time).SetEase(Ease.Linear);
                yield return t.WaitForCompletion();
            }

            yield return null;
        }
    }

    [System.Serializable]
    public struct RecordData
    {
        public Vector2 position;
        public float time;
    }
}
