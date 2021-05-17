using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TapticPlugin;

public class Glacier : MonoBehaviour
{
    public float LifeTime;
    public float timer;
    public int count;
    public MeshRenderer myMeshRenderer;
    public bool isPicked, isLast, isCreated;
    public GameObject[] ChildObjects;
    public GameObject BrokenGlacier;
    public Color BrokenColor, DefaultColor;

    private void Start()
    {
        timer = LifeTime / 21;
        DefaultColor = myMeshRenderer.material.color;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isLast && isCreated)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && count <= 20)
            {
                if (count == 0)
                {                    
                    myMeshRenderer.enabled = false;
                    myMeshRenderer.GetComponent<Collider>().enabled = false;
                    foreach (var item in ChildObjects)
                    {
                        item.GetComponent<MeshRenderer>().material.DOColor(BrokenColor, 1);
                    }
                }
                else if (count == 10)
                {
                    GameManager.Instance.m_ShipController.ShipsGlaciers[1].isLast = true;
                    GameManager.Instance.m_ShipController.ShipsGlaciers[1].CreateBrokenPieces(BrokenGlacier);
                }
                else if (count == 15)
                {
                    if (GameManager.Instance.m_ShipController.ShipsGlaciers.Count == 3)
                    {
                        GameManager.Instance.m_ShipController.m_Animator.enabled = true;
                        GameManager.Instance.m_ShipController.transform.DOMoveY(-15, 5);
                        if (PlayerPrefs.GetInt("VIBRATION") == 1)
                            TapticManager.Impact(ImpactFeedback.Medium);
                        GameManager.Instance.m_ShipController.m_Animator.SetTrigger("Sink");
                    }
                }
                timer = LifeTime / 21;

                ChildObjects[count].GetComponent<MeshRenderer>().material.DOColor(BrokenColor, .3f);
                ChildObjects[count].GetComponent<Rigidbody>().useGravity = true;
                ChildObjects[count].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Random.Range(-1.5f, -1f));
                count++;
                if (count > 20)
                {
                    GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(this);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void CreateBrokenPieces(GameObject broken)
    {        
        myMeshRenderer.enabled = false;
        GameObject broke = Instantiate(broken);
        broke.transform.parent = transform;
        broke.transform.localPosition = Vector3.zero;
        ChildObjects = new GameObject[21];
        for (int i = 0; i < broke.transform.childCount; i++)
        {
            ChildObjects[i] = broke.transform.GetChild(i).gameObject;
        }
        isCreated = true;
    }

    [SerializeField] Color EndColor;
    public void ActivateObject(Vector3 emptySlot)
    {
        isPicked = true;
        myMeshRenderer.material.DOColor(EndColor, .8f);
        transform.DOMove(emptySlot, .8f);
    }

    public void BreakAll()
    {
        isLast = false;
        for (int i = count; i < ChildObjects.Length; i++)
        {
            ChildObjects[i].GetComponent<MeshRenderer>().material.DOColor(BrokenColor, .3f);
            ChildObjects[i].GetComponent<Rigidbody>().useGravity = true;
            ChildObjects[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, Random.Range(-1.5f, -1f));
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Light);
        }
        GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(this);

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        if (isPicked)
        {
            if (other.CompareTag("Hazard"))
            {
                other.GetComponent<Collider>().enabled = false;
                GameManager.Instance.m_ShipController.BreakLastGlacier();
                other.GetComponent<Rigidbody>().useGravity = true;
                other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 2f);
                GameManager.Instance.ShakeCamera();
                SoundManager.Instance.playSound(SoundManager.GameSounds.HazardHit);
                Destroy(other.gameObject, 2f);
            }
            else if (other.CompareTag("FinishLine"))
            {
                GameManager.Instance.isGameOver = true;
                StartCoroutine(GameManager.Instance.WaitAndGameWin());
                GameManager.Instance.m_CollisionDetection.Player.transform.DOMoveZ(GameManager.Instance.m_CollisionDetection.Player.transform.position.z + 5, 1f);
                if (PlayerPrefs.GetInt("VIBRATION") == 1)
                    TapticManager.Impact(ImpactFeedback.Light);
            }
            else if (other.CompareTag("Glacier") && !other.GetComponent<Glacier>().isPicked)
            {
                other.GetComponent<Collider>().enabled = false;
                other.GetComponent<Glacier>().CreateBrokenPieces(BrokenGlacier);
                other.GetComponent<Glacier>().BreakAll();
                GameManager.Instance.m_ShipController.BreakLastGlacier();
                other.GetComponent<Rigidbody>().useGravity = true;
                other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 2f);
                GameManager.Instance.ShakeCamera();
                GameManager.Instance.m_ShipController.ShipsGlaciers.Remove(other.GetComponent<Glacier>());
                SoundManager.Instance.playSound(SoundManager.GameSounds.HazardHit);
                if (GameManager.Instance.m_ShipController.ShipsGlaciers.Count == 2)
                {
                    if (PlayerPrefs.GetInt("VIBRATION") == 1)
                        TapticManager.Impact(ImpactFeedback.Medium);
                    GameManager.Instance.m_ShipController.m_Animator.enabled = true;
                    GameManager.Instance.m_ShipController.transform.DOMoveY(-15, 5);
                    GameManager.Instance.m_ShipController.m_Animator.SetTrigger("Sink");
                }
                Destroy(other.gameObject, 2f);
            }
        }
    }

    public bool blink;

    public IEnumerator Blink()
    {
        bool myState = true;         //Makes our state true
        while (blink)                        //While our state is true
        {
            myState = !myState;             //Change our state to the oposite
            if (myState)
            {
                myMeshRenderer.material.color = BrokenColor;
            }
            else
            {
                myMeshRenderer.material.color = DefaultColor;
            }
            yield return new WaitForSeconds(0.25f);     //Waits for 0.25 seconds
        }
    }
}
