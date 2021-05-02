using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using MazeGeneration;
using Puzzles;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Action eventNewLevelStarted;
    
    private static GameManager _instance;
    public static GameManager Instance => _instance;

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

    public Maze Maze => maze;

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
    }

    [NaughtyAttributes.Button]
    public void NotifyPuzzleSolved()
    {
        StartCoroutine(LevelTransitionFirstPhase());
    }

    private IEnumerator LevelTransitionFirstPhase()
    {
        // desabilitar input
        playerController.enabled = false;
        
        // wait some time e.g. 1 sec
        yield return new WaitForSeconds(1);
        
        // animación reducir LoS
        float currentLineOfSightRadius = lineOfSightData.LineOfSightRadius;
        yield return LerpLineOfSightToTargetValue(0);
        
        // selección de items
        
        
        // 2nd phase
        eventNewLevelStarted?.Invoke();
        
        // hide maze
        int playerSortingOrder = playerRenderer.sortingOrder;
        playerRenderer.sortingOrder = blackBackgroundSortingOrder;
        blackBackgroundCanvas.gameObject.SetActive(true);

        // generar maze
        GenerateMazeWithNewPuzzles();

        yield return null;
        
        // show maze
        playerRenderer.sortingOrder = playerSortingOrder;
        blackBackgroundCanvas.gameObject.SetActive(false);
        
        // posicionar personaje en suelo (y camara en personaje)
        var position = NearestFloor(Vector3.zero);
        SetPlayerAndCameraPositions(position);
        
        // retroalimentación de progreso
        
        // animación aumentar LoS
        yield return LerpLineOfSightToTargetValue(currentLineOfSightRadius);

        // habilitar input
        playerController.enabled = true;
    }

    private IEnumerator LerpLineOfSightToTargetValue(float losTarget)
    {
        float dif = Mathf.Abs(lineOfSightData.LineOfSightRadius - losTarget);
        while (dif > 1E-3)
        {
            float lerpValue = Mathf.Lerp(lineOfSightData.LineOfSightRadius, losTarget, lineOfSightLerpSpeed);
            lineOfSightData.SetLineOfSightRadius(lerpValue);
            dif = Mathf.Abs(lineOfSightData.LineOfSightRadius - losTarget);
            yield return null;
        }
        
        lineOfSightData.SetLineOfSightRadius(losTarget);
    }

    private Vector3 NearestFloor(Vector3 position)
    {
        var floorPositions = maze.GetFloorPositions();

        float minSqrDist = Mathf.Infinity;
        Vector3 nearestPosition = Vector3.zero;
        foreach (var pos in floorPositions)
        {
            var worldPosition = maze.Grid.CellToWorld(pos);

            var dif = worldPosition - position;
            float sqrDist = dif.x * dif.x + dif.y * dif.y;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearestPosition = worldPosition;
            }
        }

        return nearestPosition;
    }

    private void SetPlayerAndCameraPositions(Vector3 position)
    {
        float playerPosZ = playerController.transform.position.z;
        float cameraPosZ = cameraTransform.position.z;
        position.z = playerPosZ;
        playerController.transform.position = position;
        position.z = cameraPosZ;
        cameraTransform.position = position;
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

    public void GenerateMazeWithNewPuzzles()
    {
        var puzzlesToAdd = GetRandomPuzzles(puzzlesPerLevel, LastLevelPuzzles);
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
        
        mazeController.GenerateMaze(rooms);
    }
}