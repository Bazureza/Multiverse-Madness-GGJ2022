using DG.Tweening;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class CharacterCloneController : MonoBehaviour, IGameState
{
    [SerializeField] private Transform clone;
    [SerializeField] private int frameRecord;
    [SerializeField] private int numberOfClone;
    [SerializeField, ReadOnly] private Transform target;
    [SerializeField, ReadOnly] private List<RecordData> recordedData = new List<RecordData>();
    [SerializeField, ReadOnly] private TapeState currentState;

    private GameManager gameManager;

    private Coroutine record;
    private Coroutine playing;
    private List<Coroutine> iteratorClonePlaying = new List<Coroutine>();
    private List<GameObject> spawnedClone = new List<GameObject>();

    private float firstRecord;
    private float lastRecord;

    private bool freeze;

    public enum TapeState { None, RecordingTape, StopRecord, PlayingTape, StopTape }

    private void OnEnable()
    {
        gameManager.RegisterGameStateListener(this);
    }

    private void OnDisable()
    {
        gameManager.UnregisterGameStateListener(this);
    }

    private void Awake()
    {
        gameManager = ServiceLocator.Resolve<GameManager>();
        clone.CreatePool(numberOfClone);
    }

    [Button]
    public void StartRecording(Transform target)
    {
        if (currentState.Equals(TapeState.RecordingTape)) return;
        currentState = TapeState.RecordingTape;

        this.target = target;
        firstRecord = Time.time;
        if (record != null) StopCoroutine(record);
        record = StartCoroutine(DoRecordTape());
    }

    [Button]
    public void StopRecording()
    {
        if (!currentState.Equals(TapeState.RecordingTape)) return;
        currentState = TapeState.StopRecord;

        lastRecord = Time.time;
        if (record != null) StopCoroutine(record);
        RecordData data = new RecordData();
        data.position = target.position;
        data.time = Time.time;
        recordedData.Add(data);
    }

    [Button]
    public void PlayingTape()
    {
        if (currentState.Equals(TapeState.PlayingTape)) return;
        currentState = TapeState.PlayingTape;

        if (recordedData.Count < 2) return; 
        if (playing != null) StopCoroutine(playing);
        playing = StartCoroutine(DoPlayingTape());
    }

    [Button]
    public void StopTape()
    {
        if (!currentState.Equals(TapeState.PlayingTape)) return;
        currentState = TapeState.StopTape;

        if (playing != null) StopCoroutine(playing);
        foreach (Coroutine coroutineClone in iteratorClonePlaying)
        {
            if (coroutineClone != null) StopCoroutine(coroutineClone);
        }
    }

    [Button]
    public void DestroyClone()
    {
        if (!currentState.Equals(TapeState.StopTape)) return;
        currentState = TapeState.None;

        foreach (GameObject go in spawnedClone)
        {
            go.Recycle();
        }

        spawnedClone.Clear();
    }

    [Button]
    public void DestroyAllTape()
    {
        DestroyClone();
        iteratorClonePlaying.Clear();
        recordedData.Clear();
    }

    private IEnumerator DoRecordTape()
    {
        int frameCount = 0;
        
        while (true)
        {
            if (freeze)
            {
                yield return null;
                continue;
            }

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
        for (int i = 0; i < numberOfClone; i++)
        {
            while (freeze) yield return null;

            GameObject objSpawned = clone.gameObject.Spawn(recordedData[0].position);
            if (!spawnedClone.Contains(objSpawned)) spawnedClone.Add(objSpawned);
            iteratorClonePlaying.Add(StartCoroutine(DoPlayingTape(objSpawned.transform)));
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
                while (freeze) yield return null;

                t = clone.DOMove(recordedData[i].position, recordedData[i + 1].time - recordedData[i].time).SetEase(Ease.Linear);
                yield return t.WaitForCompletion();
            }

            yield return null;
        }
    }

    void IGameState.Pause()
    {
        freeze = true;
    }

    void IGameState.Resume()
    {
        freeze = false;
    }

    [System.Serializable]
    public struct RecordData
    {
        public Vector2 position;
        public float time;
    }
}
