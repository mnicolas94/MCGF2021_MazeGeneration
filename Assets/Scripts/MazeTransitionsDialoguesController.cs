using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Dialogues;
using Dialogues.UI;
using Puzzles;
using UnityEngine;

public class MazeTransitionsDialoguesController : MonoBehaviour
{
    [SerializeField] private DialoguePanel dialoguePanel;
    
    [SerializeField] private DialogueSequenceBase startGameDialogue;
    [SerializeField] private DialogueSequenceBase gameCompletedDialogue;
    [SerializeField] private DialogueSequenceBase mazeTimeOutDialogue;
    [SerializeField] private DialogueSequenceBase loosedGameDialogue;
    [SerializeField] private List<PuzzleDialogues> puzzlesDialogues;

    public DialogueSequenceBase StartGameDialogue => startGameDialogue;

    public DialogueSequenceBase MazeTimeOutDialogue => mazeTimeOutDialogue;

    public DialogueSequenceBase GetSolvedPuzzleDialogue(PuzzleData puzzleData)
    {
        foreach (var puzzleDialogue in puzzlesDialogues)
        {
            if (puzzleDialogue.puzzleData == puzzleData)
            {
                return puzzleDialogue.dialogues;
            }
        }

        return null;
    }

    private void Start()
    {
        GameManager.Instance.eventGameStarted += () => ShowDialogue(startGameDialogue);
        GameManager.Instance.eventGameCompleted += () => ShowDialogue(gameCompletedDialogue);
        GameManager.Instance.eventRestartedByTimeout += () => ShowDialogue(mazeTimeOutDialogue);
        GameManager.Instance.eventGameLoosed += () => ShowDialogue(loosedGameDialogue);
        GameManager.Instance.eventPuzzleSolved += ShowSolvedPuzzleDialogue;
    }

    private void ShowDialogue(DialogueSequenceBase dialogue)
    {
        dialoguePanel.PushDialogueSequence(dialogue);
    }

    private void ShowSolvedPuzzleDialogue(PuzzleData puzzle)
    {
        var dialogue = GetSolvedPuzzleDialogue(puzzle);
        ShowDialogue(dialogue);
    }
}

[Serializable]
public struct PuzzleDialogues
{
    public PuzzleData puzzleData;
    public DialogueSequenceBase dialogues;
}