using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    public Transform player;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.25f;
    public float decreaseFactor = 1;

    Vector3 originalPos;

    public Vector3 offset;
    private Vector3 velocity;
    public float smoothsTime = .5f;

    void Awake()
    {
        offset = transform.position - player.position;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }       

        if (shakeDuration > 0)
        {
            camTransform.localPosition = new Vector3(originalPos.x + Random.insideUnitSphere.x * shakeAmount, transform.localPosition.y, transform.localPosition.z);

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            transform.position = player.position + offset;
        }        
    }
}
