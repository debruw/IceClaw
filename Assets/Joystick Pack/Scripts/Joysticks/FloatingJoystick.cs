using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FloatingJoystick : Joystick
{
    public GameObject RightIKTarget;
    public Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        RightIKTarget = GameManager.Instance.m_CollisionDetection.RightIKTarget.gameObject;
        startPosition = RightIKTarget.transform.localPosition;        
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        if (GameManager.Instance.currentLevel == 1 || GameManager.Instance.currentLevel == 2)
        {
            GameManager.Instance.Tutorial.SetActive(false);
            GameManager.Instance.m_ShipController.speed = 2;
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        if (GameManager.Instance.m_CollisionDetection.isEmpty)
        {
            RightIKTarget.transform.DOLocalMoveX(startPosition.x, .5f);
            RightIKTarget.transform.DOLocalMoveZ(startPosition.z, .5f);
        }
        base.OnPointerUp(eventData);
    }
}