using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    bool isRunning = true;
    [SerializeField]
    float speed = 6.0f;
    [SerializeField]
    float rotationSens = 10;

    Animator animator;
    public GameObject player;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        animator.SetBool("isRunning", isRunning);
    }

    // Update is called once per frame
    void Update()
    {
        TouchControl();
        Movement();
    }
    void Movement()
    {
        /*float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;*/

        float translation = speed;
        float straffe = 0;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        if (isRunning)
        {
            transform.Translate(straffe, 0, translation);
        }
    }

    void TouchControl()
    {
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];
            if (touch.isInProgress)
            {
                isRunning = false;
                Debug.Log("Touching");
            }
            else
            {
                isRunning = true;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            isRunning = false;
            Debug.Log("Touching");
        }
        else
        {
            isRunning = true;
        }
        animator.SetBool("isRunning", isRunning);
    }

    bool turnLeft = false;
    bool turnRight = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("turn"))
        {
            turnLeft = collision.gameObject.GetComponent<TurnMechanic>().turnLeft;
            turnRight = collision.gameObject.GetComponent<TurnMechanic>().turnRight;
            Quaternion originalRotationValue = transform.rotation;
            
            if(turnLeft)
            {
                StartCoroutine(IStartTurning(-90));
            }
            else if (turnRight)
            {
                StartCoroutine(IStartTurning(90));
            }
        }
    }
    IEnumerator IStartTurning(float angle)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + angle, currentRotation.eulerAngles.z);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSens);
            yield return null;
        }
    }
}
