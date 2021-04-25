using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glacier : MonoBehaviour
{
    [SerializeField] float LifeTime;
    float timer;

    private void Start()
    {
        timer = LifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Debug.Log("Öldü");
        }
        else if (timer < LifeTime / 2)
        {

        }
    }
}
