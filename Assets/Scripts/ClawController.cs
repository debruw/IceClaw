using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] FloatingJoystick MyJoystick;
    public float speed;

    Vector2 xClamp, zClamp;
    Vector3 startPos;
    [SerializeField] GameObject Arm1, Arm2, Arm3, Arm4, Body;
    float bodyStartRotationX;
    public bool isActive = true;

    private void Awake()
    {
        xClamp = new Vector2(transform.localPosition.x - 8, transform.localPosition.x + 5);
        zClamp = new Vector2(transform.localPosition.z - 5, transform.localPosition.z + 5);
        bodyStartRotationX = Body.transform.localEulerAngles.x;
    }

    private void Update()
    {
        if (isActive)
        {
            Vector3 direction = Vector3.forward * MyJoystick.Vertical + Vector3.right * MyJoystick.Horizontal;
            transform.position += direction * speed * Time.fixedDeltaTime;

            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xClamp.x, xClamp.y), transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, zClamp.x, zClamp.y));
            Body.transform.localEulerAngles = new Vector3(bodyStartRotationX, Mathf.Clamp(Body.transform.localEulerAngles.y + direction.x, 45, 135), 0); 
        }
    }

    private void LateUpdate()
    {
        if (isActive)
        {
            Arm1.transform.localRotation = new Quaternion(0, 0, Arm1.transform.localRotation.z, Arm1.transform.localRotation.w);
            Arm2.transform.localRotation = new Quaternion(0, 0, Arm2.transform.localRotation.z, Arm2.transform.localRotation.w);
            Arm3.transform.localRotation = new Quaternion(0, 0, Arm3.transform.localRotation.z, Arm3.transform.localRotation.w);
            Arm4.transform.localRotation = new Quaternion(0, 0, Arm4.transform.localRotation.z, Arm4.transform.localRotation.w);
            CheckGround(); 
        }
    }

    #region
    [SerializeField] Transform LeftBack, RightBack;
    RaycastHit hit;
    bool isFalled;

    public void CheckGround()
    {
        if (isFalled)
        {
            return;
        }
        if (Physics.Raycast(LeftBack.position, Vector3.down, out hit, 1))
        {
            Debug.DrawRay(LeftBack.position, Vector3.down * hit.distance, Color.blue);
        }
        else
        {
            Debug.DrawRay(LeftBack.position, Vector3.down * 1, Color.red);
            Debug.Log("Fall Left");
            isFalled = true;
            //Fall Left
        }

        if (Physics.Raycast(RightBack.position, Vector3.down, out hit, 1))
        {
            Debug.DrawRay(RightBack.position, Vector3.down * hit.distance, Color.blue);
        }
        else
        {
            Debug.DrawRay(RightBack.position, Vector3.down * 1, Color.red);
            Debug.Log("Fall Right");
            isFalled = true;
            //Fall Right
        }
    }
    #endregion
}
