using UnityEngine;
using System.Collections;
using mvc;
using UnityEngine.UI;

/// <summary>
/// Class that handles the application data.
/// </summary>
public class PuzzleGameModel : Model<PuzzleGameApplication>
{
    public int x = 4;
    public int y = 4;
    public int[,] serializedGrid;

    
}
