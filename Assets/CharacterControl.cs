using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    internal Vector3 inputSmooth; // generate smooth input based on the inputSmooth value       

    public GameObject Spider;
    private bool isSpider = false;

    private CharacterController controller;
    private Vector3 moveDirection;
    public float gravity = 20.0f;
    public float wallCrawlSpeed = 6.0f;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public Transform cameraTransform; // Reference to the camera's transform

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Spider.SetActive(false);
        cameraTransform = Camera.main.transform; // Assuming your camera is tagged as "MainCamera"
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isSpider = !isSpider;
            Spider.SetActive(isSpider);
        }

        if (isSpider)
        {


            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Wall"))
                {
                    // Code for climbing walls
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                    moveDirection *= wallCrawlSpeed;
                    
                    Debug.Log("wall");
                }
            }
        }
        else
        {
            // Code for movement and jumping
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                // Convert input direction to world space
                moveDirection = cameraTransform.TransformDirection(moveDirection);
                moveDirection.y = 0; // Keep the character grounded



                moveDirection *= speed;

                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
