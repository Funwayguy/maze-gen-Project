using UnityEngine;

public interface mazeAlgorithm
{
    void Initialise();
    void CreateMaze();
    
    MazeCell[,] getCells();
    int getRows();
    int getColumns();
    float getWallSize();
    GameObject getWallObject();
}
