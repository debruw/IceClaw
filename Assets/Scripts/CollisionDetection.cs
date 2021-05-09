using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] ShipController shipController;
    public GameObject Player;
    int nextSlot = 1;
    public GameObject[] EmptySlots;
    float MoveStep;
    [SerializeField] Transform RightIKTarget;
    [SerializeField] ClawController clawController;
    [SerializeField] Transform HazardPoint1, HazardPoint2;

    private void Start()
    {
        MoveStep = Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position);
    }

    IEnumerator MovePlayerForward()
    {
        yield return new WaitForSeconds(.3f);
        Player.transform.DOLocalMoveZ(
            Player.transform.localPosition.z + (MoveStep / 2), .5f            
            ).OnComplete(() =>
            {
                EmptySlots[nextSlot].transform.DOLocalMoveZ(EmptySlots[nextSlot].transform.localPosition.z + MoveStep, 0);
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glacier"))
        {
            if (!other.GetComponent<Glacier>().isPicked)
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
                shipController.ShipsGlaciers.Add(other.GetComponent<Glacier>());
                other.transform.parent = shipController.transform;
            }
        }
        else if (other.CompareTag("Hazard"))
        {

        }
    }
}
