using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Random = UnityEngine.Random;

public class LanternSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private int maxLanterns;
    [SerializeField] private float minPeriod;
    [SerializeField] private float maxPeriod;
    [SerializeField] private int maxInFrame;
    [SerializeField] private bool spawnOnAwake;

    [Header("Movement Settings")]
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float minLifetime;
    [SerializeField] private float maxLifetime;

    private List<MeshRenderer> _lanterns = new List<MeshRenderer>();
    private List<Sequence> _sequences = new List<Sequence>();
    private float _period;
    private float _lastTime;
    private static readonly int Offset = Shader.PropertyToID("_Offset");
    private bool _paused;

    private GameLoopManager _gameLoopManager;


    void Awake()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnLevelReady += Play;
        _gameLoopManager.OnComplete += Pause;
        _gameLoopManager.OnComplete += Stop;
        
        
        _paused = false;
        _period = Random.Range(minPeriod, maxPeriod);
        _lastTime = spawnOnAwake ? -_period : 0;
    }

    void Update()
    {
        if (_paused || !(Time.timeSinceLevelLoad >= _lastTime + _period)) return;
        
        _lastTime = Time.timeSinceLevelLoad;
        _period = Random.Range(minPeriod, maxPeriod);

        int numToSpawn = Mathf.Min(maxInFrame, Mathf.Max(0, maxLanterns - _lanterns.Count));
        for (int i = 0; i < numToSpawn; i++)
        {
            SpawnLantern();
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.DrawWireCube(Vector3.zero, Vector3.one);
    }

#endif

    [ContextMenu("Play")]
    public void Play() => _paused = false;
    
    [ContextMenu("Pause")]
    public void Pause() => _paused = true;
    
    void SpawnLantern()
    {
        Vector3 spawnPosition =
            transform.TransformPoint(new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f));
        Quaternion spawnRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
        Vector3 targetScale = lanternPrefab.transform.localScale;
        
        GameObject newLantern = Instantiate(lanternPrefab, spawnPosition, spawnRotation);
        MeshRenderer newRenderer = newLantern.GetComponent<MeshRenderer>();
        newLantern.transform.localScale = Vector3.zero;

        float lifetime = Random.Range(minLifetime, maxLifetime);

        Sequence sequence = DOTween.Sequence();
        _sequences.Add(sequence);

        sequence.Append(newLantern.transform.DOScale(targetScale, scaleSpeed));

        sequence.Insert(
            lifetime - scaleSpeed, 
            newLantern.transform.DOScale(Vector3.zero, scaleSpeed).OnComplete(
                () => { DestroyLantern(newRenderer); }
                )
            );

        sequence.OnComplete(
            () =>
            {
                if (_sequences.Contains(sequence))
                    _sequences.Remove(sequence);
            });
        
        newRenderer.material.SetFloat(Offset, Random.value);

        _lanterns.Add(newRenderer);
    }

    void DestroyLantern(MeshRenderer lanternRenderer)
    {
        _lanterns.Remove(lanternRenderer);
        Destroy(lanternRenderer.gameObject);
    }

    void Stop()
    {
        for (int i = _sequences.Count - 1; i >= 0; i--)
        {
            _sequences[i].timeScale = (_sequences[i].Duration() - _sequences[i].Elapsed()) / 2f;
        }
    }
}
