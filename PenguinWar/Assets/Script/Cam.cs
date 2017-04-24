using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour {

    public RaycastHit rayHit;

    private MeshRenderer[] meshs;

    void Start ()
    {
        meshs = this.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer m in meshs)
        {
            if (m.tag == "Player")
            {
                m.material.color = Color.red;
            }
        }

        StartCoroutine(myUpdate());
    }

    IEnumerator myUpdate()
    {
        while(true)
        {
            yield return null;

            Ray CamRay = new Ray(this.transform.position, this.transform.forward);

            if (Physics.Raycast(CamRay, out rayHit))
            {
                meshs = this.GetComponentsInChildren<MeshRenderer>();

                foreach(MeshRenderer m in meshs )
                {
                    if (m.tag == "Player")
                    {
                        m.material.color = Color.blue;

                        if (rayHit.collider.tag == "YES")
                        {
                            if(Input.GetButtonDown("Fire1"))
                            {
                                SceneManager.LoadScene("Scene01");
                                yield return null;
                            }
                        }
                        else if (rayHit.collider.tag == "NO")
                        {
                            if (Input.GetButtonDown("Fire1"))
                            {
                                Application.Quit();
                                yield return null;
                            }
                        }
                    }
                }
            }
            else
            {
                meshs = this.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer m in meshs)
                {
                    if (m.tag == "Player")
                    {
                        m.material.color = Color.red;
                    }
                }
            }

            Debug.DrawRay(CamRay.origin, CamRay.direction * 100.0f, Color.red);
        }
    }
}
