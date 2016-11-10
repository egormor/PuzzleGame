using UnityEngine;
using System.Collections;
using System.Linq;
using mvc;
enum GameState
{
    Playing, Win
}
public class PuzzleGameController : Controller<PuzzleGameApplication>
{
    private GameState _gameState=GameState.Playing;

    /// Handle notifications from the application.
    public override void OnNotification(string notification, Object target, params object[] data)
    {
        switch (notification)
        {
            case "scene.load":
            {
                //Scene loaded
                _gameState = GameState.Playing;
                RandomizeModel();
                app.view.Init();
                app.view.InitBoard((int[,]) app.model.serializedGrid.Clone());
                break;
            }
            case "button.restart@click":
            {
                //Button restart clicked
                _gameState = GameState.Playing;
                app.view.ShowPlayingPanel();
                RandomizeModel();
                app.view.InitBoard((int[,]) app.model.serializedGrid.Clone());
                //app.view.timer.Play();
                break;
            }
            case "button.quit@click":
            {
                //Button quit clicked
                Application.Quit();
                break;
            }
            case "mouse.clicked":
            {
                //mouse clicked
                int id = (int) data[0];
                if (id != 0 && _gameState==GameState.Playing)
                {
                    SwapPuzzles(id);
                    if (IsWin())
                    {
                        app.view.ShowWinPanel();
                        _gameState=GameState.Win;
                    }
                }
                break;
            }
        }
    }

    bool IsWin()
    {
        int t = 0;
        for(int i=0;i<app.model.serializedGrid.GetLength(1);i++)
        {
            for (int j = 0; j < app.model.serializedGrid.GetLength(0);j++)
            {
                if (app.model.serializedGrid[i,j] == t)
                {
                    t++;
                }
                else return false;
                ;
            }

        }
        return true;
    }

    //set random puzzles 
    void RandomizeModel()
    {
        int x = app.model.x;
        int y = app.model.y;
        
        app.model.serializedGrid = new int[x, y];
        System.Random r1 = new System.Random();

        var values =
            Enumerable
                .Range(0, x * y)
                .OrderBy(n => r1.Next())
                .ToArray();

        for (int j1 = 0; j1 < y; j1++)
        {
            for (int i1 = 0; i1 < x; i1++)
            {
                app.model.serializedGrid[i1, j1] = values[i1 * x + j1];
            }
        }
    }
    
    void SwapPuzzles(int id)
    {
        int width = app.model.x;
        int height = app.model.y;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (app.model.serializedGrid[x, y] == id)
                {
                    if (x > 0 && app.model.serializedGrid[x - 1, y] == 0)
                    {
                        ReplaceBlocks(x, y, x - 1, y);
                        app.view.UpdateBoard(id);
                        return;
                    }
                    else if (x < width-1 && app.model.serializedGrid[x + 1, y] == 0)
                    {
                        ReplaceBlocks(x, y, x + 1, y);
                        app.view.UpdateBoard(id);
                        return;
                    }
                    else if (y > 0 && app.model.serializedGrid[x, y - 1] == 0)
                    {
                        
                        ReplaceBlocks(x, y, x, y - 1);
                        app.view.UpdateBoard(id);
                        return;
                    }
                    else if (y < height-1 && app.model.serializedGrid[x, y + 1] == 0)
                    {
                       
                        ReplaceBlocks(x, y, x, y + 1);
                        app.view.UpdateBoard(id);
                        return;
                    }
                }        
            }
        }
    }
    
    void ReplaceBlocks(int x1, int y1, int x2, int y2)
    {
        int t;
        t= app.model.serializedGrid[x1, y1];
        app.model.serializedGrid[x1, y1] = app.model.serializedGrid[x2, y2];
        app.model.serializedGrid[x2, y2] = t;
    }
}
