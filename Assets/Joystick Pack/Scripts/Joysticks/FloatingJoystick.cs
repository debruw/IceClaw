using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FloatingJoystick : Joystick
{
    public GameObject RightIKTarget;
    Vector3 startPosition;

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