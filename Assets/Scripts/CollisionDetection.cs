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
    bool isEmpty;

    private void Start()
    {
        isEmpty = true;
        MoveStep = Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position);
    }

    IEnumerator MovePlayerForward()
    {
        yield return new WaitForSeconds(.3f);
        Player.transform.DOLocalMoveZ(Player.transform.localPosition.z + (MoveStep / 2), 1.5f);
        EmptySlots[nextSlot].transform.DOLocalMoveZ(EmptySlots[nextSlot].transform.localPosition.z + MoveStep, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted || !isEmpty)
        {
            return;
        }
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
                isEmpty = false;
                RightIKTarget.DOMoveX(EmptySlots[nextSlot].transform.position.x, .8f).OnComplete(() =>
                {
                    clawController.isActive = true;
                    isEmpty = true;
                });
                RightIKTarget.DOMoveZ(EmptySlots[nextSlot].transform.position.z, .8f);
                other.GetComponent<Glacier>().ActivateObject(EmptySlots[nextSlot].transform);

                StartCoroutine(MovePlayerForward());
                shipController.ShipsGlaciers.Add(other.GetComponent<Glacier>());
                other.transform.parent = shipController.transform;
            }
        }
        else if (other.CompareTag("Hazard"))
        {
            clawController.isActive = false;
            other.GetComponent<Collider>().enabled = false;
            isEmpty = false;
            if (Vector3.Distance(HazardPoint1.position, other.transform.position) < Vector3.Distance(HazardPoint2.position, other.transform.position))
            {
                other.transform.DOMove(HazardPoint1.position, .5f);
                RightIKTarget.DOMoveX(HazardPoint1.position.x, .5f).OnComplete(() =>
                {
                    clawController.isActive = true;
                    isEmpty = true;
                });
                RightIKTarget.DOMoveZ(HazardPoint1.position.z, .5f);
            }
            else
            {
                other.transform.DOMove(HazardPoint2.position, .5f);
                RightIKTarget.DOMoveX(HazardPoint2.position.x, .5f).OnComplete(() =>
                {
                    clawController.isActive = true;
                    isEmpty = true;
                });
                RightIKTarget.DOMoveZ(HazardPoint2.position.z, .5f);
            }
        }
    }
}
