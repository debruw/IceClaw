using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollisionDetection : MonoBehaviour
{
    public GameObject GlacierParent;
    int nextSlot = 1;
    public GameObject[] EmptySlots;
    float MoveStep;
    [SerializeField] Transform RightIKTarget;
    [SerializeField] ClawController clawController;

    private void Start()
    {
        MoveStep = Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position);
    }

    IEnumerator MovePlayerForward()
    {
        yield return new WaitForSeconds(.3f);
        GlacierParent.transform.DOMoveZ(
            GlacierParent.transform.position.z - (
                                                    MoveStep / 2),
            .5f
            ).OnComplete(() =>
            {
                EmptySlots[nextSlot].transform.DOMoveZ(EmptySlots[nextSlot].transform.position.z + MoveStep, 0);
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glacier"))
        {
            if (!other.GetComponent<Glacier>().IsActive)
            {
                if (nextSlot == 0)
                {
                    nextSlot = 1;
                }
                else
                {
                    nextSlot = 0;
                }
                clawController.isActive = false;
                RightIKTarget.DOMoveX(EmptySlots[nextSlot].transform.position.x, .4f).OnComplete(() =>
                {
                    clawController.isActive = true;
                });
                RightIKTarget.DOMoveZ(EmptySlots[nextSlot].transform.position.z, .4f);
                other.GetComponent<Glacier>().ActivateObject(EmptySlots[nextSlot].transform);

                StartCoroutine(MovePlayerForward());
            }
        }
        else if (other.CompareTag("Hazard"))
        {

        }
        else if (other.CompareTag("FinishLine"))
        {
            GlacierParent.transform.DOMoveZ(GlacierParent.transform.position.z - 3, 1f);
        }
    }
}
