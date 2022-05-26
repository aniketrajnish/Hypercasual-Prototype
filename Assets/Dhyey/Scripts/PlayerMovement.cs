using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : MonoBehaviour
{
    public bool isRunning = true;
    public float speed = 6.0f;

    public Animator animator;
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
            if(touch.isInProgress)
            {
                isRunning = false;
                Debug.Log("Touching");
            }
            else
            {
                isRunning = true;
            }
        }
        animator.SetBool("isRunning", isRunning);
    }

    bool turnLeft = false;
    bool turnRight = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("turn"))
        {
            Debug.Log("Collision");
            turnLeft = collision.gameObject.GetComponent<TurnMechanic>().turnLeft;
            turnRight = collision.gameObject.GetComponent<TurnMechanic>().turnRight;
            
            if(turnLeft)
            {
                transform.Rotate(0, -90, 0);
            }
            else if (turnRight)
            {
                transform.Rotate(0, 90, 0);
            }
        }
    }
}
