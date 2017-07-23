using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public bool isMined = false;

    public int ID;
    public int tilesPerRow;
    public int adjacentMines = 0;

    public string state = "Idle";

    public GameObject displayFlag;
    public GameObject displayText;
    public GameObject particleExplosion;

    public Collider selfCollider;

    public Tile tileUpper;
    public Tile tileLower;
    public Tile tileLeft;
    public Tile tileRight;

    public Tile tileUpperRight;
    public Tile tileUpperLeft;
    public Tile tileLowerRight;
    public Tile tileLowerLeft;

    public List<Tile> adjacentTiles = new List<Tile>();

    public Renderer r;

    Grid grid;
    
    void Start()
    {
        this.gameObject.name = "Tile" + ID.ToString();

        state = "Idle";

        grid = GameObject.Find("Grid").GetComponent<Grid>();
        

        if (inBounds(grid.tilesAll, ID + tilesPerRow))
            tileUpper = grid.tilesAll[ID + tilesPerRow];

        if (inBounds(grid.tilesAll, ID - tilesPerRow))
            tileLower = grid.tilesAll[ID - tilesPerRow];

        if (inBounds(grid.tilesAll, ID - 1) && ID % tilesPerRow != 0)
            tileLeft = grid.tilesAll[ID - 1];

        if (inBounds(grid.tilesAll, ID + 1) && (ID + 1) % tilesPerRow != 0)
            tileRight = grid.tilesAll[ID + 1];

        if (inBounds(grid.tilesAll, ID + tilesPerRow + 1) && (ID+1) % tilesPerRow != 0)
            tileUpperRight = grid.tilesAll[ID + tilesPerRow + 1];

        if(inBounds(grid.tilesAll, ID + tilesPerRow - 1) && ID % tilesPerRow != 0)
            tileUpperLeft = grid.tilesAll[ID + tilesPerRow - 1];

        if(inBounds(grid.tilesAll, ID - tilesPerRow + 1) && (ID+1) % tilesPerRow != 0)
            tileLowerRight = grid.tilesAll[ID - tilesPerRow + 1];

        if(inBounds(grid.tilesAll, ID - tilesPerRow - 1) && ID % tilesPerRow != 0)
            tileLowerLeft = grid.tilesAll[ID - tilesPerRow - 1];

        if (tileUpper)
            adjacentTiles.Add(tileUpper);

        if (tileLower)
            adjacentTiles.Add(tileLower);

        if (tileLeft)
            adjacentTiles.Add(tileLeft);

        if (tileRight)
            adjacentTiles.Add(tileRight);

        if (tileUpperRight)
            adjacentTiles.Add(tileUpperRight);

        if (tileUpperLeft)
            adjacentTiles.Add(tileUpperLeft);

        if (tileLowerRight)
            adjacentTiles.Add(tileLowerRight);

        if (tileLowerLeft)
            adjacentTiles.Add(tileLowerLeft);

        CountMines();

        displayFlag.GetComponent<MeshRenderer>().enabled = false;
        displayText.GetComponent<MeshRenderer>().enabled = false;

        StartCoroutine(tileUpdate());

        AzitLoad(80, "P2Azit");
        AzitLoad(0, "P1Azit");
    }

    private bool inBounds(Tile[] inputList, int targetID)
    {
        if (targetID < 0 || targetID >= inputList.Length)
            return false;
        else
            return true;
    }

    void CountMines()
    {
        adjacentMines = 0;

        foreach (Tile currentTile in adjacentTiles)
            if (currentTile.isMined)
                adjacentMines += 1;

        displayText.GetComponent<TextMesh>().text = adjacentMines.ToString();

        if (adjacentMines <= 0)
            displayText.GetComponent<TextMesh>().text = "";
    }

    void move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 pos = hit.collider.transform.position;
            pos.y += 1.5f;
            GameObject.Find("Player").transform.position = pos;
        }
    }

    void AzitLoad(int setTileID, string Name)
    {
        if (ID == setTileID)
        {
            GameObject Azit = (GameObject)Resources.Load("preFab/" + Name);
            Vector3 pos = this.transform.position;
            pos.y = this.transform.position.y + 0.5f;
            Instantiate(Azit, pos, Azit.transform.rotation);
        }
    }

    IEnumerator tileUpdate()
    {
        while(true)
        {
            yield return null;

            RaycastHit hit = GameObject.Find("Player").GetComponentInChildren<Cam>().rayHit;

            if (hit.collider == this.gameObject.GetComponent<Collider>())
            {
                if (grid.state == "inGame")
                {
                    if (state == "Idle") 
                    {
                        r.material = Resources.Load("Material/TileMaterialLightUp") as Material;

                        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Fire1"))
                        {
                            UncoverTile();
                            Vector3 pos = hit.collider.transform.position;
                            pos.y += 1.5f;
                            GameObject.Find("Player").transform.position = pos;
                        }

                        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Fire2"))
                        {
                            SetFlag();
                        }

                        if (Input.GetKey(KeyCode.G) || Input.GetButton("Fire3"))
                        {
                            if(isMined == true)
                            {
                                r.material = Resources.Load("Material/TileMaterialRed") as Material;
                            }
                        }
                    }

                    else if (state == "Flagged")
                    {
                        r.material = Resources.Load("Material/TileMaterialLightUp") as Material;

                        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Fire1"))
                        {
                            UncoverTile();
                        }

                        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Fire2"))
                        {
                            SetFlag();
                        }
                    }
                }
            }

            else
            {
                if (grid.state == "inGame")
                {
                    if (state == "Idle" || state == "Flagged")
                    {
                        r.material = Resources.Load("Material/TileMaterialDefault") as Material;
                    }
                }
            }
        }
    }

    public void SetFlag()
    {
        if(state == "Idle")
        {
            state = "Flagged";
            displayFlag.GetComponent<MeshRenderer>().enabled = true;
            grid.minesRemaining -= 1;

            if(isMined)
            {
                grid.minesMarkedCorrectly += 1;
            }
        }
        else if(state == "Flagged")
        {
            state = "Idle";
            displayFlag.GetComponent<MeshRenderer>().enabled = false;
            grid.minesRemaining += 1;

            if (isMined)
            {
                grid.minesMarkedCorrectly -= 1;
            }
        }
    }

    private void UncoverAdjacentTiles()
    {
        foreach (Tile currentTile in adjacentTiles)
        {
            if (!currentTile.isMined && currentTile.state == "Idle" && currentTile.adjacentMines == 0)
            {
                currentTile.UncoverTile();
            }
            else if (!currentTile.isMined && currentTile.state == "Idle" && currentTile.adjacentMines > 0)
            {
                currentTile.UncoverTileExternal();
            }
        }
    }

    public void UncoverTileExternal()
    {
        state = "Uncovered";
        displayText.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<MeshRenderer>().material = Resources.Load("Material/TileMaterialUncovered") as Material;
        grid.tilesUncoverd += 1;
    }

    void UncoverTile()
    {
        if(!isMined)
        {
            state = "Uncovered";

            displayText.GetComponent<MeshRenderer>().enabled = true;

            this.GetComponent<MeshRenderer>().material = Resources.Load("Material/TileMaterialUncovered") as Material;

            grid.tilesUncoverd += 1;

            if (adjacentMines == 0)
            {
                UncoverAdjacentTiles();
            }
        }
        else
        {
            Explode();
        }
    }
    
    void Explode()
    {
        state = "Detonated";
        
        this.GetComponent<MeshRenderer>().material = Resources.Load("Material/TileMaterialDetonated") as Material;

        foreach(Tile currtentTile in grid.tilesMined)
        {
            currtentTile.ExplodeExternal();
        }

        grid.state = "gameOver";

        this.gameObject.GetComponent<Collider>().enabled = false;

        Instantiate(particleExplosion, this.transform.position, this.transform.rotation);
        
        grid.popRestartMenu();
    }

    void ExplodeExternal()
    {
        state = "Detonated";
        this.GetComponent<MeshRenderer>().material = Resources.Load("Material/TileMaterialDetonated") as Material;
    }

    /*
    void OnMouseOver() // Change Camera Tracking
    {
        if(state == "Idle")
        {
            r.material = Resources.Load("Material/TileMaterialLightUp") as Material;

            if (Input.GetMouseButtonDown(0))
            {
                UncoverTile();
            }
            if (Input.GetMouseButtonDown(1))
            {
                SetFlag();
            }
        }

        else if(state == "Flagged")
        {
            r.material = Resources.Load("Material/TileMaterialLightUp") as Material;

            if (Input.GetMouseButtonDown(1))
            {
                SetFlag();
            }
        }
    }*/

    /*
    void OnMouseExit()
    {
        if(state == "Idle" || state == "Flagged")
            r.material = Resources.Load("Material/TileMaterialDefault") as Material;
    }*/
}
