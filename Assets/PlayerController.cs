using UnityEngine;
using Invector.vCharacterController;
using Invector;


public class PlayerController : MonoBehaviour
{

    public GameObject worm, spider, chiken, bear, kenguru, nullRotation;

    [Header("Worm Settings")]
    public float wormSpeed;
    public float jumpHeight_Worm;
    public float cameraDistanceWorm = 4;
    public Animator animWorm;

    [Header("Spider Settings")]
    public float spiderSpeed;
    public float jumpHeight_Spider;
    public float cameraDistanceSpider = 5;
    public Animator animSpider;

    [Header("Chiken Settings")]
    public float chickenSpeed;
    public float jumpHeight_Chicken;
    public float cameraDistanceChicken = 6;
    public float fallChicken = 5;
    public Animator animChicken;

    [Header("Bear Settings")]
    public float bearSpeed;
    public float jumpHeight_Bear;
    public float cameraDistanceBear = 8;
    public Animator animBear;

    [Header("Kenguru Settings")]
    public float kenguruSpeed;
    public float jumpHeight_Kenguru;
    public float cameraDistanceKenguru = 10;
    public Animator animKenguru;

    public GameObject cameraPoint;


    private vThirdPersonMotor thirdPersonMotor;
    private Rigidbody _rigidbody;

    public bool isRuning;



    [Header("Camera")]
    public CameraScript thridPersonCamera;

   



    private void Start()
    {
        thirdPersonMotor = GetComponent<vThirdPersonMotor>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (nullRotation)
        {
            nullRotation.transform.position = transform.position;
            //if (spider.activeSelf)
            //nullRotation.transform.rotation = spider.transform.rotation * Quaternion.Euler(0, -spider.transform.rotation.eulerAngles.y, 0);
            //else
            //nullRotation.transform.rotation = transform.rotation * Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Worm();
            _rigidbody.drag = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Spider();
            _rigidbody.drag = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Chicken();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Bear();
            _rigidbody.drag = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            Kenguru();
            _rigidbody.drag = 0;
        }

        if (worm.activeSelf == true)
        {
            animWorm.SetBool("isRun", isRuning);
        }
        else if (spider.activeSelf == true)
        {
            //animSpider.SetBool("isRun", isRuning);
        }
        else if (chiken.activeSelf == true)
        {
            animChicken.SetBool("isRun", isRuning);
        }
        else if (bear.activeSelf == true)
        {
            animBear.SetBool("isRun", isRuning);
        }
        else if (kenguru.activeSelf == true)
        {
            animKenguru.SetBool("isRun", isRuning);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !thirdPersonMotor.isGrounded)
        {
            isRuning = true;
        }
        else isRuning = false;
    }

    void FixedUpdate()
    {
        cameraPoint.transform.position = transform.position + new Vector3(0, 2, 0);
    }

    public void Worm()
    {
        spider.transform.parent = transform;
        worm.SetActive(true);

        spider.SetActive(false);
        chiken.SetActive(false);
        bear.SetActive(false);
        kenguru.SetActive(false);


        //изменени€ скорости ходьбы
        thirdPersonMotor.SetWalkSpeed(wormSpeed);
        thirdPersonMotor.SetJumpParameters(jumpHeight_Worm);

        //изменение камеры
        thridPersonCamera.SetRadius(cameraDistanceWorm);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void Spider()
    {
        spider.transform.position = transform.position;
        spider.transform.parent = null;
        spider.SetActive(true);

        worm.SetActive(false);
        chiken.SetActive(false);
        bear.SetActive(false);
        kenguru.SetActive(false);


        //изменени€ скорости ходьбы
        thirdPersonMotor.SetWalkSpeed(spiderSpeed);
        thirdPersonMotor.SetJumpParameters(jumpHeight_Spider);

        //изменение камеры
        thridPersonCamera.SetRadius(cameraDistanceSpider);
    }

    public void Chicken()
    {
        spider.transform.parent = transform;
        _rigidbody.drag = fallChicken;


        chiken.SetActive(true);

        worm.SetActive(false);
        spider.SetActive(false);
        bear.SetActive(false);
        kenguru.SetActive(false);


        //изменени€ скорости ходьбы
        thirdPersonMotor.SetWalkSpeed(chickenSpeed);
        thirdPersonMotor.SetJumpParameters(jumpHeight_Chicken);

        //изменение камеры
        thridPersonCamera.SetRadius(cameraDistanceChicken);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void Bear()
    {
        spider.transform.parent = transform;
        bear.SetActive(true);

        worm.SetActive(false);
        spider.SetActive(false);
        chiken.SetActive(false);
        kenguru.SetActive(false);


        //изменени€ скорости ходьбы
        thirdPersonMotor.SetWalkSpeed(bearSpeed);
        thirdPersonMotor.SetJumpParameters(jumpHeight_Bear);

        //изменение камеры
        thridPersonCamera.SetRadius(cameraDistanceBear);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void Kenguru()
    {
        spider.transform.parent = transform;
        kenguru.SetActive(true);

        worm.SetActive(false);
        spider.SetActive(false);
        chiken.SetActive(false);
        bear.SetActive(false);


        //изменени€ скорости ходьбы
        thirdPersonMotor.SetWalkSpeed(kenguruSpeed);
        thirdPersonMotor.SetJumpParameters(jumpHeight_Kenguru);

        //изменение камеры
        thridPersonCamera.SetRadius(cameraDistanceKenguru);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
