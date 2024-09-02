using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLogic : MonoBehaviour
{
    public stateManager sm;
    
    private Rigidbody2D rb;
    private Transform cue;
    private Transform line;

    [SerializeField] private float pushForce = 0f;
    [SerializeField] private float maxPushForce = 50.0f;

    private Vector2 dir;
    private bool isOnHand = true;
    private bool isFree = true;
    private bool initialClickReleased = false;
    private Transform nan;
    private HashSet<Collider2D> collidersInContact = new HashSet<Collider2D>();
    private AudioSource audiosource;
    
    void Awake()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 1f;
        transform.position = mousePosition;

        nan  = transform.GetChild(2).transform;
    }

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sm.listOfBalls.Add(rb);

        cue  = transform.GetChild(0).transform;
        line = transform.GetChild(1).transform;
        

        SetVisibility(false);
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandlePushForce();
        HandleMovementState();
    }

    private void HandleAiming()
    {
        if (isOnHand)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 1f;
            transform.position = mousePosition;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && isOnHand && isFree && sm.AllBallsHasStopped())
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && sm.AllBallsHasStopped())
        {
            initialClickReleased = true;
        }

        if (Input.GetMouseButtonUp(0) && !isOnHand && initialClickReleased && sm.AllBallsHasStopped())
        {
            Shoot();
        }

        if (!isOnHand && Input.GetMouseButtonDown(1) && sm.AllBallsHasStopped())
        {
            ChargeShot();
        }

        if (!isOnHand && Input.GetMouseButtonUp(1) && sm.AllBallsHasStopped())
        {
            Shoot();
        }
    }

    private void HandlePushForce()
    {
        if (initialClickReleased && Input.GetMouseButton(0) && pushForce < maxPushForce && sm.AllBallsHasStopped())
        {
            pushForce += Time.deltaTime * 20;
            cue.localPosition += Vector3.left * Time.deltaTime * 2;
        }
    }

    private void HandleMovementState()
    {
        if (!isOnHand && sm.AllBallsHasStopped())
        {
            SetVisibility(true);
        }

        if (transform.localScale == new Vector3(0.30f, 0.30f, 0.30f))
        {
            SetVisibility(false);
            Destroy(this);
        }
    }

    private void StartAiming()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        isOnHand = false;
        SetVisibility(true);
    }

    private void Shoot()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mousePosition - rb.position).normalized;
        rb.velocity = dir * pushForce;

        cue.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
        cue.localPosition = new Vector3(-18.75f, 0f, 0f);
        pushForce = 0;

        audiosource.Play();
    }

    private void ChargeShot()
    {
        pushForce = maxPushForce;
        cue.localPosition += Vector3.left * 4;
    }

    private void SetVisibility(bool visibility)
    {
        cue.gameObject.SetActive(visibility);
        line.gameObject.SetActive(visibility);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        collidersInContact.Add(col);

        UpdateState();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        collidersInContact.Add(col);

        UpdateState();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        collidersInContact.Remove(col);

        UpdateState();
    }

    private void UpdateState()
    {
        if (collidersInContact.Count > 0)
        {
            nan.gameObject.SetActive(true);
            isFree = false;
        }
        else
        {
            nan.gameObject.SetActive(false);
            isFree = true;
        }
    }

    private void OnDestroy()
    {
        sm.listOfBalls.Remove(rb);
    }
}