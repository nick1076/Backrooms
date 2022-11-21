using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]

public class FpsControllerLPFP : MonoBehaviour
{

    #region Variables

    [Header("Player Statistics")]

    public float currentWalkingSpeed = 0f;
    public float currentRunningSpeed = 0f;
    public float currentSprintTime = 5.0f;
    public float finalSpeed = 0.0f;
    public float i_timeSinceLastStep;
    public float targetVolume;
    public float pushingForce = 4.0f;
    public float speedBenefit = 0.0f;
    public float speedBenefitSecondary = 1.0f;
    public float jumpsPerformed = 0.0f;
    [Space]
    private int cyclesWaited;
    [Space]
    public bool _isGrounded;
    public bool isCrouching;
    [Space]
    public bool moveVarLocked;
    public bool canMove;
    public bool canCrouch = true;
    public bool canRun = true;
    public bool mouseOut = false;
    public bool stopVelControl;
    [Space]
    public bool noClip;
    public bool dead;

    //Input Check
    public bool w;
    public bool a;
    public bool s;
    public bool d;
    public bool jump;
    public bool shift;

    public bool inJump;
    [Space]
    public AudioClip current;
    public AudioClip target;
    [Space]
    public WalkingMaterialData currentWalkingMat;
    [Space]
    public Vector3 armPosition;
    [Space]
    public GameObject currentlyBeingHeldObject;

#pragma warning disable 649

    [Header("Player Settings")]
    
    public bool cheats;
    public float reach = 2.0f;
    public string colliderType = "Capsule";
    public bool funMovement;

    [Header("Movement Settings")]

    public float baseWalkSpeed = 3.5f;
    public float baseRunSpeed = 6;
    public float baseCrouchSpeed = 2;
    public float speedStepMultiplierWalk = .125f;
    public float speedStepMultiplierRun = .095f;
    public float speedStepMultiplierCrouch = .2f;
    public float crouchHeight = .2f;
    public float normalHeight = 1.8f;
    public float movementSmoothness = 0.125f;
    public float jumpPower = 2.5f;
    public float totalSprintTimeAllowed = 8.0f;
    public float airMovementMultiplier = 0.02f;
    public float speedLimit = 3;
    public float speedLimitRunning = 6;

    [Header("Look Settings")]

    public float mouseSensitivity = 7f;
    public float rotationSmoothness = 0.05f;
    [SerializeField] private float minVerticalAngle = -90f;
    [SerializeField] private float maxVerticalAngle = 90f;

    [SerializeField] private FpsInput input;

    #pragma warning restore 649

    [HideInInspector] public Rigidbody _rigidbody;
    private BoxCollider _collider;
    private AudioSource _audioSource;
    private SmoothRotation _rotationX;
    private SmoothRotation _rotationY;
    private SmoothVelocity _velocityX;
    private SmoothVelocity _velocityZ;

    private bool justGrabbed = false;

    [Header("Assigned Variables")]

    public TooltipManager tooltipMan;
    [Space]
    public Image mainSprintDisplay;
    public Image parentSprintDisplay;
    [Space]
    public List<Transform> crouchTests = new List<Transform>();
    public List<AmbienceSetData> ambiences = new List<AmbienceSetData>();
    [Space]
    public GameObject baseFootstepObj;
    public GameObject objectCamera;
    public GameObject deathCover;
    [Space]
    public Transform footstepSoundOrigin;
    public Transform cameraTransform;
    public Transform cameraParent;
    public Transform objectHoldTransform;
    [Space]
    public AudioSource ambience;
    [Space]
    public PhysicMaterial physicsMaterial;
    [Space]
    public InventoryController invenControl;

    //Hidden

