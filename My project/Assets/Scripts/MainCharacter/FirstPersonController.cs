using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //Objects Attached here......
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private GameObject cam;
    private CharacterController controller;

    //Masks and variables for detect ground.
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private Transform feet;
    private float feetRadius = .2f;

    //physics related variables
    private float speed = 10f;
    private float gravity = -20f;
    [SerializeField]
    private float jumpHeight = 3f;
    Vector3 velocity;
    Vector3 preMove;

    //other variables
    [SerializeField]
    public float sensitivity = 150f;
    private float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        movement();
        fixSpeed();
        
        
    }

    void fixSpeed(){
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 30f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 10f;
        }
    }

    void movement()
    {
        bool isGround = Physics.CheckSphere(feet.position, feetRadius, groundMask);

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        Vector3 move = new Vector3();
        if (isGround)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            move = transform.right * x + transform.forward * z;
            preMove = move;
        }
        else
        {
            move = preMove;
        }


        controller.Move(move);

        if (isGround && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if (move != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
