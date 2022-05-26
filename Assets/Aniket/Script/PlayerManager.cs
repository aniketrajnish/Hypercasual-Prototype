using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float rotationSpeed = 800f;
    [SerializeField] GameManager gm;
    bool isGrounded;
    [HideInInspector] public int playerHealth = 250;
    public static PlayerManager instance;
    bool dead, changed;

    void Awake()
    {
        instance = this;
        isGrounded = false;
    }

    private void Update()
    {
        MovePlayer();
        //RotatePlayer();

        if (playerHealth < 0f && !dead)        
            dead = true;   
    }     

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            Grounded();
    }

    void Grounded()
    {
        isGrounded = true;
        gm.playerState = GameManager.PlayerState.Move;
    }

    void RotatePlayer()
    {
        float horizontalInput = movementJoystick.joystickVector.x;
        float verticalInput = movementJoystick.joystickVector.y;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        Transform playerModel = transform;

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void MovePlayer()
    {
        Vector3 velocity;

        if (isGrounded && movementJoystick.joystickVector.y != 0)
        {
            velocity = new Vector3(movementJoystick.joystickVector.x * playerSpeed,
            0,
            movementJoystick.joystickVector.y * playerSpeed);

            transform.position += velocity * Time.deltaTime;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -21f, 21f), transform.position.y, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            GameManager.instance.ChangePlayer();
        }
    }
}
