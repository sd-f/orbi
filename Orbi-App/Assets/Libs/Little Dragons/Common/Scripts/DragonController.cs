using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;



public class DragonController : MonoBehaviour
{
    #region Variables 

    public enum DragoType
    {
        Tiger = 0, Mouse = 1, Sea = 2, Coming_Soon = 3
    }
    public enum Ground
    {
        walk = 1, trot = 2, run = 3
    }
    public enum ActionsEmotions //Every Int value represents the IDAction on the Animator
    {
        Dig = 1,
        Eat = 2,
        Pet = 3,
        LevelUp = 4,
        Talk = 5,
        Sleep = 6,
        Happy = 101,
        Sad = 102,
        Angry = 103,
        Scared = 104,
        Sneeze = 105,
        Confuse = 106
    }

    #region Drago Components 
    private Animator anim;
    private Transform DragoTransform;
    private Rigidbody dragoRigidBody;
    private CapsuleCollider dragoCollider;
    private Transform Cam;
    #endregion

    #region Animator Parameters Variables
    private bool
        speed1,
        speed2,
        speed3,
        jump,
        shift,
        down,
        damage,
        fly,
        dodge,
        fall,
        death,
        swim, isInWater, underWater,
        grounded,
        attack1,
        attack2,
        stun,
        action,
        stand = true;
        
    private float
        horizontal,
        vertical,
        upDown,
        jumpPoint,
        dragoFloat,
        speed,
        direction,
        groundSpeed = 1f,
        flyspeedanimator,
        waterlevel,
        dragoHeight;

    private int
        dragoInt = 1,
        actionID = -1,
        tired;
    #endregion

    #region Inspector Entries
    public DragoType DragonType = DragoType.Tiger;
    [Header("Camera Move Input")]
    public bool cameraMove;
    [Tooltip("Activate Camera Y Axis also while flying and underwater swimming")]
    public bool UpDownAxis;

    [Header("Ground")]

    [Tooltip("Specify wich layer is the ground")]
    public LayerMask GroundLayer;
    public Ground StartSpeed;
    [Space]
    [Tooltip("Add Walk Speed greater than 1 the dragon will Slide")]
    public float WalkSpeed = 1f;
    [Tooltip("Add Trot Speed greater than 1 the dragon will Slide")]
    public float TrotSpeed = 1f;
    [Tooltip("Add Run Speed greater than 1 the dragon will Slide")]
    public float RunSpeed = 1f;
    [Space]
    [Tooltip("Add Turn Speed .... Zero will rotate with the default animation rotation")]
    public float TurnSpeed = 0f;
    [Space]
    [Space]
    public int GotoSleep;


    [Header("Air")]
    public float flySpeed = 1f;
    public float flyTurn = 0f;
    [Range(0.2f,1f)]
    public float flyAnimationSpeed = 1;


    [Header("Water")]

    [Tooltip("Water Level for the dragon to Swim on the water")]
    public float waterLine = 0f;
    public float swimSpeed = 1f;
    public float swimTurn = 0f;

    [Header("Underwater")]
    public float UnderSpeed = 1f;
    public float UnderTurn = 0f;
    #endregion

    [Header("Water")]

    #region Modify_the_Position_Variables
    RaycastHit hit_Hip, hit_Chest;
    Vector3 Drago_Hip, Drago_Chest;
    float 
        turnAmount, 
        forwardAmount,
        scaleFactor,
        maxHeight;
    Pivots[] pivots;
    #endregion

    #region Properties

    public float JumpPoint    {
        set { jumpPoint = value; }
        get { return this.jumpPoint; }
    }

    public float GroundSpeed    {
        set { groundSpeed = value; }
        get { return this.groundSpeed; }
    }

    public float MaxHeight    {
        set { maxHeight = value; }
        get { return this.maxHeight; }
    }

    public int DragoInt    {
        set { dragoInt = value; }
        get { return this.dragoInt; }
    }

