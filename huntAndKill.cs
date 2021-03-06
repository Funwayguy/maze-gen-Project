using UnityEngine;

public class huntAndKillMazeAlgorithm : mazeAlgorithm {
    protected MazeCell[,] mazeCells;
    protected int mazeRows, mazeColumns;
    protected GameObject wall;
    protected float size;
    
    private int currentRow = 0;
    private int currentColumn = 0;

    private bool courseComplete = false;

    public huntAndKillMazeAlgorithm(int mazeRows, int mazeColumns, GameObject wall, float size)
    {
        this.mazeCells = new MazeCell[mazeRows, mazeColumns];
        this.mazeRows = mazeRows;
        this.mazeColumns = mazeColumns;
        this.size = size;
        this.wall = wall;
    }
    
    public void Initialise()
    {
        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

                mazeCells[r, c].floor = GameObject.Instantiate(wall, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].floor.name = "Floor " + r + "," + c;
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);

                if (c == 0)
                {
                    mazeCells[r, c].westWall = GameObject.Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;

                }
                mazeCells[r, c].eastWall = GameObject.Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;

                if (r == 0)
                {
                    mazeCells[r, c].northWall = GameObject.Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                }

                mazeCells[r, c].southWall = GameObject.Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
            }
        }
    }
    
    public void CreateMaze()
    {
        HuntAndKill();
    }

    private void HuntAndKill()
    {
        mazeCells[currentRow, currentColumn].visited = true;

        while(! courseComplete)
        {
            Kill(); //until hits a dead end
            Hunt();
            
        }
    }

    private void Kill()
    {
        while(RouteStillAvailable(currentRow, currentColumn))
        {
            int direction = UnityEngine.Random.Range(1, 5);
            //north
            if (direction == 1 && CellsAvailable(currentRow -1, currentColumn))
            {
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn].northWall);
                DestroyWallIfItExists(mazeCells[currentRow -1, currentColumn].southWall);
                currentRow--;
            }
            //south
            else if (direction == 2 && CellsAvailable(currentRow + 1, currentColumn))
            {
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn].southWall);
                DestroyWallIfItExists(mazeCells[currentRow + 1, currentColumn].northWall);
                currentRow++;
            }
            //east
            else if (direction == 3 && CellsAvailable(currentRow, currentColumn +1))
            {
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn].eastWall);
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn +1].westWall);
                currentColumn++;
            }
            //west
            else if (direction == 4 && CellsAvailable(currentRow, currentColumn -1))
            {
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn].westWall);
                DestroyWallIfItExists(mazeCells[currentRow, currentColumn -1].eastWall);
                currentColumn--;
            }
            mazeCells[currentRow, currentColumn].visited = true;
        }
    }

    private void Hunt()
    {
        courseComplete = true; //set true and check if there is still something below

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                if(!mazeCells[r,c].visited && CellHasAnAdjacentVisitedCell(r,c))
                {
                    courseComplete = false; //found something left
                    currentRow = r;
                    currentColumn = c;

                    DestroyAdjecentWall(currentRow, currentColumn);
                    mazeCells[currentRow, currentColumn].visited = true;
                    return;
                }
            }
        }
    }
private bool RouteStillAvailable(int row, int column)
    {
        int availableRoutes = 0;

        if (row > 0 && !mazeCells[row-1, column].visited)
        {
            availableRoutes++;
        }
        if (row < mazeRows -1 && !mazeCells[row+1, column].visited)
        {
            availableRoutes++;
        }
        if(column > 0 && !mazeCells[row, column-1].visited)
        {
            availableRoutes++;
        }
        if(column < mazeColumns-1 && !mazeCells[row, column +1].visited)
        {
            availableRoutes++;
        }
        return availableRoutes > 0;
    }

    private bool CellsAvailable(int row, int column)
    {
        if(row>= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells[row, column].visited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void DestroyAdjecentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while(!wallDestroyed)
        {
            int direction = UnityEngine.Random.Range(1, 5);

            if(direction == 1 && row > 0 && mazeCells[row - 1, column].visited)
            {
                DestroyWallIfItExists(mazeCells[row, column].northWall);
                DestroyWallIfItExists(mazeCells[row - 1, column].southWall);
                wallDestroyed = true;
            }
            else if(direction == 2 && row < (mazeRows - 2) && mazeCells[row + 1, column].visited)
            {
                DestroyWallIfItExists(mazeCells[row, column].southWall);
                DestroyWallIfItExists(mazeCells[row + 1, column].northWall);
                wallDestroyed = true;
            }
            else if(direction == 3 && column > 0 && mazeCells[row, column - 1].visited)
            {
                DestroyWallIfItExists(mazeCells[row, column].westWall);
                DestroyWallIfItExists(mazeCells[row, column - 1].eastWall);
                wallDestroyed = true;
            }
            else if(direction == 4 && column < (mazeColumns - 2) && mazeCells[row, column + 1].visited)
            {
                DestroyWallIfItExists(mazeCells[row, column].eastWall);
                DestroyWallIfItExists(mazeCells[row, column + 1].westWall);
                wallDestroyed = true;
            }
        }
    }
private void DestroyWallIfItExists(GameObject wall)
    {
        if(wall != null)
        {
            GameObject.Destroy(wall);
        }
    }

    private bool CellHasAnAdjacentVisitedCell(int row, int column)
    {
        int visitedCells = 0;
        //Look north if we're at row 1 or more
        if(row > 0 && mazeCells[row -1, column].visited)
        {
            visitedCells++;
        }

        //Look south if we're at the 2nd last row or less
        if(row < (mazeRows - 2) && mazeCells[row + 1, column].visited)
        {
            visitedCells++;
        }

        //Look west if we're at coloumn 1 or more
        if(column > 0 && mazeCells[row, column - 1].visited)
        {
            visitedCells++;
        }

        //Look east if we're at the 2nd last column or less
        if(column < (mazeColumns - 2) && mazeCells[row, column + 1].visited)
        {
            visitedCells++;
        }
        //return if there are adjecent cells
        return visitedCells > 0;
    }
    
    public MazeCell[,] getCells()
    {
        return mazeCells;
    }
    
    public int getRows()
    {
        return mazeRows;
    }
    
    public int getColumns()
    {
        return mazeColumns;
    }
    
    public float getWallSize()
    {
        return size;
    }
    
    public GameObject getWallObject()
    {
        return wall;
    }
}
