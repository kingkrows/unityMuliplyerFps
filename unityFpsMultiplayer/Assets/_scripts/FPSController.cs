﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    private Transform firstPerson_View;
    private Transform firstPerson_Camera;
    private Vector3 firstPerson_View_Rotation = Vector3.zero;

    public float walkSpeed=6.75f;
    public float runSpeed=10f;
    public float crouchSpeed = 4f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private float speed;
    private bool is_Moving, is_Grounded, is_Crouching;
    private float inputX, inputY;
    private float inputX_Set, inputY_Set;
    private float inputModifyFactor;
    private bool limitDiagonalSpeed = true;

    private float antiBumpFactor = 0.75f;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {

        firstPerson_View = transform.Find("FPS View").transform;
        characterController = GetComponent<CharacterController>();
        speed = walkSpeed;
        is_Moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    public void PlayerMovement()
    {
        //checking moving forword
        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S))
        {
            if(Input.GetKey(KeyCode.W))
            {
                inputY_Set = 1f;
            }
            else
            {
                inputY_Set = -1f;
            }
        }
        else
        {
            inputY_Set = 0f;
        }

        //checking moveing left or right
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                inputX_Set = -1f;
            }
            else
            {
                inputX_Set = 1f;
            }
        }
        else
        {
            inputX_Set = 0f;
        }

        //move  slowly form 0 to 1
        inputY = Mathf.Lerp(inputY,inputY_Set,Time.deltaTime*19f);
        inputX = Mathf.Lerp(inputX, inputX_Set, Time.deltaTime * 19f);

        inputModifyFactor = Mathf.Lerp(inputModifyFactor,
            (inputY_Set != 0 && inputX_Set != 0 && limitDiagonalSpeed) ? 0.75f : 1f,
            Time.deltaTime * 19f);

        firstPerson_View_Rotation = Vector3.Lerp(firstPerson_View_Rotation, Vector3.zero, Time.deltaTime * 5f);
        firstPerson_View.localEulerAngles = firstPerson_View_Rotation;

        if(is_Grounded)
        {
            moveDirection = new Vector3(inputX*inputModifyFactor,-antiBumpFactor,inputY*inputModifyFactor);
            moveDirection = transform.TransformDirection(moveDirection)*speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        is_Grounded = (characterController.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        is_Moving = characterController.velocity.magnitude > 0.15f;

    }

}
