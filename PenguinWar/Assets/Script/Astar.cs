using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    int tilesPerRow;

    public newGrid grid;

    List<Tile_> OpenList = new List<Tile_>();
    HashSet<Tile_> ClosedList = new HashSet<Tile_>();

    //public List<Tile_> PathList = new List<Tile_>();
    
    void Awake()
    {
        grid = GameObject.Find("newGrid").GetComponent<newGrid>();
        tilesPerRow = grid.tilesPerRow;
    }

    public void PathFinding(Tile_ start, Tile_ target)
    {    
        OpenList.Add(start); // OpenList[0] is start

        while (OpenList.Count > 0)
        {
            //Tile_ current = OpenList.Find(Iter => Iter.fCost == (OpenList.Min<Tile_>(I => I.fCost)));// lowest F // FirstTime Only One

            Tile_ current = OpenList[0];
            for (int i=0; i < OpenList.Count; i++)
            {
                if (OpenList[i].fCost < current.fCost || OpenList[i].fCost == current.fCost && OpenList[i].H < current.H)
                    current = OpenList[i];
            }

            OpenList.Remove(current);
            ClosedList.Add(current);

            if (current == target)
            {
                RetracePath(start, target);
                return;
            }

            foreach (Tile_ neibour in GetNeibours(current))
            {
                if (neibour.gameObject.tag != "Walkable" || ClosedList.Contains(neibour))
                {
                    continue;
                }

                int newMoveCost = current.G + GetG(current, neibour);

                if (newMoveCost < neibour.G || !OpenList.Contains(neibour))
                {
                    neibour.G = newMoveCost;
                    neibour.H = GetH(neibour, target);
                    neibour.parent = current;

                    if (!OpenList.Contains(neibour))
                    {
                        OpenList.Add(neibour);
                    }
                }
            }
        }
    }

    void RetracePath(Tile_ start, Tile_ end)
    {
        Tile_ current = end;

        while (current != start)
        {
            this.GetComponentInChildren<PenguinUnit>().PathList.Add(current);
            //PathList.Add(current);
            current = current.parent;
        }
        this.GetComponentInChildren<PenguinUnit>().PathList.Reverse();
        //PathList.Reverse();
        //grid.path = PathList;
        //grid.path = this.GetComponentInChildren<PenguinUnit>().PathList;
    }

    int GetG(Tile_ A, Tile_ B)
    {
        return (int)(Vector3.Distance(A.transform.position, B.transform.position) * 10);
    }

    int GetH(Tile_ n, Tile_ t)
    {
        return 10 * (int)(Mathf.Abs(n.transform.position.x - t.transform.position.x) + Mathf.Abs(n.transform.position.z - t.transform.position.z));
    }

    private bool inBounds(List<Tile_> inputtilesAll, int targetID)
    {
        if (targetID < 0 || targetID >= inputtilesAll.Count)
            return false;
        else
            return true;
    }

    /*
    private bool inBounds(Tile_[] inputtilesAll, int targetID)
    {
        if (targetID < 0 || targetID >= inputtilesAll.Length)
            return false;
        else
            return true;
    }*/

    private bool checkTag(Tile_ checker)
    {
        if (checker.gameObject.tag == "Walkable" || checker.gameObject.tag == "Target")
            return true;
        else
            return false;
    }

    List<Tile_> GetNeibours(Tile_ tile)
    {
        List<Tile_> neibours = new List<Tile_>();

        //up
        if (inBounds(grid.tilesAll, tile.ID + tilesPerRow) && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID + tilesPerRow].ID) && checkTag(grid.tilesAll[tile.ID + tilesPerRow]))
            neibours.Add(grid.tilesAll[tile.ID + tilesPerRow]);

        //low
        if (inBounds(grid.tilesAll, tile.ID - tilesPerRow) && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID - tilesPerRow].ID) && checkTag(grid.tilesAll[tile.ID - tilesPerRow]))
            neibours.Add(grid.tilesAll[tile.ID - tilesPerRow]);

        //left
        if (inBounds(grid.tilesAll, tile.ID - 1) && tile.ID % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID - 1].ID) && checkTag(grid.tilesAll[tile.ID - 1]))
            neibours.Add(grid.tilesAll[tile.ID - 1]);

        //right
        if (inBounds(grid.tilesAll, tile.ID + 1) && (tile.ID + 1) % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID + 1].ID) && checkTag(grid.tilesAll[tile.ID + 1]))
            neibours.Add(grid.tilesAll[tile.ID + 1]);

        //upRight
        if (inBounds(grid.tilesAll, tile.ID + tilesPerRow + 1) && (tile.ID + 1) % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID + tilesPerRow + 1].ID) && checkTag(grid.tilesAll[tile.ID + tilesPerRow + 1]))
            neibours.Add(grid.tilesAll[tile.ID + tilesPerRow + 1]);

        //upLeft
        if (inBounds(grid.tilesAll, tile.ID + tilesPerRow - 1) && tile.ID % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID + tilesPerRow - 1].ID) && checkTag(grid.tilesAll[tile.ID + tilesPerRow - 1]))
            neibours.Add(grid.tilesAll[tile.ID + tilesPerRow - 1]);

        //lowRight
        if (inBounds(grid.tilesAll, tile.ID - tilesPerRow + 1) && (tile.ID + 1) % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID - tilesPerRow + 1].ID) && checkTag(grid.tilesAll[tile.ID - tilesPerRow + 1]))
            neibours.Add(grid.tilesAll[tile.ID - tilesPerRow + 1]);

        //lowLeft
        if (inBounds(grid.tilesAll, tile.ID - tilesPerRow - 1) && tile.ID % tilesPerRow != 0 && !OpenList.Exists(I => I.ID == grid.tilesAll[tile.ID - tilesPerRow - 1].ID) && checkTag(grid.tilesAll[tile.ID - tilesPerRow - 1]))
            neibours.Add(grid.tilesAll[tile.ID - tilesPerRow - 1]);

        return neibours;
    }
}