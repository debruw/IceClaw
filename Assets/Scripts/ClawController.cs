using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] DynamicJoystick MyJoystick;
    public float speed;
    public GameObject[] EmptySlots;
    public GameObject GlacierParent;
    int nextSlot = 1;
    Vector2 xClamp, zClamp;
    Vector3 startPos;

    private void Start()
    {
        xClamp = new Vector2(transform.localPosition.x - 2, transform.localPosition.x + 2);
        zClamp = new Vector2(transform.localPosition.z - 1, transform.localPosition.z + 2);
    }

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * MyJoystick.Vertical + Vector3.right * MyJoystick.Horizontal;
        transform.position += direction * speed * Time.fixedDeltaTime;
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xClamp.x, xClamp.y), transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, zClamp.x, zClamp.y));
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
                other.GetComponent<Glacier>().ActivateObject(EmptySlots[nextSlot].transform);

                StartCoroutine(MovePlayerForward());
            }
        }
    }

    IEnumerator MovePlayerForward()
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("Moveforward");
        GlacierParent.transform.position -= new Vector3(0, 0, (Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position) / 2));
        EmptySlots[nextSlot].transform.position += new Vector3(0, 0, (Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position)));
    }
}
