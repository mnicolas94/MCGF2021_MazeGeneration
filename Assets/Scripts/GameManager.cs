using System;
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
    public Action eventFinishedLevel;
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [SerializeField] private bool initLevelOnStart;

    [SerializeField] private BattleControllerUi battleController;
    [SerializeField] private ItemSelectionsUi itemSelectionPanel;

    [SerializeField] private MazeController mazeController;
    [SerializeField] private Maze maze;

    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CharacterMovement playerMovement;
    [SerializeField] private SpriteRenderer playerRenderer;

    [SerializeField] private Canvas blackBackgroundCanvas;
    [SerializeField] private int blackBackgroundSortingOrder;
    
    [SerializeField] private LineOfSightData lineOfSightData;
    [Range(0.0f, 1.0f)] [SerializeField] private float lineOfSightLerpSpeed;

    [Space]
    
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private List<PuzzleData> puzzles;
    [SerializeField] private int puzzlesPerLevel;

    [SerializeField] private int currentLevel;

    private List<PuzzleData> _solvedPuzzles;
    private List<PuzzleData> SolvedPuzzles
    {
        get
        {
            if (_solvedPuzzles == null)
            {
                _solvedPuzzles = new List<PuzzleData>();
            }
            return _solvedPuzzles;
        }
    }

    private float _currentLineOfSightRadius;

    public Maze Maze => maze;

    public LineOfSightData LineOfSightData => lineOfSightData;

    public PlayerController PlayerController => playerController;

    public BattleControllerUi BattleController => battleController;

    public int CurrentLevel => currentLevel;

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

        _currentLineOfSightRadius = lineOfSightData.LineOfSightRadius;

        if (initLevelOnStart)
        {
            currentLevel = 0;
            StartLevel();
        }
    }

    private IEnumerator CoroutineSequence(List<IEnumerator> coroutines)
    {
        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }
    
    private void StartLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }
    
    public void NotifyPuzzleSolved(PuzzleData solvedPuzzle)
    {
        currentLevel++;
        SolvedPuzzles.Add(solvedPuzzle);

        if (IsGameCompleted())
        {
            StartCoroutine(CoroutineSequence(new List<IEnumerator>
            {
                FinishLevelCoroutine(),
                // diálogo final
            }));
            print("me gané el juego");
        }
        else
        {
            StartCoroutine(CoroutineSequence(new List<IEnumerator>
            {
                FinishLevelCoroutine(),
                SelectItemsCoroutine(),
                StartLevelCoroutine()
            }));
        }
    }

    public void NotifyPuzzleSolved(PuzzleData solvedPuzzle, float delayTime)
    {
        StartCoroutine(NotifyPuzzleSolvedDelayCoroutine(solvedPuzzle, delayTime));
    }

    private IEnumerator NotifyPuzzleSolvedDelayCoroutine(PuzzleData solvedPuzzle, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        NotifyPuzzleSolved(solvedPuzzle);
    }
    
    public void RestartLevel()
    {
        if (battleController.IsRunningBattle)
        {
            battleController.EndBattle();
        }
        
        StartCoroutine(CoroutineSequence(new List<IEnumerator>
        {
            FinishLevelCoroutine(),
            StartLevelCoroutine()
        }));
    }

    private bool IsGameCompleted()
    {
        return currentLevel == levels.Count;
    }
    
    private IEnumerator StartLevelCoroutine()
    {
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
        eventNewLevelStarted?.Invoke();
    }
    
    private IEnumerator FinishLevelCoroutine()
    {
        eventFinishedLevel?.Invoke();
        
        // desabilitar input
        playerMovement.Stop();
        playerController.enabled = false;
        
        // wait some time e.g. 1 sec
        yield return new WaitForSeconds(1);
        
        // animación reducir LoS
        _currentLineOfSightRadius = lineOfSightData.LineOfSightRadius;
        yield return LerpLineOfSightToTargetValue(0, lineOfSightLerpSpeed);
    }

    private IEnumerator SelectItemsCoroutine()
    {
        // selección de items
        itemSelectionPanel.ShowItemSelectionPanel();

        while (itemSelectionPanel.SelectingItems)  // wait for item selection
        {
            yield return null;
        }
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

    private List<PuzzleData> GetRandomPuzzles(int count, List<PuzzleData> puzzlesAvailable, List<PuzzleData> except)
    {
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
        }

        return rooms;
    }
    
    public void GenerateMazeWithNewPuzzles()
    {
        int currentLevelIndex = Math.Min(currentLevel, levels.Count - 1);
        var currentLevelData = levels[currentLevelIndex];
        var availablePuzzles = new List<PuzzleData>(currentLevelData.Puzzles);
        var puzzlesToAdd = GetRandomPuzzles(puzzlesPerLevel, availablePuzzles, SolvedPuzzles);
        var rooms = AddPuzzlesToMaze(puzzlesToAdd);
        var quadrants = new List<Vector2Int>(currentLevelData.Quadrants);
        mazeController.GenerateMaze(currentLevelData.MazeWidth, currentLevelData.MazeHeight, rooms, quadrants);
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
            var puzzlesToAdd = new List<PuzzleData>
            {
                puzzle
            };
            var rooms = AddPuzzlesToMaze(puzzlesToAdd);
            var centerQuadrant = new List<Vector2Int>
            {
                Vector2Int.one,
                Vector2Int.one + Vector2Int.left
            };
            mazeController.GenerateMaze(rooms, centerQuadrant);
        }
    }
}