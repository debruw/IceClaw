using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject GlacierParent;
    int nextSlot = 1;
    public GameObject[] EmptySlots;   

    IEnumerator MovePlayerForward()
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("Moveforward");
        GlacierParent.transform.position -= new Vector3(0, 0, (Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position) / 2));
        EmptySlots[nextSlot].transform.position += new Vector3(0, 0, (Vector3.Distance(EmptySlots[0].transform.position, EmptySlots[1].transform.position)));
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
}
