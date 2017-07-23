using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile_ : MonoBehaviour
{
    private bool targeted = false;

    public int G;
    public int H;
    public int ID;
    
    public Renderer render;
    public TextMesh displayText;
    public Collider myCollider;
    public Tile_ parent;

    public int fCost
    {
        get
        {
            return G + H;
        }
    }

    newGrid grid;
    
    void Start()
    {
        grid = GameObject.Find("newGrid").GetComponent<newGrid>();
        this.gameObject.name = "Tile" + ID.ToString();        
    }

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.T))
        {
            GameObject finder = GameObject.Find("PENGUIN" + grid.currentClicked);

            if (finder)
            {
                PenguinUnit peng = finder.GetComponent<PenguinUnit>();
                peng.myTarget = this;
                this.render.material = Resources.Load("Material/TileMaterialTarget") as Material;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Z)) // TEST
        {
            grid.CreatePenguinA(new Vector3(this.transform.position.x, this.transform.position.y + 0.05f, this.transform.position.z));
        }

        else if(Input.GetKeyDown(KeyCode.U))
        {
            this.gameObject.tag = "UnWalkable";
        }

        else if(gameObject.tag == "Walkable")
            render.material = Resources.Load("Material/TileMaterialRed") as Material;
    }

    private void OnMouseExit()
    {
        if (gameObject.tag == "Walkable")
            render.material = Resources.Load("Material/TileMaterialDefault") as Material;
    }

    void Temp()
    {
        RaycastHit hit = GameObject.Find("Player").GetComponentInChildren<Cam>().rayHit;

        if (hit.collider == this.gameObject.GetComponent<Collider>())
        {
            render.material = Resources.Load("Material/TileMaterialLightUp") as Material;
        }

        else
        {
            render.material = Resources.Load("Material/TileMaterialDefault") as Material;
        }
    }
}