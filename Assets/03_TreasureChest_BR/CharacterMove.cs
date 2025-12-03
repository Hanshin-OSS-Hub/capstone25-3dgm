using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1.0f;
 
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
 
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
 
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
 
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}