using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform cue;
    private Transform line;
    [SerializeField] private float pushForce = 1f;
    [SerializeField] private float maxpushForce = 30.0f;
    private Vector2 dir;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cue = transform.GetChild(0).transform;
        line = transform.GetChild(1).transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && pushForce < maxpushForce)
        {
            pushForce += Time.deltaTime * 12;
            cue.localPosition += Vector3.left * Time.deltaTime * 2;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = (mousePosition - rb.position).normalized;
            rb.velocity = dir * pushForce;

            cue.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
            cue.localPosition = new Vector3(-18.75f, 0f, 0f);
            pushForce = 0;
        }
        if (Input.GetMouseButtonDown(1))
        {
            pushForce = maxpushForce;
            cue.localPosition += Vector3.left * 4;
        }
        if (rb.velocity.magnitude < 0.01f)
        {
            OnAction();
        }
    }

    void OnAction()
    {
        cue.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
    }
}