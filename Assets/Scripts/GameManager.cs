﻿using System;
using System.Collections;
using System.Collections.Generic;
using Battles;
using Character;
using Character.Controllers;
using Items;
using MazeGeneration;
using Puzzles;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Action eventNewLevelStarted;
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private bool initLevelOnStart;

    [SerializeField] private BattleControllerUi battleController;
    [SerializeField] private ItemSelectionsUi itemSelectionPanel;

    [SerializeField] private MazeController mazeController;
    [SerializeField] private Maze maze;

    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpriteRenderer playerRenderer;

    [SerializeField] private Canvas blackBackgroundCanvas;
    [SerializeField] private int blackBackgroundSortingOrder;
    
    [SerializeField] private LineOfSightData lineOfSightData;
    [Range(0.0f, 1.0f)] [SerializeField] private float lineOfSightLerpSpeed;

    [Space]
    
    [SerializeField] private List<PuzzleData> puzzles;
    [SerializeField] private int puzzlesPerLevel;

    private List<PuzzleData> _lastLevelPuzzles;
    private List<PuzzleData> LastLevelPuzzles
    {
        get
        {
            if (_lastLevelPuzzles == null)
            {
                _lastLevelPuzzles = new List<PuzzleData>();
            }
            return _lastLevelPuzzles;
        }
    }

    private float _currentLineOfSightRadius;

    public Maze Maze => maze;

    public LineOfSightData LineOfSightData => lineOfSightData;

    public PlayerController PlayerController => playerController;

    public BattleControllerUi BattleController => battleController;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        blackBackgroundCanvas.gameObject.SetActive(false);
        itemSelectionPanel.eventItemSelectionFinished += StartLevel;

        _currentLineOfSightRadius = lineOfSightData.LineOfSightRadius;

        if (initLevelOnStart)
        {
            StartLevel();
        }
    }
    
    private void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }
    
    private IEnumerator StartLevelCoroutine()
    {
        eventNewLevelStarted?.Invoke();
        
        // hide maze
        int playerSortingOrder = playerRenderer.sortingOrder;
        playerRenderer.sortingOrder = blackBackgroundSortingOrder;
        blackBackgroundCanvas.gameObject.SetActive(true);

        // generar maze
        GenerateMazeWithNewPuzzles();

        yield return null;
        
        // posicionar cámara encima de personaje
        SetCameraToPlayerPosition();
        
        // show maze
        playerRenderer.sortingOrder = playerSortingOrder;
        blackBackgroundCanvas.gameObject.SetActive(false);
        
        // retroalimentación de progreso
        
        // animación aumentar LoS
        yield return LerpLineOfSightToTargetValue(0, 1);  // make sure LoS radius is 0
        yield return LerpLineOfSightToTargetValue(_currentLineOfSightRadius, lineOfSightLerpSpeed);

        // habilitar input
        playerController.enabled = true;
    }
    
    [NaughtyAttributes.Button]
    public void NotifyPuzzleSolved()
    {
        FinishLevel();
    }

    private void FinishLevel()
    {
        StartCoroutine(FinishLevelCoroutine());
    }
    
    private IEnumerator FinishLevelCoroutine()
    {
        // desabilitar input
        playerController.enabled = false;
        
        // wait some time e.g. 1 sec
        yield return new WaitForSeconds(1);
        
        // animación reducir LoS
        _currentLineOfSightRadius = lineOfSightData.LineOfSightRadius;
        yield return LerpLineOfSightToTargetValue(0, lineOfSightLerpSpeed);
        
        // selección de items
        itemSelectionPanel.ShowItemSelectionPanel();
    }

    private IEnumerator LerpLineOfSightToTargetValue(float losTarget, float lerpSpeed)
    {
        float dif = Mathf.Abs(lineOfSightData.LineOfSightRadius - losTarget);
        while (dif > 1E-3)
        {
            float lerpValue = Mathf.Lerp(lineOfSightData.LineOfSightRadius, losTarget, lerpSpeed);
            lineOfSightData.SetLineOfSightRadius(lerpValue);
            dif = Mathf.Abs(lineOfSightData.LineOfSightRadius - losTarget);
            yield return null;
        }
        
        lineOfSightData.SetLineOfSightRadius(losTarget);
    }

    private void SetCameraToPlayerPosition()
    {
        float cameraPosZ = cameraTransform.position.z;
        var playerPos = playerController.transform.position;
        playerPos.z = cameraPosZ;
        cameraTransform.position = playerPos;
    }

    private List<PuzzleData> GetRandomPuzzles(int count, List<PuzzleData> except)
    {
        List<PuzzleData> puzzlesAvailable = new List<PuzzleData>(puzzles);
        List<PuzzleData> ret = new List<PuzzleData>();
        
        foreach (var exc in except)
        {
            if (puzzlesAvailable.Contains(exc))
            {
                puzzlesAvailable.Remove(exc);
            }
        }

        while (count > 0 && puzzlesAvailable.Count > 0)
        {
            int index = Random.Range(0, puzzlesAvailable.Count);
            var puzzle = puzzlesAvailable[index];
            puzzlesAvailable.RemoveAt(index);
            
            ret.Add(puzzle);
            count--;
        }

        return ret;
    }

    private List<MazeData> AddPuzzlesToMaze(List<PuzzleData> puzzlesToAdd)
    {
        var rooms = new List<MazeData>();
        mazeController.AlternativeDecorators.Clear();
        LastLevelPuzzles.Clear();
        foreach (var puzzle in puzzlesToAdd)
        {
            foreach (var roomData in puzzle.RoomsData)
            {
                rooms.Add(roomData);
            }

            foreach (var decorator in puzzle.Decorators)
            {
                mazeController.AlternativeDecorators.Add(decorator);
            }
            
            LastLevelPuzzles.Add(puzzle);
        }

        return rooms;
    }
    
    public void GenerateMazeWithNewPuzzles()
    {
        var puzzlesToAdd = GetRandomPuzzles(puzzlesPerLevel, LastLevelPuzzles);
        var rooms = AddPuzzlesToMaze(puzzlesToAdd);
        mazeController.GenerateMaze(rooms);
    }
    
    public void GenerateMazeWithPuzzleIndex(int index=-1)
    {
        if (index == -1)
        {
            GenerateMazeWithNewPuzzles();
        }
        else
        {
            var puzzle = puzzles[index];
            var puzzlesToAdd = new List<PuzzleData>()
            {
                puzzle
            };
            var rooms = AddPuzzlesToMaze(puzzlesToAdd);
            var centerQuadrant = new List<Vector2Int> { Vector2Int.one };
            mazeController.GenerateMaze(rooms, centerQuadrant);
        }
    }
}