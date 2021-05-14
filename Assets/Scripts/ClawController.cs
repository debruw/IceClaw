using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] FloatingJoystick MyJoystick;
    public float speed;

    Vector2 xClamp, zClamp;
    Vector3 distancePos;
    [SerializeField] GameObject Arm1, Arm2, Arm3, Arm4, Body;
    float bodyStartRotationX;
    public bool isActive = true;

    private void Awake()
    {
        xClamp = new Vector2(transform.localPosition.x - 10, transform.localPosition.x + 8);
        zClamp = new Vector2(transform.localPosition.z - 13, transform.localPosition.z + 13);
        bodyStartRotationX = Body.transform.localEulerAngles.x;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isActive)
        {
            Vector3 direction = Vector3.forward * MyJoystick.Vertical + Vector3.right * MyJoystick.Horizontal;
            transform.position += direction * speed * Time.deltaTime;

            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xClamp.x, xClamp.y), transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, zClamp.x, zClamp.y));
        }
        distancePos = Body.transform.position - transform.position;
        Body.transform.localEulerAngles = new Vector3(bodyStartRotationX, Mathf.Atan2(-distancePos.z, distancePos.x) * Mathf.Rad2Deg, 0);
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        Arm1.transform.localRotation = new Quaternion(0, 0, Arm1.transform.localRotation.z, Arm1.transform.localRotation.w);
        Arm2.transform.localRotation = new Quaternion(0, 0, Arm2.transform.localRotation.z, Arm2.transform.localRotation.w);
        Arm3.transform.localRotation = new Quaternion(0, 0, Arm3.transform.localRotation.z, Arm3.transform.localRotation.w);
        Arm4.transform.localRotation = new Quaternion(0, 0, Arm4.transform.localRotation.z, Arm4.transform.localRotation.w);

        //Arm1.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Arm1.transform.localRotation.z, 40, 90));
        //Arm2.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Arm2.transform.localRotation.z, 40, 120));
        //Arm3.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Arm3.transform.localRotation.z, 0, 50));
        //Arm4.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Arm1.transform.localRotation.z, 40, 90));
    }
}
