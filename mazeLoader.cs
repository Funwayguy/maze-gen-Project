using UnityEngine;

public class mazeLoader : MonoBehaviour {
    public int mazeRows, mazeColumns;
    public GameObject wall;
    public float size = 2f;

	// Use this for initialization
	void Start () {
        mazeAlgorithm ma = new huntAndKillMazeAlgorithm (mazeRows, mazeColumns, wall, size);
        ma.Initialise();
        ma.CreateMaze();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
