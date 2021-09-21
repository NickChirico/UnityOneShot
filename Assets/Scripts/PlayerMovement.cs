using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug Tools")]
    public bool RightClickToMove;

    private Rigidbody2D rb;
    private float moveH, moveV;
    public float baseMoveSpeed = 1.25f;
    public float maxMoveSpeed = 5f;
    public float speedBurstOneShot = 2f;
    public float speedBurstNormal = 0.5f;
    public float accelerationTime;
    public float decelerationTime;
    public float targetSpeed;

    Vector2 destination;
    Vector2 direction;
    bool isMoving;
    public GameObject destinationIndicator;


    [SerializeField] private float currentMoveSpeed = 1.25f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;

        destination = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (RightClickToMove)
        {
            //destinationIndicator.SetActive(true);

            if (Input.GetMouseButtonDown(1))
            {
                SetDestinationClick();

                Vector2 myPos = new Vector2(this.transform.position.x, this.transform.position.y);
                direction = (destination - myPos).normalized;


            }

            //moveH = direction.x * currentMoveSpeed;
            //moveV = direction.y * currentMoveSpeed;

            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, currentMoveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, destination) < 0.05f)
                {
                    // Arrived
                    isMoving = false;
                }
            }

        }
        else
        {
            //destinationIndicator.SetActive(false);

            moveH = Input.GetAxis("Horizontal") * currentMoveSpeed;
            moveV = Input.GetAxis("Vertical") * currentMoveSpeed;

            rb.velocity = new Vector2(moveH, moveV);
            direction = new Vector2(moveH, moveV);
            FindObjectOfType<PlayerAnimation>().SetDirection(direction);

            Vector2 indicatiorLoc = new Vector2(this.transform.position.x + (moveH * 0.15f), this.transform.position.y + (moveV * 0.15f));
            destinationIndicator.transform.position = indicatiorLoc;

        }
    }

    void SetDestinationClick()
    {
        destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        destinationIndicator.transform.position = destination;

        isMoving = true;
    }

    public void SpeedBoost(bool oneShot)
    {
        // NOT USING AMOUNT PARAMETER
        if (oneShot)
        {
            targetSpeed = currentMoveSpeed + speedBurstOneShot;
        }
        else
        {
            targetSpeed = currentMoveSpeed + speedBurstNormal;
        }
        //Debug.Log(targetSpeed);

        if (targetSpeed > maxMoveSpeed)
        {
            targetSpeed = maxMoveSpeed;
        }
    }

    private void Update()
    {
        /*if (targetSpeed > currentMoveSpeed)
        {
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetSpeed, Time.deltaTime * accelerationTime);
        }
        else if(targetSpeed < currentMoveSpeed && currentMoveSpeed >= baseMoveSpeed)
        {
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetSpeed, Time.deltaTime * decelerationTime);
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }*/

        if (targetSpeed > baseMoveSpeed)
        {
            targetSpeed = Mathf.Lerp(targetSpeed, baseMoveSpeed, Time.deltaTime * decelerationTime);
        }
        else
        {
            targetSpeed = baseMoveSpeed;
        }

        currentMoveSpeed = targetSpeed;


        //Debug.Log(currentMoveSpeed);

    }

}
