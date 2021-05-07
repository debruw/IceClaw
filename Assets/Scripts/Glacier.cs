using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Glacier : MonoBehaviour
{
    [SerializeField] float LifeTime;
    float timer;
    int currentState;
    [SerializeField] Material[] GlacierBreaks;
    [SerializeField] MeshRenderer myMeshRenderer;
    [SerializeField] GameObject BrokenGlacier;
    public bool IsActive;
    [SerializeField] GameObject[] FallOneObjs;
    [SerializeField] GameObject[] FallTwoObjs;
    [SerializeField] GameObject[] DestroyObjs;

    private void Start()
    {
        timer = LifeTime;
        if (IsActive)
        {
            StartCoroutine(WaitAndBreak(LifeTime / 3));
        }
    }

    private void Update()
    {
        if (IsActive)
        {
            timer -= Time.deltaTime;
        }
    }

    IEnumerator WaitAndBreak(float time)
    {
        if (timer > 0 && IsActive)
        {
            yield return new WaitForSeconds(time);
            currentState++;
            switch (currentState)
            {
                case 1:
                    BrokenGlacier.SetActive(true);
                    myMeshRenderer.enabled = false;
                    FallOne();
                    break;
                case 2:
                    FallTwo();
                    break;
                case 3:
                    StartCoroutine(DestroyGlacier());
                    break;
            }
            StartCoroutine(WaitAndBreak(LifeTime / 3));
        }
    }

    public void ActivateObject(Transform emptySlot)
    {
        GetComponent<Collider>().enabled = false;
        IsActive = true;
        transform.DOMove(emptySlot.position, .4f);
               
        StartCoroutine(WaitAndBreak(LifeTime / 3));
    }

    void FallOne()
    {
        myMeshRenderer.material = GlacierBreaks[0];
        for (int i = 0; i < FallOneObjs.Length; i++)
        {
            FallOneObjs[i].GetComponent<Rigidbody>().useGravity = true;
            FallOneObjs[i].GetComponent<MeshCollider>().enabled = true;
        }
    }

    void FallTwo()
    {
        myMeshRenderer.material = GlacierBreaks[1];
        for (int i = 0; i < FallTwoObjs.Length; i++)
        {
            FallTwoObjs[i].GetComponent<Rigidbody>().useGravity = true;
            FallTwoObjs[i].GetComponent<MeshCollider>().enabled = true;
        }
    }

    IEnumerator DestroyGlacier()
    {
        IsActive = false;
        for (int i = 0; i < DestroyObjs.Length; i++)
        {
            DestroyObjs[i].GetComponent<Rigidbody>().useGravity = true;
            DestroyObjs[i].GetComponent<MeshCollider>().enabled = true;
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive && other.CompareTag("Hazard"))
        {
            timer = 0;
        }
    }
}
