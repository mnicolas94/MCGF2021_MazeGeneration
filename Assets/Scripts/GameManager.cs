using System;
using System.Collections;
using Character;
using MazeGeneration;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    [SerializeField] private MazeController mazecontroller;
    [SerializeField] private Maze maze;

    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpriteRenderer playerRenderer;

    [SerializeField] private Canvas blackBackgroundCanvas;
    [SerializeField] private int blackBackgroundSortingOrder;
    
    [SerializeField] private LineOfSightData lineOfSightData;
    [Range(0.0f, 1.0f)] [SerializeField] private float lineOfSightLerpSpeed;
    
    
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
        
        // seleccionar puzzles y ponerselos al maze controller
        
        // hide maze
        int playerSortingOrder = playerRenderer.sortingOrder;
        playerRenderer.sortingOrder = blackBackgroundSortingOrder;
        blackBackgroundCanvas.gameObject.SetActive(true);

        // generar maze
        mazecontroller.GenerateMaze();

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
}