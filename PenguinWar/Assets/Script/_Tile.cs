using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Tile : MonoBehaviour {

	void Start ()
    {
        StartCoroutine(_TileUpdate());
	}
	
	IEnumerator _TileUpdate()
    {
        while(true)
        {
            yield return null;
            Debug.Log("TILE_OK");
        }
    }
}
