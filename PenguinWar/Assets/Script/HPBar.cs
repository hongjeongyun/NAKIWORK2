using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {

    public Transform target;

    void Start ()
    {
        StartCoroutine(thisUpdate());
	}

    IEnumerator thisUpdate()
    {
        while(true)
        {
            yield return null;
            this.transform.LookAt(target);
        }
    }
}
