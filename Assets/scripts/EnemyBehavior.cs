using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private bool isGrounded;
    private Rigidbody rb;
    private bool collided = false;
    private bool killMode = false;
    public double attackDuration;
    private double attackStart;
    public GameObject target;
    
    public float timer;

    public float pushForce;

    private Vector3 targetPosition;

    private bool itsTime = false;
    private bool startTimer = false;

    private bool isDashing;

    private Rigidbody playerRB;

    private float end;

    private float time;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        time = Time.fixedTime;
        playerRB = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isDashing)
        {
            killMode = true;
            startTimer = true;
            end = Time.time + timer;

        }

        if (other.gameObject.tag == "Wall")
        {
            isDashing = false;
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerRB.AddExplosionForce(pushForce * 100, rb.position, 10);
            playerRB.AddForce(Vector3.up * pushForce);
            killMode = false;
        }
    }

    void Update()
    {

        var step =  speed * Time.deltaTime; // calculate distance to move


        
        if (killMode == true && startTimer == true)
        {
            targetPosition = target.transform.position;
            rb.transform.LookAt(targetPosition);
            //rb.transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
            
        }
        
        
        if (Time.time >= end && killMode == true)
        {
            killMode = false;
            startTimer = false;
            isDashing = true;
            attackStart = Time.time;
            rb.AddForce(transform.forward * speed);
        }

        if (isDashing)
        {

            if (Time.time > attackStart + attackDuration)
            {
                isDashing = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            
        }
        
        if (Vector3.Distance(rb.transform.position, targetPosition) < 0.01f)
        {
            isDashing = false;
        }


    }
}
