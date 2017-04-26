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
            RaycastHit hit = GameObject.Find("Player").GetComponentInChildren<Cam>().rayHit; //ok
            if (hit.collider == this.gameObject.GetComponent<Collider>()) //ok
            {
                Debug.Log("HIT_OK");
            }
        }
    }
}