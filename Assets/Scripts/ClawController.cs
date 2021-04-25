using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] DynamicJoystick MyJoystick;

    public float speed;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * MyJoystick.Vertical + Vector3.right * MyJoystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glacier"))
        {
            Debug.Log(other.name);
            other.GetComponent<Collider>().enabled = false;

        }
    }
}