    private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
    private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];
    List<GameObject> grounds = new List<GameObject>();
    Rigidbody physics;
    Collider collider;

    private Coroutine crouch;
    private Coroutine unCrouch;

    private GameObject lastTooltipObj;
    private bool lastTooltipObjClicked;
    private GameObject lastHoverObj;

    #endregion

    #region Coutotines

    IEnumerator BlendAmbience()
    {
        yield return new WaitForSeconds(0.025f);

        if (target == null)
        {
            if (current != null)
            {
                //We must lower the volume then stop the track
                if (ambience.volume >= 0.01f)
                {
                    ambience.volume -= targetVolume / 100f;
                }
                else
                {
                    ambience.Stop();
                }
            }
        }
        else
        {
            if (!ambience.isPlaying)
            {
                ambience.Play();
            }

            if (current != target)
            {
                //We need to turn down current until 0, then change to target and turn taget up to targetVolume

                if (ambience.volume < 0.01f)
                {
                    ambience.clip = target;
                    current = target;
                }
                else if (ambience.volume > 0.01f)
                {
                    ambience.volume -= targetVolume / 100f;
                }
            }

            if (current == target)
            {
                //We have the clip currently set to the target clip
                if (ambience.volume >= targetVolume - targetVolume / 100f && ambience.volume <= targetVolume + targetVolume / 100f)
                {
                    //Volume is good
                }
                else
                {
                    //Volume needs to be changed
                    if (ambience.volume < targetVolume)
                    {
                        ambience.volume += targetVolume / 100f;
                    }
                    else if (ambience.volume < targetVolume)
                    {
                        ambience.volume -= targetVolume / 100f;
                    }
                }
            }
        }

        StartCoroutine(BlendAmbience());
    }

    IEnumerator RechargeSprint()
    {
        if (input.Run && _isGrounded && _rigidbody.velocity.sqrMagnitude > 0.1f && !isCrouching)
        {
            currentSprintTime -= 0.01f;
            mainSprintDisplay.color = new Color(mainSprintDisplay.color.r, mainSprintDisplay.color.g, mainSprintDisplay.color.b, 1);
            parentSprintDisplay.color = new Color(parentSprintDisplay.color.r, parentSprintDisplay.color.g, parentSprintDisplay.color.b, 1);
            cyclesWaited = 0;
        }
        else
        {
            if (cyclesWaited >= 100)
            {
                currentSprintTime += 0.025f;
                mainSprintDisplay.color = new Color(mainSprintDisplay.color.r, mainSprintDisplay.color.g, mainSprintDisplay.color.b, mainSprintDisplay.color.a - 0.01f);
                parentSprintDisplay.color = new Color(parentSprintDisplay.color.r, parentSprintDisplay.color.g, parentSprintDisplay.color.b, parentSprintDisplay.color.a - 0.01f);
            }
        }

        if (currentSprintTime <= 0)
        {
            currentSprintTime = 0;
        }
        else if (currentSprintTime > totalSprintTimeAllowed)
        {
            currentSprintTime = totalSprintTimeAllowed;
        }

        mainSprintDisplay.rectTransform.localScale = new Vector3(currentSprintTime / totalSprintTimeAllowed, 1, 1);

        cyclesWaited += 1;

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(RechargeSprint());
    }
    IEnumerator Check_StepCheck()
    {
        yield return new WaitForSeconds(.05f);
        i_timeSinceLastStep += .05f;

        StartCoroutine(Check_StepCheck());
    }

    #endregion

    #region Methods

    private void Start()
    {
        StartAlt();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentWalkingSpeed = baseWalkSpeed;
        currentRunningSpeed = baseRunSpeed;
        StartCoroutine(RechargeSprint());
        StartCoroutine(Check_StepCheck());
        StartCoroutine(BlendAmbience());

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _collider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
        cameraParent = AssignCharactersCamera();
        _audioSource.loop = true;
        _velocityX = new SmoothVelocity();
        _velocityZ = new SmoothVelocity();
        ValidateRotationRestriction();
    }

    private Transform AssignCharactersCamera()
    {
        var t = transform;
        cameraParent.SetPositionAndRotation(t.position, t.rotation);
        return cameraParent;
    }

    /// Clamps <see cref="minVerticalAngle"/> and <see cref="maxVerticalAngle"/> to valid values and
    /// ensures that <see cref="minVerticalAngle"/> is less than <see cref="maxVerticalAngle"/>.
    private void ValidateRotationRestriction()
    {
        minVerticalAngle = ClampRotationRestriction(minVerticalAngle, -90, 90);
        maxVerticalAngle = ClampRotationRestriction(maxVerticalAngle, -90, 90);
        if (maxVerticalAngle >= minVerticalAngle) return;
        Debug.LogWarning("maxVerticalAngle should be greater than minVerticalAngle.");
        var min = minVerticalAngle;
        minVerticalAngle = maxVerticalAngle;
        maxVerticalAngle = min;
    }

    private static float ClampRotationRestriction(float rotationRestriction, float min, float max)
    {
        if (rotationRestriction >= min && rotationRestriction <= max) return rotationRestriction;
        var message = string.Format("Rotation restrictions should be between {0} and {1} degrees.", min, max);
        Debug.LogWarning(message);
        return Mathf.Clamp(rotationRestriction, min, max);
    }

    /// Checks if the character is on the ground.
    private void OnCollisionStay()
    {
        var bounds = _collider.bounds;
        var extents = bounds.extents;
        var radius = extents.x - 0.01f;
        Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
            _groundCastResults, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);
        if (!_groundCastResults.Any(hit => hit.collider != null && hit.collider != _collider)) return;
        for (var i = 0; i < _groundCastResults.Length; i++)
        {
            _groundCastResults[i] = new RaycastHit();
        }

        _isGrounded = true;
    }

    /// Processes the character movement and the camera rotation every fixed framerate frame.
    private void FixedUpdate()
    {
        // FixedUpdate is used instead of Update because this code is dealing with physics and smoothing.
        _isGrounded = false;
    }

    /// Moves the camera to the character, processes jumping and plays sounds every frame.
    /// 

    

    private void Update()
    {
        if (dead && Input.GetKeyDown(KeyCode.Mouse0))
        {
            dead = false;
            deathCover.SetActive(false);

            if (GameObject.Find("Area.All.Manager.World") != null)
            {
                this.transform.position = new Vector3(0, 0, 0);
                this.transform.rotation = Quaternion.identity;
            }
        }

        UpdateAlt();
        //Section to update held object
        if (currentlyBeingHeldObject != null)
        {
            currentlyBeingHeldObject.transform.position = objectHoldTransform.position;
        }

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        layerMask = ~layerMask;
        RaycastHit hitMaterial;
        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(-Vector3.up), out hitMaterial, Mathf.Infinity, layerMask))
        {
            if (hitMaterial.collider.gameObject.GetComponent<ObjectMat>() != null)
            {
                Debug.DrawRay(this.transform.position, this.transform.TransformDirection(-Vector3.up) * 1000, Color.green);
                currentWalkingMat = hitMaterial.collider.gameObject.GetComponent<ObjectMat>().material;
            }
            else
            {
                Debug.DrawRay(this.transform.position, this.transform.TransformDirection(-Vector3.up) * 1000, Color.red);
            }
        }

        RaycastHit hitForward;
        if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward), out hitForward, Mathf.Infinity, layerMask))
        {
            if (hitForward.distance <= reach)
            {
                Debug.DrawRay(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward) * 1000, Color.green);
                if (hitForward.collider.gameObject.GetComponent<TooltipObject>() != null)
                {
                    tooltipMan.ShowTooltip();
                    if (lastTooltipObj == null)
                    {
                        lastTooltipObj = hitForward.collider.gameObject;
                    }
                    if (hitForward.collider.gameObject != lastTooltipObj)
                    {
                        lastTooltipObj = hitForward.collider.gameObject;
                        lastTooltipObjClicked = false;
                    }
                    if (lastTooltipObjClicked == false)
                    {
                        tooltipMan.SetTooltip(hitForward.collider.gameObject.GetComponent<TooltipObject>().hoverNote);
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        lastTooltipObjClicked = true;
                        tooltipMan.SetTooltip(hitForward.collider.gameObject.GetComponent<TooltipObject>().clickNote);
                    }
                }
                else
                {
                    lastTooltipObjClicked = false;
                    tooltipMan.HideTooltip();
                }
                if (hitForward.collider.gameObject.GetComponent<HoverUseEvent>() != null)
                {
                    if (lastHoverObj != null)
                    {
                        if (lastHoverObj != hitForward.collider.gameObject)
                        {
                            if (lastHoverObj.GetComponent<HoverUseEvent>() != null)
                            {
                                lastHoverObj.GetComponent<HoverUseEvent>().avaliable = false;
                            }
                        }
                    }
                    lastHoverObj = hitForward.collider.gameObject;
                    hitForward.collider.gameObject.GetComponent<HoverUseEvent>().avaliable = true;
                }
                else
                {
                    if (lastHoverObj != null)
                    {
                        if (lastHoverObj.GetComponent<HoverUseEvent>() != null)
                        {
                            lastHoverObj.GetComponent<HoverUseEvent>().avaliable = false;
                        }
                    }
                }
                if (hitForward.collider.gameObject.GetComponent<WorldObject>() != null)
                {
                    if (hitForward.collider.gameObject.GetComponent<Rigidbody>() != null && hitForward.collider.gameObject.GetComponent<WorldObject>().ContainsTag("Pushable"))
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            hitForward.collider.gameObject.GetComponent<Rigidbody>().velocity = cameraTransform.transform.forward * pushingForce;
                        }
                    }
                    if (hitForward.collider.gameObject.GetComponent<WorldObject>().ContainsTag("Grabable") && currentlyBeingHeldObject == null)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            currentlyBeingHeldObject = hitForward.collider.gameObject;
                            if (currentlyBeingHeldObject.GetComponent<ConstantForce>() != null)
                            {
                                currentlyBeingHeldObject.GetComponent<ConstantForce>().enabled = false;
                            }
                            if (currentlyBeingHeldObject.GetComponent<BoxCollider>() != null)
                            {
                                currentlyBeingHeldObject.GetComponent<BoxCollider>().enabled = true;
                            }
                            justGrabbed = true;
                        }
                    }
                }
            }
            else
            {
                tooltipMan.HideTooltip();
                lastTooltipObjClicked = false;
                Debug.DrawRay(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            }
        }
        else
        {
            tooltipMan.HideTooltip();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouseOut = false;

            invenControl.controlingInventory = true;

            if (!moveVarLocked)
            {
                canMove = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseOut = true;

            invenControl.controlingInventory = false;
        }

        if (cheats)
        {
        }

        if (mouseOut)
        {
            canMove = false;
        }

        bool passed = true;
        foreach (Transform place in crouchTests)
        {
            RaycastHit hit;
            if (Physics.Raycast(place.transform.position, place.transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    //Ignore
                }
                else
                {
                    if (hit.distance <= .35f)
                    {
                        passed = false;
                        Debug.DrawRay(place.transform.position, place.transform.TransformDirection(Vector3.up) * 1000, Color.red);
                    }
                    else
                    {
                        Debug.DrawRay(place.transform.position, place.transform.TransformDirection(Vector3.up) * 1000, Color.green);
                    }
                }
            }

            if (!passed)
            {
                canCrouch = false;
            }
            else
            {
                canCrouch = true;
            }
        }

        if (!canMove || mouseOut)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentlyBeingHeldObject != null)
        {
            currentlyBeingHeldObject.GetComponent<Rigidbody>().velocity = cameraTransform.transform.forward * pushingForce;

            if (currentlyBeingHeldObject.GetComponent<ConstantForce>() != null)
            {
                currentlyBeingHeldObject.GetComponent<ConstantForce>().enabled = true;
            }
            if (currentlyBeingHeldObject.GetComponent<BoxCollider>() != null)
            {
                currentlyBeingHeldObject.GetComponent<BoxCollider>().enabled = true;
            }
            currentlyBeingHeldObject = null;
        }
        if (Input.GetKeyDown(KeyCode.E) && currentlyBeingHeldObject != null)
        {
            if (justGrabbed)
            {
                justGrabbed = false;
            }
            else
            {
                if (currentlyBeingHeldObject.GetComponent<ConstantForce>() != null)
                {
                    currentlyBeingHeldObject.GetComponent<ConstantForce>().enabled = true;
                }
                if (currentlyBeingHeldObject.GetComponent<BoxCollider>() != null)
                {
                    currentlyBeingHeldObject.GetComponent<BoxCollider>().enabled = true;
                }
                currentlyBeingHeldObject = null;
            }
        }

        finalSpeed = 1.5f;

        if (!isCrouching)
        {
            if (input.Run && currentSprintTime > 0 && canRun)
            {
                finalSpeed = baseCrouchSpeed * speedStepMultiplierRun;
            }
            else
            {
                finalSpeed = currentWalkingSpeed * speedStepMultiplierWalk;
            }
        }
        else
        {
            finalSpeed = currentRunningSpeed * speedStepMultiplierCrouch;
        }

        if (_isGrounded && _rigidbody.velocity.sqrMagnitude > 0.1f && i_timeSinceLastStep >= finalSpeed)
        {
            if (currentWalkingMat != null)
            {
                int random = UnityEngine.Random.Range(0, currentWalkingMat.footstepSounds.Count);
                if (currentWalkingMat.footstepSounds.Count > 0)
                {
                    AudioSource step = Instantiate(baseFootstepObj, footstepSoundOrigin.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                    step.clip = currentWalkingMat.footstepSounds[random];
                    step.volume = 0;// currentWalkingMat.footstepSoundVolume[random];
                    step.Play();
                }

                i_timeSinceLastStep = 0;
            }
        }

        cameraParent.position = transform.position + transform.TransformVector(armPosition);

        if (canCrouch)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Crouch
                isCrouching = true;

                if (unCrouch != null)
                {
                    StopCoroutine(unCrouch);
                }
                if (crouch != null)
                {
                    StopCoroutine(crouch);
                }

                unCrouch = null;
                crouch = StartCoroutine(Crouch());

                currentWalkingSpeed = baseCrouchSpeed;
                currentRunningSpeed = baseCrouchSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                isCrouching = false;

                if (crouch != null)
                {
                    StopCoroutine(crouch);
                }

                if (unCrouch == null)
                {
                    unCrouch = StartCoroutine(UnCrouch());
                }

                currentWalkingSpeed = baseWalkSpeed;
                currentRunningSpeed = baseRunSpeed;
            }
        }
    }

    IEnumerator Crouch()
    {
        float dist = _collider.size.y - crouchHeight;
        float distCent = Mathf.Abs(_collider.center.y) - Mathf.Abs(-.12f);

        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0);
            _collider.size = new Vector3(_collider.size.x, _collider.size.y - dist / 25, _collider.size.z);
            _collider.center = new Vector3(_collider.center.x, _collider.center.y + distCent / 25, _collider.center.z);
        }
    }

    IEnumerator UnCrouch()
    {
        float dist = normalHeight - _collider.size.y;
        float distCent = Mathf.Abs(-.4f) - Mathf.Abs(_collider.center.y);

        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0);
            _collider.size = new Vector3(_collider.size.x, _collider.size.y + dist / 25, _collider.size.z);
            _collider.center = new Vector3(_collider.center.x, _collider.center.y - distCent / 25, _collider.center.z);
        }
    }

    
    private bool CheckCollisionsWithWalls(Vector3 velocity)
    {
        if (_isGrounded) return false;
        var bounds = _collider.bounds;
        var radius = _collider.size.x;
        var halfHeight = _collider.size.y * 0.5f - radius * 1.0f;
        var point1 = bounds.center;
        point1.y += halfHeight;
        var point2 = bounds.center;
        point2.y -= halfHeight;
        Physics.CapsuleCastNonAlloc(point1, point2, radius, velocity.normalized, _wallCastResults,
            radius * 0.04f, ~0, QueryTriggerInteraction.Ignore);
        var collides = _wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider);
        if (!collides) return false;
        for (var i = 0; i < _wallCastResults.Length; i++)
        {
            _wallCastResults[i] = new RaycastHit();
        }

        return true;
    }


    public void SetAmbience(int id)
    {
        if (id == -2)
        {
            ambience.Stop();
            target = null;
            current = null;
            ambience.volume = 0;
            ambience.clip = null;
        }
        else if (id != -1)
        {
            target = ambiences[id].ambientTrack;
            targetVolume = ambiences[id].volumeToPlayAt;
        }
        else
        {
            target = null;
        }
    }

    #endregion

    #region Embedded Scripts

    /// A helper for assistance with smoothing the camera rotation.
    private class SmoothRotation
    {
        private float _current;
        private float _currentVelocity;

        public SmoothRotation(float startAngle)
        {
            _current = startAngle;
        }

        /// Returns the smoothed rotation.
        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
        }

        public float Current
        {
            set { _current = value; }
        }
    }

    /// A helper for assistance with smoothing the movement.
    private class SmoothVelocity
    {
        private float _current;
        private float _currentVelocity;

        /// Returns the smoothed velocity.
        public float Update(float target, float smoothTime)
        {
            return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);
        }

        public float Current
        {
            set { _current = value; }
        }
    }

    /// Input mappings
    [Serializable]
    private class FpsInput
    {
        [Tooltip("The name of the virtual axis mapped to rotate the camera around the y axis."),
         SerializeField]
        private string rotateX = "Mouse X";

        [Tooltip("The name of the virtual axis mapped to rotate the camera around the x axis."),
         SerializeField]
        private string rotateY = "Mouse Y";

        [Tooltip("The name of the virtual axis mapped to move the character back and forth."),
         SerializeField]
        private string move = "Horizontal";

        [Tooltip("The name of the virtual axis mapped to move the character left and right."),
         SerializeField]
        private string strafe = "Vertical";

        [Tooltip("The name of the virtual button mapped to run."),
         SerializeField]
        private string run = "Fire3";

        [Tooltip("The name of the virtual button mapped to jump."),
         SerializeField]
        private string jump = "Jump";

        /// Returns the value of the virtual axis mapped to rotate the camera around the y axis.
        public float RotateX
        {
            get { return Input.GetAxisRaw(rotateX); }
        }

        /// Returns the value of the virtual axis mapped to rotate the camera around the x axis.        
        public float RotateY
        {
            get { return Input.GetAxisRaw(rotateY); }
        }

        /// Returns the value of the virtual axis mapped to move the character back and forth.        
        public float Move
        {
            get { return Input.GetAxisRaw(move); }
        }

        /// Returns the value of the virtual axis mapped to move the character left and right.         
        public float Strafe
        {
            get { return Input.GetAxisRaw(strafe); }
        }

        /// Returns true while the virtual button mapped to run is held down.          
        public bool Run
        {
            get { return Input.GetButton(run); }
        }

        /// Returns true during the frame the user pressed down the virtual button mapped to jump.          
        public bool Jump
        {
            get { return Input.GetButtonDown(jump); }
        }
    }

    #endregion

    public void OnDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseOut = true;
        dead = true;
        deathCover.SetActive(true);

        invenControl.controlingInventory = false;
    }

    private void StartAlt()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            physics = this.gameObject.AddComponent<Rigidbody>();
            physics.freezeRotation = true;
            physics.useGravity = false;
        }
        else
        {
            physics = GetComponent<Rigidbody>();
            physics.freezeRotation = true;
            physics.useGravity = false;
        }

        if (physicsMaterial == null)
        {
            physicsMaterial = Resources.Load<PhysicMaterial>("Materials/Default");
        }

        if (colliderType == "Box")
        {
            if (GetComponent<BoxCollider>() == null)
            {
                collider = this.gameObject.AddComponent<BoxCollider>();
                collider.material = physicsMaterial;
            }
            else
            {
                collider = GetComponent<BoxCollider>();
                collider.material = physicsMaterial;
            }
        }
        else if (colliderType == "Capsule")
        {
            if (GetComponent<CapsuleCollider>() == null)
            {
                collider = this.gameObject.AddComponent<CapsuleCollider>();
                collider.material = physicsMaterial;
            }
            else
            {
                collider = GetComponent<CapsuleCollider>();
                collider.material = physicsMaterial;
            }
        }

        StartCoroutine(ReduceBhopSpeed());
    }

    private void UpdateAlt()
    {
        if (!cheats && noClip)
        {
            noClip = false;
        }

        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, layerMask))
        {
            if (!funMovement)
            {
                if (hit.distance < 0.5f)
                {
                    if (hit.collider.gameObject.tag == "Ground")
                    {
                        if (!grounds.Contains(hit.collider.gameObject))
                        {
                            StartCoroutine(AddToBunnyhop(hit.collider.gameObject));
                            grounds.Add(hit.collider.gameObject);
                        }
                        jump = true;
                    }
                    else
                    {
                        grounds.Clear();
                    }
                }
                else
                {
                    grounds.Clear();
                    jump = false;
                }
            }
            else
            {
                if (hit.distance < 2)
                {
                    if (hit.collider.gameObject.tag == "Ground")
                    {
                        if (!grounds.Contains(hit.collider.gameObject))
                        {
                            StartCoroutine(AddToBunnyhop(hit.collider.gameObject));
                            grounds.Add(hit.collider.gameObject);
                        }
                        jump = true;
                    }
                    else
                    {
                        grounds.Clear();
                    }
                }
                else
                {
                    grounds.Clear();
                    jump = false;
                }
            }
            
        }

        if (!funMovement)
        {
            if (_isGrounded)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
        else
        {
            if (grounds.Count > 0)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }
        }
        if (jump && !w && !a && !s && !d && !noClip)
        {
            physics.velocity = new Vector3(0, physics.velocity.y, 0);
        }
        if (noClip && !w && !a && !s && !d)
        {
            physics.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            if (jump && !inJump)
            {
                inJump = true;
                physics.velocity += this.transform.up * jumpPower;
                StartCoroutine(ReAllowJump());
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shift = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shift = false;
        }

        if (currentSprintTime <= 0 || !canRun)
        {
            shift = false;
        }

        if (Input.GetKeyDown(KeyCode.V) && objectCamera != null && cheats)
        {
            if (noClip)
            {
                noClip = false;
                GetComponent<ConstantForce>().enabled = true;
                collider.enabled = true;
            }
            else
            {
                noClip = true;
                GetComponent<ConstantForce>().enabled = false;
                collider.enabled = false;
                grounds.Clear();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            w = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            w = false;
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            a = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            a = false;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            s = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            s = false;
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            d = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            d = false;
        }

        if (stopVelControl)
        {
            return;
        }

        if (canMove)
        {
            if (!noClip)
            {
                if (jump || noClip)
                {
                    //Player Is Not In Air
                    if (w)
                    {
                        if (shift)
                        {
                            physics.velocity += this.transform.forward * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity += this.transform.forward * currentWalkingSpeed;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= this.transform.forward * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity -= this.transform.forward * currentWalkingSpeed;
                        }
                    }
                    if (a)
                    {
                        if (shift)
                        {
                            physics.velocity += -this.transform.right * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity += -this.transform.right * currentWalkingSpeed;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= -this.transform.right * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity -= -this.transform.right * currentWalkingSpeed;
                        }
                    }

                    if (s)
                    {
                        if (shift)
                        {
                            physics.velocity += -this.transform.forward * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity += -this.transform.forward * currentWalkingSpeed;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= -this.transform.forward * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity -= -this.transform.forward * currentWalkingSpeed;
                        }
                    }
                    if (d)
                    {
                        if (shift)
                        {
                            physics.velocity += this.transform.right * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity += this.transform.right * currentWalkingSpeed;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= this.transform.right * currentRunningSpeed;
                        }
                        else
                        {
                            physics.velocity -= this.transform.right * currentWalkingSpeed;
                        }
                    }
                }
                else
                {
                    if (w)
                    {
                        if (shift)
                        {
                            physics.velocity += this.transform.forward * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity += this.transform.forward * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= this.transform.forward * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity -= this.transform.forward * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    if (a)
                    {
                        if (shift)
                        {
                            physics.velocity += -this.transform.right * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity += -this.transform.right * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= -this.transform.right * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity -= -this.transform.right * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }

                    if (s)
                    {
                        if (shift)
                        {
                            physics.velocity += -this.transform.forward * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity += -this.transform.forward * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= -this.transform.forward * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity -= -this.transform.forward * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    if (d)
                    {
                        if (shift)
                        {
                            physics.velocity += this.transform.right * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity += this.transform.right * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                    else
                    {
                        if (shift)
                        {
                            physics.velocity -= this.transform.right * currentRunningSpeed * airMovementMultiplier;
                        }
                        else
                        {
                            physics.velocity -= this.transform.right * currentWalkingSpeed * airMovementMultiplier;
                        }
                    }
                }
            }
            else if (noClip)
            {

                if (w)
                {
                    if (shift)
                    {
                        physics.velocity += objectCamera.transform.forward * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity += objectCamera.transform.forward * currentWalkingSpeed;
                    }
                }
                else
                {
                    if (shift)
                    {
                        physics.velocity -= objectCamera.transform.forward * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity -= objectCamera.transform.forward * currentWalkingSpeed;
                    }
                }
                if (a)
                {
                    if (shift)
                    {
                        physics.velocity += -objectCamera.transform.right * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity += -objectCamera.transform.right * currentWalkingSpeed;
                    }
                }
                else
                {
                    if (shift)
                    {
                        physics.velocity -= -objectCamera.transform.right * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity -= -objectCamera.transform.right * currentWalkingSpeed;
                    }
                }

                if (s)
                {
                    if (shift)
                    {
                        physics.velocity += -objectCamera.transform.forward * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity += -objectCamera.transform.forward * currentWalkingSpeed;
                    }
                }
                else
                {
                    if (shift)
                    {
                        physics.velocity -= -objectCamera.transform.forward * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity -= -objectCamera.transform.forward * currentWalkingSpeed;
                    }
                }
                if (d)
                {
                    if (shift)
                    {
                        physics.velocity += objectCamera.transform.right * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity += objectCamera.transform.right * currentWalkingSpeed;
                    }
                }
                else
                {
                    if (shift)
                    {
                        physics.velocity -= objectCamera.transform.right * currentRunningSpeed;
                    }
                    else
                    {
                        physics.velocity -= objectCamera.transform.right * currentWalkingSpeed;
                    }
                }

                physics.velocity *= 5;
            }
        }

        Vector3 newVel = physics.velocity;
        Vector3 speedNew = physics.velocity;

        if (!noClip)
        {
            if (jump || !jump)
            {
                if (shift)
                {
                    if (physics.velocity.magnitude > speedLimitRunning)
                    {
                        speedNew.y = 0;
                        speedNew.Normalize();
                        speedNew *= speedLimitRunning;
                        speedNew.y = physics.velocity.y;
                    }
                }
                else
                {
                    if (physics.velocity.magnitude > speedLimit)
                    {
                        speedNew.y = 0;
                        speedNew.Normalize();
                        speedNew *= speedLimit;
                        speedNew.y = physics.velocity.y;
                    }
                }
            }
            else
            {
                if (physics.velocity.magnitude > speedLimit)
                {
                    speedNew.y = 0;
                    speedNew.Normalize();
                    speedNew *= speedLimit;
                    speedNew.y = physics.velocity.y;
                }
            }
        }
        else
        {
            if (jump || !jump)
            {
                if (shift)
                {
                    if (physics.velocity.magnitude > speedLimitRunning)
                    {
                        speedNew.Normalize();
                        speedNew *= speedLimitRunning;
                    }
                }
                else
                {
                    if (physics.velocity.magnitude > speedLimit)
                    {
                        speedNew.Normalize();
                        speedNew *= speedLimit;
                    }
                }
            }
            else
            {
                if (physics.velocity.magnitude > speedLimit)
                {
                    speedNew.Normalize();
                    speedNew *= speedLimit;
                }
            }
        }

        if (!noClip)
        {
            Vector3 benefit = new Vector3();

            if (w)
            {
                benefit += objectCamera.transform.forward * speedBenefit;
            }
            if (a)
            {
                benefit += -objectCamera.transform.right * speedBenefit;
            }
            if (s)
            {
                benefit += -objectCamera.transform.forward * speedBenefit;
            }
            if (d)
            {
                benefit += objectCamera.transform.right * speedBenefit;
            }

            benefit.y = 0;
            speedNew += benefit;
        }

        Vector3 vectNew = speedNew;
        vectNew *= speedBenefitSecondary;

        speedNew.x = vectNew.x;
        speedNew.z = vectNew.z;

        physics.velocity = speedNew;
    }

    IEnumerator AddToBunnyhop(GameObject obj)
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(.1f);
            if (!grounds.Contains(obj))
            {
                //Player left ground in time, add speed!
                if (w || a || s || d)
                {
                    if (jumpsPerformed != 10)
                    {
                        jumpsPerformed += 1;
                    }
                    if (speedBenefit < 10.0f)
                    {
                        speedBenefit += 0.5f * jumpsPerformed;
                    }
                }
                yield break;
            }
        }
        jumpsPerformed = 0;
    }

    IEnumerator ReduceBhopSpeed()
    {
        yield return new WaitForSeconds(0.025f);

        if (!w && !a && !s && !d)
        {
            speedBenefit = 0.0f;
        }

        if (speedBenefit - 0.1f > 0)
        {
            speedBenefit -= 0.1f;
        }
        else
        {
            speedBenefit = 0;
        }

        StartCoroutine(ReduceBhopSpeed());
    }

    IEnumerator ReAllowJump()
    {
        yield return new WaitForSeconds(0.25f);
        inJump = false;
    }
}