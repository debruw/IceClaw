using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GlaciersManager : MonoBehaviour
{
    [SerializeField] List<Glacier> AllGlaciers;
    private void Start()
    {
        foreach (Glacier gl in AllGlaciers)
        {
            gl.timer = gl.LifeTime / 21;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Glacier gl in AllGlaciers)
        {
            if (gl.isLast)
            {
                gl.timer -= Time.deltaTime;
                if (gl.timer <= 0 && gl.count <= 20)
                {
                    if (gl.count == 0)
                    {
                        gl.myMeshRenderer.enabled = false;
                        gl.myMeshRenderer.GetComponent<Collider>().enabled = false;
                    }
                    else if (gl.count == 10)
                    {
                        GameManager.Instance.m_ShipController.ShipsGlaciers[1].isLast = true;

                    }
                    else if (gl.count == 15)
                    {
                        if (GameManager.Instance.m_ShipController.ShipsGlaciers.Count == 3)
                        {
                            GameManager.Instance.m_ShipController.m_Animator.enabled = true;
                            GameManager.Instance.m_ShipController.transform.DOMoveY(-15, 5);
                            GameManager.Instance.m_ShipController.m_Animator.SetTrigger("Sink");
                        }
                    }
                    gl.timer = gl.LifeTime / 21;

                    gl.ChildObjects[gl.count].GetComponent<Rigidbody>().useGravity = true;
                    gl.ChildObjects[gl.count].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Random.Range(-1.5f, -1f));
                    gl.count++;
                    if (gl.count > 20)
                    {
                        GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(gl);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
