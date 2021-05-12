using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoystick MyJoystick;
    public float speed;
    Vector2 LastJoystickPos;
    float bodyStartRotationX;
    [SerializeField] GameObject Arm1, Arm2, Arm3, Body;
    float heading;
    public bool isActive = true;
    public Vector2 JoystickVariables;

    // Start is called before the first frame update
    void Start()
    {
        bodyStartRotationX = Body.transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isActive)
        {
            JoystickVariables.x = Mathf.Clamp(MyJoystick.Vertical, 0, 1);
            JoystickVariables.y = Mathf.Clamp(-MyJoystick.Horizontal, -1, +1);
            heading = Mathf.Atan2(JoystickVariables.x, JoystickVariables.y);
            //Debug.Log(Mathf.Clamp(heading * Mathf.Rad2Deg, 0, 100)); 
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isActive)
        {
            if (Input.GetMouseButton(0))
            {
                Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.Euler(bodyStartRotationX, 0f, Mathf.Clamp(heading * Mathf.Rad2Deg, 0, 179)), speed);
                MoveArms();
            }
        }
    }

    void MoveArms()
    {
        Arm1.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Arm1.transform.localEulerAngles), Quaternion.Euler(0, 0, 40 + JoystickVariables.x * 50), speed);//40-90 pozitif
        Arm2.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Arm2.transform.localEulerAngles), Quaternion.Euler(0, 0, ((1 - JoystickVariables.x) * 40) + ((1 - JoystickVariables.x) * 90f)), speed);//40-120 negatif
        Arm3.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Arm3.transform.localEulerAngles), Quaternion.Euler(0, 0, ((1 - JoystickVariables.x) * 50)), speed);//0-50 negatif   
    }

    public IEnumerator MoveBack()
    {
        while (JoystickVariables.x > 0)
        {
            yield return new WaitForSeconds(.01f);
            JoystickVariables.x -= Time.deltaTime;
            JoystickVariables.y = 0;
            heading = Mathf.Atan2(JoystickVariables.x, JoystickVariables.y);
            MoveArms();
        }
        JoystickVariables.x = 0;
        isActive = true;
        GameManager.Instance.m_CollisionDetection.isEmpty = true;
    }
}