    public int Tired    {
        set { tired = value; }
        get { return this.tired; }
    }

    public float DragoFloat    {
        set { dragoFloat = value; }
        get { return this.dragoFloat; }
    }

    public bool IsInWater    {
        set { isInWater = value; }
        get { return this.isInWater; }
    }

    public bool UnderWater    {
        set { underWater = value; }
        get { return this.underWater; }
    }

    public float Horizontal    {
        get { return horizontal; }
        set { horizontal = value; }
    }

    public float Vertical    {
        get { return vertical; }
        set { vertical = value; }
    }

    public float UpDown    {
        get { return upDown; }
        set { upDown = value; }
    }

    public bool Speed1    {
        get { return speed1; }
        set { speed1 = value; }
    }

    public bool Speed2    {
        get { return speed2; }
        set { speed2 = value; }
    }

    public bool Speed3    {
        get { return speed3; }
        set { speed3 = value; }
    }

    public bool Jump    {
        get { return jump; }
        set { jump = value; }
    }
    public bool Shift    {
        get { return shift; }
        set { shift = value; }
    }
    public bool Down    {
        get { return down; }
        set { down = value; }
    }

    public bool Damage    {
        get { return damage; }
        set { damage = value; }
    }
    public bool Fly    {
        get { return fly; }
        set { fly = value; }
    }
    public bool Dodge    {
        get { return dodge; }
        set { dodge = value; }
    }
    public bool Death    {
        get { return death; }
        set { death = value; }
    }

    public bool Attack1    {
        get { return attack1; }
        set { attack1 = value; }
    }
    public bool Attack2{
        get { return attack2; }
        set { attack2 = value; }
    }

    public bool Stun{
        get { return stun; }
        set { stun = value; }
    }

    public bool Action{
        get { return action; }
        set { action = value; }
    }

    public int ActionID    {
        get { return actionID; }
        set { actionID = value; }
    }

    #endregion

