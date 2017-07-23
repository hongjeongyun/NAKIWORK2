using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Azit : MonoBehaviour {

	void Start ()
    {
        StartCoroutine(AzitUpdate());
	}

    IEnumerator AzitUpdate()
    {
        while(true)
        {
            yield return null;
            
			RaycastHit hit = GameObject.Find("Player").GetComponentInChildren<Cam>().rayHit;

            if(hit.collider == this.gameObject.GetComponent<Collider>())
            {
				if(Input.GetKeyDown(KeyCode.A))
                {
                    Debug.Log("OK");
                }
            }
        }
    }

    void CreatePenguinUnit(string unitName)
    {
		GameObject unit = (GameObject)Instantiate(Resources.Load("preFab/" + unitName), this.transform.position, this.transform.rotation) as GameObject;
        if(unit) return;
    }
}
