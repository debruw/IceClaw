using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed;
    public List<Glacier> ShipsGlaciers;
    public Animator m_Animator;
    public GameObject BrokenGlacier;

    private void Start()
    {
        ShipsGlaciers[0].isLast = true;
        ShipsGlaciers[0].CreateBrokenPieces(BrokenGlacier);
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

    public void Sink()
    {
        StartCoroutine(GameManager.Instance.WaitAndGameLose());
    }

    public void BreakLastGlacier()
    {
        if (!ShipsGlaciers[1].isLast)
        {
            ShipsGlaciers[1].isLast = true;
            ShipsGlaciers[1].CreateBrokenPieces(BrokenGlacier);
        }
        ShipsGlaciers[0].BreakAll();
    }
}
