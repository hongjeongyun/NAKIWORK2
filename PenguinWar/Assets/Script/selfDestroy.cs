using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour {

    private float fTime;

    void Start () {
        fTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        fTime += Time.deltaTime * 1.0f;

        if(fTime > 1.0f)
        {
            fTime = 0;
            Destroy(this.gameObject);
        }


    }
}
