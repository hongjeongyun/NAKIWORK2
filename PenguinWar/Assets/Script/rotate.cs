using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(myUpdate());	
	}

    IEnumerator myUpdate()
    {
        while (true)
        {
            yield return null;
            Quaternion rot = this.transform.rotation;
            rot.y = (rot.y += 360.0f * Time.deltaTime) * 0.2f;

            if (rot.y > 360.0f)
            {
                rot.y = 0;
                this.transform.rotation = Quaternion.identity;
            }

            Vector3 r = new Vector3(0, rot.y, 0);

            this.transform.Rotate(r);
        }
    }
}
