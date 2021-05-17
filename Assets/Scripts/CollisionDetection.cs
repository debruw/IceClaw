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
    public Transform RightIKTarget;
    [SerializeField] ClawController clawController;
    [SerializeField] Transform HazardPoint1, HazardPoint2;
    public bool isEmpty;

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
    Vector3 middlePoint;
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

                middlePoint = new Vector3((other.transform.position.x - shipController.transform.position.x) / 2, 0, (other.transform.position.z - shipController.transform.position.z) / 2);

                RightIKTarget.transform.position = new Vector3(other.transform.position.x, RightIKTarget.position.y, other.transform.position.z);
                RightIKTarget.DOMoveX(EmptySlots[nextSlot].transform.position.x + middlePoint.x, .8f).OnComplete(() =>
                {
                    RightIKTarget.transform.DOLocalMoveX(GameManager.Instance.m_ClawController.MyJoystick.startPosition.x, .5f);
                    RightIKTarget.transform.DOLocalMoveZ(GameManager.Instance.m_ClawController.MyJoystick.startPosition.z, .5f);
                    clawController.isActive = true;
                    isEmpty = true;
                });
                RightIKTarget.DOMoveZ(EmptySlots[nextSlot].transform.position.z + middlePoint.z, .8f);                

                shipController.transform.DOMoveX(shipController.transform.position.x + middlePoint.x, .8f);
                shipController.transform.DOMoveZ(shipController.transform.position.z + middlePoint.z, .8f);

                other.GetComponent<Glacier>().ActivateObject(EmptySlots[nextSlot].transform.position + middlePoint);

                StartCoroutine(MovePlayerForward());
                shipController.ShipsGlaciers.Add(other.GetComponent<Glacier>());
                other.transform.parent = shipController.transform;
                SoundManager.Instance.playSound(SoundManager.GameSounds.GlacierMove);
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
                    RightIKTarget.transform.DOLocalMoveX(GameManager.Instance.m_ClawController.MyJoystick.startPosition.x, .5f);
                    RightIKTarget.transform.DOLocalMoveZ(GameManager.Instance.m_ClawController.MyJoystick.startPosition.z, .5f);
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
                    RightIKTarget.transform.DOLocalMoveX(GameManager.Instance.m_ClawController.MyJoystick.startPosition.x, .5f);
                    RightIKTarget.transform.DOLocalMoveZ(GameManager.Instance.m_ClawController.MyJoystick.startPosition.z, .5f);
                    clawController.isActive = true;
                    isEmpty = true;
                });
                RightIKTarget.DOMoveZ(HazardPoint2.position.z, .5f);
            }
            SoundManager.Instance.playSound(SoundManager.GameSounds.GlacierMove);
        }
    }
}
