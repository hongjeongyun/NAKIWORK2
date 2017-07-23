using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Tile tilePrefab;

    public int tilesPerRow;
    public int numberOfTiles;
    public int numberOfMines;

    public int minesMarkedCorrectly = 0;
    public int tilesUncoverd = 0;
    public int minesRemaining = 0;

    public float distanceBetweenTiles = 1.0f;

    public string state = "inGame";

    public Tile[] tilesAll;

    public List<Tile> tilesMined;
    public List<Tile> tilesUnmined;

    public GameObject Player;
    public GameObject RestartMenu;

    GameObject target;

    void Awake()
    {
        //RestartMenu.SetActive(false);
        CreateTiles();
    }

    void Start()
    {
        minesRemaining = numberOfMines;
        minesMarkedCorrectly = 0;
        tilesUncoverd = 0;
        state = "inGame";

        StartCoroutine(gridUpdate());
    }

    void CreateTiles()
    {
        tilesAll = new Tile[numberOfTiles];
        tilesMined = new List<Tile>();
        tilesUnmined = new List<Tile>();

        float xOffset = 0.0f;
        float zOffset = 0.0f;

        for (int tilesCreated = 0; tilesCreated < numberOfTiles; tilesCreated++)
        {
            xOffset += distanceBetweenTiles;

            if(tilesCreated % tilesPerRow == 0)
            {
                zOffset += distanceBetweenTiles;
                xOffset = 0;
            }

            Tile newTile = (Tile)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);
            newTile.ID = tilesCreated;
            newTile.tilesPerRow = tilesPerRow;
            tilesAll[tilesCreated] = newTile;
        }
        AssignMines();
    }

    void AssignMines()
    {
        tilesUnmined = new List<Tile>(tilesAll);

        tilesMined = new List<Tile>();

        for(int minesAssigned = 0; minesAssigned < numberOfMines; minesAssigned++)
        {
            Tile currentTile = tilesUnmined[Random.Range(0, tilesUnmined.Count)];

            currentTile.GetComponent<Tile>().isMined = true;

            tilesMined.Add(currentTile);

            tilesUnmined.Remove(currentTile);
        }
    }

    IEnumerator gridUpdate()
    {
        while (true)
        {
            yield return null;

            //Player.GetComponentInChildren<TextMesh>().text = "Mines Left : " + minesRemaining;

            if (state == "inGame")
            {
                if(minesRemaining == 0 && minesMarkedCorrectly == numberOfMines)
                {
                    FinishGame();
                }
            }

            Vector3 pos = GameObject.Find("Player").transform.position;
            pos.z += 5.0f;
            RestartMenu.transform.position = pos;
        }
    }

    void FinishGame()
    {
        state = "gameWon";

        foreach (Tile currentTile in tilesAll)
        {
            if(currentTile.state == "Idle" && !currentTile.isMined)
            {
                currentTile.UncoverTileExternal();
            }
        }

        foreach(Tile currentTile in GameObject.Find("Grid").GetComponent<Grid>().tilesMined)
        {
            if(currentTile.state == "Flagged")
            {
                currentTile.SetFlag();
            }
        }

        popRestartMenu();
    }

    public void popRestartMenu()
    {
        if(state == "gameWon")
        {
            RestartMenu.transform.Find("Message").GetComponent<TextMesh>().text = "YOU WIN!";
        }
        else if (state == "gameOver")
        {
            RestartMenu.transform.Find("Message").GetComponent<TextMesh>().text = "YOU LOSE!";
        }
        RestartMenu.SetActive(true);
    }

    void Restart()
    {
        SceneManager.LoadScene("Scene01");
    }

    void OnGUI()
    {
        //GUI.Box(new Rect(10, 10, 100, 50), state);

        GUI.Box(new Rect(1000, 10, 100, 50), Input.GetAxis("Fire1").ToString() );

        if (state == "inGame")
        {
            GUI.Box(new Rect(10, 10, 200, 50), "Mines Left : " + minesRemaining);
        }
        else if(state == "gameOver")
        {
            GUI.Box(new Rect(10, 10, 200, 50), "LOSE!");

            if( GUI.Button(new Rect(10,70,200,50), "Restart"))
            {
                Restart();
            }
        }
        else if(state == "gameWon")
        {
            GUI.Box(new Rect(10, 10, 200, 50), "WIN!");

            if (GUI.Button(new Rect(10, 70, 200, 50), "Restart"))
            {
                Restart();
            }
        }
    }
}
