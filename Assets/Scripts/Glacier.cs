using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Glacier : MonoBehaviour
{
    public float LifeTime;
    float timer;
    public int count;
    [SerializeField] MeshRenderer myMeshRenderer;
    [SerializeField] GameObject BrokenGlacier;
    public bool isPicked, isLast;
    [SerializeField] GameObject[] ChildObjects;

    private void Start()
    {
        timer = LifeTime / 21;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isLast)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && count <= 20)
            {
                if (count == 0)
                {
                    myMeshRenderer.enabled = false;
                    myMeshRenderer.GetComponent<Collider>().enabled = false;
                    BrokenGlacier.SetActive(true);
                }
                else if (count == 10)
                {
                    GameManager.Instance.m_ShipController.ShipsGlaciers[1].isLast = true;

                }
                else if (count == 15)
                {
                    if (GameManager.Instance.m_ShipController.ShipsGlaciers.Count == 3)
                    {
                        GameManager.Instance.m_ShipController.m_Animator.enabled = true;
                        GameManager.Instance.m_ShipController.transform.DOMoveY(-15, 5);
                        GameManager.Instance.m_ShipController.m_Animator.SetTrigger("Sink");
                    }
                }
                timer = LifeTime / 21;

                ChildObjects[count].GetComponent<Rigidbody>().useGravity = true;
                ChildObjects[count].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Random.Range(-1.5f, -1f));
                count++;
                if (count > 20)
                {
                    GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(this);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void ActivateObject(Transform emptySlot)
    {
        isPicked = true;
        transform.DOMove(emptySlot.position, .8f);
    }

    public void BreakAll()
    {
        isLast = false;
        for (int i = count; i <= 20; i++)
        {
            ChildObjects[i].GetComponent<Rigidbody>().useGravity = true;
            ChildObjects[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Random.Range(-1.5f, -1f));
        }
        GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(this);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isPicked)
        {
            if (other.CompareTag("Hazard"))
            {
                other.GetComponent<Collider>().enabled = false;
                GameManager.Instance.m_ShipController.BreakLastGlacier();
                other.GetComponent<Rigidbody>().useGravity = true;
                other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 2f);
                GameManager.Instance.ShakeCamera();
                Destroy(other.gameObject, 2f);
            }
            else if (other.CompareTag("FinishLine"))
            {
                GameManager.Instance.isGameOver = true;
                StartCoroutine(GameManager.Instance.WaitAndGameWin());
                GameManager.Instance.m_CollisionDetection.Player.transform.DOMoveZ(GameManager.Instance.m_CollisionDetection.Player.transform.position.z + 5, 1f).OnComplete(() =>
                {

                });
            } 
        }
    }
}
