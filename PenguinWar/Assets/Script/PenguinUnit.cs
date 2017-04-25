using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinUnit : MonoBehaviour
{
	void Start ()
    {
        StartCoroutine(PenguinUnitUpdate());
	}

    IEnumerator PenguinUnitUpdate()
    {
        while(true)
        {
            yield return null;
            //Debug.Log("PENGUIN_IS_ALIVE");
            RaycastHit hit = GameObject.Find("Player").GetComponentInChildren<Cam>().rayHit;

            if(hit.collider == null)
            {
                Debug.Log("OK");
            }


            if (hit.collider == this.gameObject.GetComponent<Collider>())
            {
                //Debug.Log("OK");
            }
        }
    }
}