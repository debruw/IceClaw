using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed;
    public List<Glacier> ShipsGlaciers;


    private void Start()
    {
        ShipsGlaciers[0].isLast = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
    }
}