    #endregion

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        DragoTransform = transform;
        dragoCollider = GetComponent<CapsuleCollider>();
        dragoRigidBody = GetComponent<Rigidbody>();
        pivots = GetComponentsInChildren<Pivots>(); //Pivots are Strategically Transform objects use to cast rays used by the drago
        scaleFactor = DragoTransform.localScale.y;  //TOTALLY SCALABE DRAGO
        dragoHeight = pivots[1].transform.localPosition.y;
        groundSpeed = (int)StartSpeed;
        flyspeedanimator = flyAnimationSpeed;
        anim.SetInteger("Type", (int)DragonType);
    }

    public void SetForwardAmount(float amount)
    {
        this.forwardAmount = amount;
    }

    public void CameraMove(Vector3 move)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.   
        if (move.magnitude > 1f) move.Normalize();

        move = transform.InverseTransformDirection(move);
        turnAmount = Mathf.Atan2(move.x, move.z);
        forwardAmount = move.z;


        //Up & Down movement while flying or swiming;
        if (UpDownAxis && !jump && !down)
        {
            if (fly || underWater)
            {
                float a = move.y;
                if (a > 0) a = a * 1.8f;
                upDown = Mathf.Lerp(upDown, a, Time.deltaTime * 5f);
            }
        }
    }

    //----------------------Linking  the Parameters to the Animator-----------------------------------------------------------------------------
    void LinkingAnimator(Animator anim_)
    {
        anim_.SetFloat(HashIDsDragons.verticalHash, vertical * speed);
        anim_.SetFloat(HashIDsDragons.horizontalHash, direction);
        anim_.SetFloat(HashIDsDragons.updownHash, upDown);
        anim_.SetFloat(HashIDsDragons.flySpeedHash, Mathf.Lerp(anim_.GetFloat(HashIDsDragons.flySpeedHash), flyspeedanimator, Time.deltaTime * 5f));
        anim_.SetBool(HashIDsDragons.shiftHash, shift);
        anim_.SetBool(HashIDsDragons.standHash, stand);
        anim_.SetBool(HashIDsDragons.jumpHash, jump);
        anim_.SetBool(HashIDsDragons.attack1Hash, attack1);
        anim_.SetBool(HashIDsDragons.attack2Hash, attack2);
        anim_.SetBool(HashIDsDragons.injuredHash, damage);
        anim_.SetBool(HashIDsDragons.flyHash, fly);
        anim_.SetBool(HashIDsDragons.fallHash, fall);
        anim_.SetBool(HashIDsDragons.dodgeHash, dodge);
        anim_.SetBool(HashIDsDragons.stunnedHash, stun);
        anim_.SetBool(HashIDsDragons.swimHash, swim);
        anim_.SetBool(HashIDsDragons.underWaterHash, underWater);
        anim_.SetBool(HashIDsDragons.action, action);
        anim_.SetInteger(HashIDsDragons.actionID, actionID);

        if (fly)
        {
            anim_.SetFloat(HashIDsDragons.floatDragonHash, dragoFloat);
        }
        anim_.SetBool(HashIDsDragons.groundedHash, grounded);

        if (death)
            anim_.SetTrigger(HashIDsDragons.deathHash); //Triggers the Death
    }

    //--Add more Rotations to the current Turn animations -------------------------------------------
    void TurnAmount()
    {
        float Turn;

        if (fly)
        {
            Turn = flyTurn;
        }
        else if (swim)
        {
            Turn = swimTurn;
        }
        else
        {
            Turn = TurnSpeed;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Locomotion")) Turn = 0;
          
        }

        if (vertical >= 0)
        {
            DragoTransform.Rotate(DragoTransform.up, Turn * 3 * horizontal * Time.deltaTime);
        }
        else
        {
            DragoTransform.Rotate(DragoTransform.up, Turn * 3 * -horizontal * Time.deltaTime);
        }

        //More Rotation when jumping and falling... in air rotation------------------
        if (isJumping() || fall && !fly && !swim && !stun)
        {
            if (vertical >= 0)
                DragoTransform.Rotate(DragoTransform.up, 100 * horizontal * Time.deltaTime);
            else
                DragoTransform.Rotate(DragoTransform.up, 100 * -horizontal * Time.deltaTime);
        }
    }

    //--Add more Speed to the current Move animations--------------------------------------------
    void SpeedAmount()
    {
        float amount = 0;
        float axis = vertical;
        Vector3 direction = DragoTransform.forward;

        if (swim && !underWater || anim.GetCurrentAnimatorStateInfo(0).IsName("Swim Jump"))
        {
            amount = swimSpeed;
        }
        else if (underWater)
        {
            amount = UnderSpeed;
        }
        else if (fly)
        {
            amount = flySpeed;

            if (vertical >= 0.1)
            {
                if (jump) direction = (DragoTransform.forward + DragoTransform.up).normalized;
                if (down) direction = (DragoTransform.forward - DragoTransform.up).normalized;
            }
            else
            {
                axis = upDown;
                if (jump || down) direction = Vector3.up;
            }
        }
        else
        {
            if (groundSpeed == 1) amount = WalkSpeed;
            if (groundSpeed == 2) amount = TrotSpeed;
            if (groundSpeed == 3) amount = RunSpeed;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Locomotion"))
            {
                amount = 0;
            }
        }

        DragoTransform.position = Vector3.Lerp(DragoTransform.position, DragoTransform.position + direction * amount * axis / 5f, Time.deltaTime);
    }

    //------------------------------------------Terrain Logic----------------------------------
    void FixPosition()
    {
        Drago_Hip = pivots[0].transform.position;
        Drago_Chest = pivots[1].transform.position;

        //Ray From Hip to the ground
        if (Physics.Raycast(Drago_Hip, -DragoTransform.up, out hit_Hip, 0.5f * scaleFactor, GroundLayer))
        {
            Debug.DrawRay(hit_Hip.point, hit_Hip.normal * 0.02f, Color.blue);
        }
        //Ray From Chest to the ground
        if (Physics.Raycast(Drago_Chest, -DragoTransform.up, out hit_Chest, 0.5f * scaleFactor, GroundLayer))
        {
            Debug.DrawRay(hit_Chest.point, hit_Chest.normal * 0.02f, Color.red);
        }

        //Smoothy rotate until is Aling with the Horizontal
        if (fly || swim && !underWater)
        {
            float amount = 10f;
            if (swim) amount = 8;

            Quaternion finalRot = Quaternion.FromToRotation(DragoTransform.up, Vector3.up) * dragoRigidBody.rotation;

            if (Vector3.Angle(DragoTransform.up, Vector3.up) > 0.1f)
                DragoTransform.rotation = Quaternion.Lerp(DragoTransform.rotation, finalRot, Time.deltaTime * amount);
            else
                DragoTransform.rotation = finalRot;
        }
        else
        {
            //------------------------------------------------Terrain Adjusment--------------------------------------------

            //---------------------------------Calculate the Align vector of the terrain-----------------------------------
            Vector3 direction = (hit_Chest.point - hit_Hip.point).normalized;
            Vector3 Side = Vector3.Cross(Vector3.up, direction).normalized;
            Vector3 SurfaceNormal = Vector3.Cross(direction, Side).normalized;
            float angleTerrain = Vector3.Angle(DragoTransform.up, SurfaceNormal);

            // ------------------------------------------Orient To Terrain--------------------------------------------------  
            Quaternion finalRot = Quaternion.FromToRotation(DragoTransform.up, SurfaceNormal) * dragoRigidBody.rotation;

            // If the dragon is falling, jumping or flying smoothly aling with the horizontal
            if (fall || isJumping(0.7f, true))
            {
                finalRot = Quaternion.FromToRotation(DragoTransform.up, Vector3.up) * dragoRigidBody.rotation;
                DragoTransform.rotation = Quaternion.Lerp(DragoTransform.rotation, finalRot, Time.deltaTime * 10f);
            }
            else
            {
                // if the terrain changes hard smoothly adjust to the terrain  ground
                if (angleTerrain > 0.2f)
                {
                    DragoTransform.rotation = Quaternion.Lerp(DragoTransform.rotation, finalRot, Time.deltaTime * 10f);
                }
                else
                {
                    DragoTransform.rotation = finalRot;
                }
            }
        }
    }

    //--------------------------------------Falling Logic----------------------------------------------------------
    void Falling()
    {
        RaycastHit hitpos;
        
        if (Physics.Raycast(dragoCollider.bounds.center, -DragoTransform.up, out hitpos, 0.9f * scaleFactor, GroundLayer))
        {
            //This will avoid to go UpHill
            if (hitpos.normal.y > .707f)
            {
                fall = false;
            }
            else
            {
                fall = true;
            }
        }
        else
        {
            fall = true;
        }
    }

    void Swimming()
    {
        RaycastHit WaterHitCenter;

        //Front RayWater Cast
        if (Physics.Raycast(pivots[2].transform.position, -DragoTransform.up, out WaterHitCenter, dragoHeight * scaleFactor * 3, LayerMask.GetMask("Water")))
        {
            waterlevel = WaterHitCenter.transform.position.y; //get the water level when find water
            isInWater = true;
        }
       else
        {
            isInWater = false;
        }

        if (isInWater) //if we hit water
        {
            if ((Drago_Chest.y < waterlevel && !swim) || (fall && !fly && !isJumping()))
            {
                swim = true;
                dragoRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
            }
            //Stop swimming when he is coming out of the water
            if (hit_Chest.distance < dragoHeight * scaleFactor)
            {
                swim = false;
                dragoRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                dragoRigidBody.useGravity = true;
            }
        }

        if (swim)
        {
            fall = false;
            fly = false;
            
            //Smoothy Move until is Aling with the Water
            if (!isJumping())
            {
                dragoRigidBody.useGravity = true;
                DragoTransform.position = Vector3.Lerp(DragoTransform.position, new Vector3(DragoTransform.position.x, waterlevel - dragoHeight + waterLine, DragoTransform.position.z), Time.deltaTime * 5f);
            }
            else
            {
                dragoRigidBody.useGravity = false;
                dragoRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }

            if (upDown != 0) upDown = Mathf.Lerp(upDown, 0, Time.fixedDeltaTime * 5);

            //-------------------Go UnderWater---------------
            if (down && !isJumping())
            {
                underWater = true;
                anim.applyRootMotion = false;
                dragoRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }

            if (isJumping(0.5f, true) && !isInWater)
            {
                swim = false;
                dragoRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
        }
    }

    void UnderWaterMovement()
    {
        YAxisMovement(2);
        dragoRigidBody.drag = 100;
        int shiftpeed = 1;
        if (shift) shiftpeed = 3;
       
        //Forwards Movement
        if (vertical > 0 || upDown != 0)
        {
            DragoTransform.position = Vector3.Lerp(DragoTransform.position, DragoTransform.position + DragoTransform.forward * UnderSpeed *shiftpeed* Mathf.Max(vertical, Mathf.Abs(upDown)) / 2, Time.fixedDeltaTime);
        }

        //Rotation left/right


        transform.RotateAround(Vector3.up, UnderTurn * horizontal * Time.fixedDeltaTime);

        //  transform.Rotate(Vector3.up, 100 * horizontal * Time.fixedDeltaTime, Space.World);

        if ((Vector3.Angle(transform.forward, Vector3.up) > 30 && jump) || (Vector3.Angle(transform.forward, Vector3.up) < 170 && down) || UpDownAxis && cameraMove) //Limit Up Down Axis
        {
            transform.RotateAround(transform.right, 2 * -upDown * Time.fixedDeltaTime);
        }

        if (!jump && !down)
        {
            upDown = Mathf.Lerp(upDown, 0, Time.fixedDeltaTime * 2);
        }

        //To Get Out of the Water---------------------------------
        RaycastHit UnderWaterHit;

        if (Physics.Raycast(pivots[2].transform.position, -Vector3.up, out UnderWaterHit, scaleFactor * 1, LayerMask.GetMask("Water")))
        {
            Debug.DrawRay(pivots[2].transform.position, -Vector3.up * scaleFactor * 1, Color.blue);
            if (!down)
            {
                underWater = false;
                anim.applyRootMotion = true;
                dragoRigidBody.drag = 0;
            }
        }
    }

    void YAxisMovement(float v)
    {
        if (jump)
        {
            upDown = Mathf.Lerp(upDown, 1, Time.deltaTime * v);
        }
        else if (down)
        {
            upDown = Mathf.Lerp(upDown, -1, Time.deltaTime * v);
        }
        else
        {
            upDown = Mathf.Lerp(upDown, 0, Time.deltaTime * v);
        }
    }

    void Grounded()
    {
        RaycastHit hitGrounded;

        if (Physics.Raycast(pivots[1].transform.position, -DragoTransform.up, out hitGrounded, dragoHeight * 1.1f * scaleFactor, GroundLayer))
        {
            Debug.DrawRay(pivots[1].transform.position, -DragoTransform.up * dragoHeight * scaleFactor, Color.blue);
            if (isJumping(0.5f, true))
            {
                grounded = false;
            }
            else
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }
    }

    //--------------------------------------------------------------------Check if the in the Jumping State-------------------------------------------------------------------------------------
    //***------------------------------------------ this will return false if is not in the Jumping state or if is not in the desired half of the jump***------------------------------------------
    bool isJumping(float normalizedtime, bool half)
    {
        if (half)  //if is jumping the first half
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedtime)
                    return true;
            }

            if (anim.GetNextAnimatorStateInfo(0).IsTag("Jump"))  //if is transitioning to jump
            {
                if (anim.GetNextAnimatorStateInfo(0).normalizedTime < normalizedtime)
                    return true;
            }
        }
        else //if is jumping the second half
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > normalizedtime)
                    return true;
            }

            if (anim.GetNextAnimatorStateInfo(0).IsTag("Jump"))  //if is transitioning to jump
            {
                if (anim.GetNextAnimatorStateInfo(0).normalizedTime > normalizedtime)
                    return true;
            }
        }
        return false;
    }
    bool isJumping()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            return true;
        }
        if (anim.GetNextAnimatorStateInfo(0).IsTag("Jump"))
        {
            return true;
        }
        return false;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    void FixedUpdate()
    {
        if (!underWater)
        {
            FixPosition();
            Falling();
            Swimming();
        }
        else
        {
            UnderWaterMovement();
        }
    }
    void Update()
    {
        Grounded();
        TurnAmount();
        SpeedAmount();

        //If CameraInput Mode is Activated
        if (cameraMove)
        {
            vertical = forwardAmount;
            horizontal = turnAmount;

            //More Rotation While aiming with the camera
            if (!underWater && anim.GetCurrentAnimatorStateInfo(0).IsTag("Locomotion") || anim.GetCurrentAnimatorStateInfo(0).IsTag("Fly"))
            {
                transform.Rotate(Vector3.up, horizontal * Time.deltaTime * 150);
            }
        }


        //Check if the Dragon is Stand
        if ((horizontal != 0) || (vertical != 0) || Tired >= GotoSleep)
            stand = false;
        else stand = true;


       

        //Change velocity on ground
        if (!fly && !swim)
        {
            if (speed1) groundSpeed = 1f;
            if (speed2) groundSpeed = 2f;
            if (speed3) groundSpeed = 3f;
        }
        else if (fly)
        {
            if (speed1) flyspeedanimator = flyAnimationSpeed;
            if (speed2) flyspeedanimator = flyAnimationSpeed + 0.25f;
            if (speed3) flyspeedanimator = flyAnimationSpeed + 0.35f;
        }
       
        int shiftSpeed = 1;

        float directionmult = 1; // for Strafe in air in horizontal 
       
        //Shift Key Changes Fly mode    
        if (shift)
        {
           shiftSpeed = 2;
            
            if (fly)
            {
                directionmult = 2; //changue in the animator fly blendtree to horizontal :2f: that stores the strafe animation while flying
                DragoFloat = Mathf.Lerp(DragoFloat, 1, Time.deltaTime * 5f); // .... Press Shift input to Glide
            }
           
        }
        else
        {
            if (fly)
                DragoFloat = Mathf.Lerp(DragoFloat, 0, Time.deltaTime * 5f); //Glide off
        }

        float maxspeed = groundSpeed;

        if (swim)
        {
            maxspeed =1;
        }


        speed = Mathf.Lerp(speed, maxspeed * shiftSpeed, Time.deltaTime * 2f);            //smoothly transitions bettwen velocities


        direction = Mathf.Lerp(direction, horizontal * directionmult, Time.deltaTime * 8f);    //smoothly transitions bettwen directions

        if (fly)
            YAxisMovement(2f); //--------------------Controls the Fly Movement Up and Down

        if (jump || attack2 || damage || stun) stand = false; //Stand False when doing some action

        //Fly close to the ground;
        if (grounded) fly = false;

        //Reset Sleep
        if (!stand || attack1 || attack2 || jump || shift || swim || fly) Tired = 0;

        if (!swim && !fly) upDown = 0;

        LinkingAnimator(anim);
    }
}

