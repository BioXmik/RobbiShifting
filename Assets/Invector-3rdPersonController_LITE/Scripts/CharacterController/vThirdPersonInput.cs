using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public bool mobileVersion;
        public Joystick joystick;
        public GameObject jumpButton;
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        private PlayerController playerController;
        public Camera cameraMain;
        public CameraScript cameraScript;

        private Rigidbody rb;
        public bool wallContact;
        private List<GameObject> collisonGameObjects = new List<GameObject>();

        #endregion

        protected virtual void Start()
        {
            if (Application.isMobilePlatform == false)
            {
                mobileVersion = false;
                joystick.gameObject.SetActive(false);
                jumpButton.SetActive(false);
                cameraScript.mobileVersion = false;
            }
            else if (Application.isMobilePlatform == true)
            {
                mobileVersion = true;
                joystick.gameObject.SetActive(true);
                jumpButton.SetActive(true);
                cameraScript.mobileVersion = true;
            }
            InitilizeController();
            InitializeTpCamera();
            rb = GetComponent<Rigidbody>();
            playerController = GetComponent<PlayerController>();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collisonGameObjects.Contains(collider.gameObject))
            {
                collisonGameObjects.Add(collider.gameObject);
            }
            if (collider.gameObject.tag == "Wall" && playerController.spider.activeSelf)
            {
                wallContact = true;
                if (wallContact && collisonGameObjects.Count > 1)
                {
                    Jump();
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            collisonGameObjects.Remove(collider.gameObject);
            int countWall = 0;
            foreach (GameObject gm in collisonGameObjects)
            {
                if (gm.tag == "Wall")
                {
                    countWall++;
                }
            }
            if (countWall <= 0)
            {
                wallContact = false;
            }
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
        }

        public virtual void MoveInput()
        {
            if (mobileVersion)
            {
                cc.input.x = joystick.Horizontal;
                cc.input.z = joystick.Vertical;
            }
            else
            {
                cc.input.x = Input.GetAxis(horizontalInput);
                cc.input.z = Input.GetAxis(verticallInput);
            }
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        public void Jump()
        {
            if (JumpConditions())
            {
                cc.Jump();
            }
        }

        #endregion       
    }
}