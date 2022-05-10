using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlThirdPerson : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] private float speed = 20;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float rotateSpeed = 5;
    private float horizontal;
    private float vertical;
    [SerializeField] private bool onGround;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerRotation();
        PlayerColor();
    }

    void PlayerMovement() {
        //moving
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(horizontal, 0, vertical);
        transform.Translate(playerMovement * speed * Time.deltaTime);

        //jumping
        if(onGround && Input.GetKeyDown(KeyCode.Space)) {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
		}
	}

    void PlayerColor() {
        if (Input.GetKeyDown(KeyCode.R)) {
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            GetComponent<Renderer>().material.color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            GetComponent<Renderer>().material.color = Color.clear;
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("WinningPlatform")) {
            onGround = true;
		}
	}

    void PlayerRotation() {
        float mouseInput = Input.GetAxis("Mouse X");
        Vector3 playerRotate = new Vector3(0, mouseInput, 0);
        transform.Rotate(playerRotate * rotateSpeed);
	}
}
