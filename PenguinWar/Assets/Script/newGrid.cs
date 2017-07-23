using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newGrid : MonoBehaviour
{
    public bool isOver = false;

    public int currentClicked = -1;
    public int tilesPerRow = 0;
    public int numberOfTiles = 0;
    public int tilesUncoverd = 0;
    public int numberOfPenguins = 0;

    public float distanceBetweenTiles = 1.0f;

    public List<Tile_> tilesAll;
    
    public List<Tile_> path;

    public List<PenguinUnit> PenguinAll;

    void Awake()
    {
        isOver = false;
        tilesAll = CreateTiles();
    }

    private void Start()
    {
        currentClicked = -1;

        tilesAll.Find(x => x.ID == 0).gameObject.tag = "IndexZero";
        tilesAll.Find(x => x.ID == numberOfTiles - 1).gameObject.tag = "IndexEnd";
        
        CreateAzitNew(0, "Azit","P1");
        CreateAzitNew(numberOfTiles - 1, "Azit", "P2");
    }

    void Update()
    {
        foreach (Tile_ current in tilesAll)
        {
            if (current.gameObject.tag == "UnWalkable")
            {
                current.render.material = Resources.Load("Material/TileMaterialOnstacle") as Material;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject finder = GameObject.Find("PENGUIN" + currentClicked);
            PenguinUnit peng = finder.GetComponent<PenguinUnit>();

            for (int i = 0; i < peng.PathList.Count; i++)
            {
                peng.myBaseTile.render.material = Resources.Load("Material/Temp2") as Material;
                peng.PathList[i].render.material = Resources.Load("Material/Temp2") as Material;
            }

            Debug.Log("P_OK");
        }
    }

    public List<Tile_> CreateTiles()
    {
        GameObject go = (GameObject)Resources.Load("preFab/newTile");
        Tile_ tilePrefab = go.GetComponent<Tile_>();
        List<Tile_> tempList = new List<Tile_>();

        float xOffset = 0.0f;
        float zOffset = 0.0f;
        
        for (int tileCreated = 0; tileCreated < numberOfTiles; tileCreated++)
        {
            xOffset += distanceBetweenTiles;

            if (tileCreated % tilesPerRow == 0)
            {
                zOffset += distanceBetweenTiles;
                xOffset = 0;
            }

            Tile_ newTile = (Tile_)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);
            newTile.ID = tileCreated;
            //newTile.tilesPerRow = tilesPerRow;
            tempList.Add(newTile);
        }
        return tempList;
    }

    public void CreatePenguinA(Vector3 pos) // TEST
    {
        GameObject newPenguin = (GameObject)Instantiate(Resources.Load("preFab/PenguinA") as GameObject, pos, transform.rotation);
        PenguinUnit peng = newPenguin.GetComponent<PenguinUnit>();
        peng.ID = numberOfPenguins;
        PenguinAll.Add(peng);
        numberOfPenguins += 1;
    }

    public void CreateAzitNew(int tileID, string Name, string tag) // TEST
    {
        Tile_ AzitTile = tilesAll.Find(x => x.ID == tileID);
        Vector3 pos = AzitTile.transform.position;
        pos.y = AzitTile.transform.position.y + 0.5f;
        GameObject azit = (GameObject)Resources.Load("preFab/" + Name);
        azit.gameObject.tag = tag;
        Instantiate(azit, pos, azit.transform.rotation);
    }

    public void DestroyPenguin(PenguinUnit p)
    {
        Destroy(p);
    }

    //if use array
    //public Tile_[] tilesAll;
    //tilesAll = new Tile_[numberOfTiles];
    //tilesAll[tileCreated] = newTile;

    //if use
    //public Tile_ tilePrefab; -> inspector assign, do not Resource.Load() + GetComponent<>()
}
