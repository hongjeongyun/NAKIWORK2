using UnityEngine;
using System.Collections;

public class FlagRotate : MonoBehaviour {

    public float rotSpeed;

    private float y = 0;

    void Start () {
        StartCoroutine(myUpdate());
	}

    IEnumerator myUpdate()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            this.transform.rotation = Quaternion.AngleAxis(y += (rotSpeed * Time.deltaTime), Vector3.up);
        }
    }
}
