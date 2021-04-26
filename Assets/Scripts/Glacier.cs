using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glacier : MonoBehaviour
{
    [SerializeField] float LifeTime;
    float timer;
    int currentState;
    [SerializeField] Material[] GlacierBreaks;
    [SerializeField] MeshRenderer myMeshRenderer;
    public bool IsActive;

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
                    myMeshRenderer.material = GlacierBreaks[0];
                    break;
                case 2:
                    myMeshRenderer.material = GlacierBreaks[1];
                    break;
                case 3:
                    IsActive = false;
                    Destroy(gameObject);
                    break;
            }
            StartCoroutine(WaitAndBreak(LifeTime / 3));
        }
    }

    public void ActivateObject(Transform emptySlot)
    {
        GetComponent<Collider>().enabled = false;
        IsActive = true;
        transform.position = emptySlot.position;
        StartCoroutine(WaitAndBreak(LifeTime / 3));
    }
}
