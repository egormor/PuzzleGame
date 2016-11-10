using UnityEngine;
using System.Collections;
using mvc;
using UnityEngine.UI;

/// <summary>
/// Root class for all views.
/// </summary>
public class PuzzleGameView : View<PuzzleGameApplication>
{
    public GameObject[] puzzlePrefabs;

    [SerializeField]private GameObject _winPanel;
    [SerializeField]private GameObject _playingPanel;

    // starting position of first element
    [SerializeField]private float _startPosX = -6f;
    [SerializeField]private float _startPosY = 6f;

    //puzzleSizes
    private float _puzzleSizeX;
    private float _puzzleSizeY;

    private  Vector3[,] _placeablePositions;//where we can place puzzle
    
    private  GameObject[,] _visualGrid;



    public void Init()
    {
        _puzzleSizeX = puzzlePrefabs[1].GetComponent<SpriteRenderer>().bounds.size.x;
        _puzzleSizeY = puzzlePrefabs[1].GetComponent<SpriteRenderer>().bounds.size.y;
        
        float posXreset = _startPosX;
        _placeablePositions = new Vector3[4, 4];
        for (int y = 0; y < 4; y++)
        {
            _startPosY -= _puzzleSizeY;
            for (int x = 0; x < 4; x++)
            {
                _startPosX += _puzzleSizeX;
                _placeablePositions[x, y] = new Vector3(_startPosX, _startPosY, 0);
            }
            _startPosX = posXreset;
        }
    }
    public void InitBoard(int[,] positionsOnGrid)
    {
        int i = 0;
        if (_visualGrid != null)
        {
            foreach (GameObject o in _visualGrid)
            {
                Destroy(o);
            }
        }
        _visualGrid = new GameObject[positionsOnGrid.GetLength(0), positionsOnGrid.GetLength(1)];
        
        //Instantiate board
        for (int y = 0; y < positionsOnGrid.GetLength(1); y++)
        {
            for (int x = 0; x < positionsOnGrid.GetLength(0); x++)
            {
                if (_visualGrid[x, y] == null)
                {
                    if (puzzlePrefabs[positionsOnGrid[x, y]] !=null)
                    {
                        _visualGrid[x, y] = Instantiate(puzzlePrefabs[positionsOnGrid[x, y]], _placeablePositions[x, y], Quaternion.identity) as GameObject;
                        _visualGrid[x, y].name = "ID-" + i;
                        _visualGrid[x, y].transform.parent = transform;                    
                        i++;
                    }
                    
                }
            }
        }
        
    }

    public void UpdateBoard(int puzzleToMoveId)
    {
        //Empty position
        int emptyX = 0;
        int emptyY = 0;

        int toMoveX = 0;
        int toMoveY = 0;

        //Define indexes of puzzles to move
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (_visualGrid[x, y].GetComponent<PuzzleView>().id == 0)
                {
                    emptyX = x;
                    emptyY = y;
                }
                else
                {
                    if (_visualGrid[x, y].GetComponent<PuzzleView>().id == puzzleToMoveId)
                    {
                        toMoveX = x;
                        toMoveY = y;
                    }                      
                }
            }         
        }
        //Swap with empty
        Vector3 t = _visualGrid[toMoveX, toMoveY].transform.position;
        _visualGrid[toMoveX, toMoveY].transform.position= _visualGrid[emptyX, emptyY].transform.position;
        _visualGrid[emptyX, emptyY].transform.position = t;
    }

    public void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        _playingPanel.SetActive(false);
    }

    public void ShowPlayingPanel()
    {
        _winPanel.SetActive(false);
        _playingPanel.SetActive(true);
    }
}
