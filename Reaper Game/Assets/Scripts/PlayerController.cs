using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] private Vector2 targetSpeed;
    [SerializeField] private Vector2 currentSpeed;

    private new Rigidbody2D rigidbody;
    private PlayerInputs input;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        input = new PlayerInputs();
    }

    private void OnEnable()
    {
        input.Player.Move.performed += ChangeMoveDirection;
        input.Player.Move.canceled += ChangeMoveDirection;
        input.Player.Move.Enable();
    }

    private void OnDisable()
    {
        input.Player.Move.Disable();
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        rigidbody.velocity = currentSpeed;
    }

    private void UpdateSpeed()
    {
        if (currentSpeed == targetSpeed)
            return;
        float accelSpeed = acceleration * Time.fixedDeltaTime;
        Vector2 accelDirection = targetSpeed - currentSpeed;
        if(accelDirection.magnitude <= accelSpeed)
        {
            currentSpeed = targetSpeed;
            return;
        }
        currentSpeed += accelDirection.normalized * accelSpeed;
        if(currentSpeed.magnitude > maxSpeed)
        {
            Debug.Log(currentSpeed.magnitude + " speed, " + currentSpeed + " vector");
        }
    }

    private void ChangeMoveDirection(InputAction.CallbackContext obj)
    {
        Vector2 inputSpeed = obj.ReadValue<Vector2>();
        if (inputSpeed.magnitude > 1)
            inputSpeed.Normalize();
        targetSpeed = inputSpeed * maxSpeed;
    }

}
