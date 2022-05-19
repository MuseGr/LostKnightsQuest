/*      Player movement script with guide
    -Make one circle colider for legs of the character and one box colider for body
    -Set Gravity scale to 3.5 and mass to 2
    -Freez rotation on Z
    -Add slipury phisic material (friction 0) to the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    public static NewMovement Instance;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public LayerMask PlatformLayer;
    public Animator animator;
    public StaminaBar staminaBar;

    //Run
    private float movementDir;
    public float movementAcceleration;   //60
    public float maxMovemetnSepeed;      //7
    public float movementDecceleration; //10
    private bool changingDirection => (rb.velocity.x > 0f && movementDir < 0f) || (rb.velocity.x < 0f && movementDir > 0f);

    //Jump
    public bool isJumping;
    public float jumpForce; //22

    //Roll
    float timeOfRoll;
    public bool canRoll = true;
    public bool isRolling = false;
    public float rollDuration;

    public int maxRollStamina = 5;
    public int rollStamina;
    public float rollStaminaRegenRate = 1;
    float lastRollTime;

    //IsAbbleTo
    public bool ableToRun = true;
    public bool ableToJump = true;
    public bool ableToRoll = true;

    private void Start()
    {
        Instance = this;
        rollStamina = maxRollStamina;
        staminaBar.SetMaxStamina(maxRollStamina);
    }

    void Update()
    {
        movementDir = GetInput().x;
        CheckForJump();
        CheckForRoll();
        CheckForRollEnd();
    }
    private void FixedUpdate()
    {
        GetInput();
        FlippSpriteWhenChangingDir();
        Run();
        ApplyLinearDrag();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "jumpable")
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);

            ableToRoll = true;
            GetComponent<CombatScript>().canAttack = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "jumpable")
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void FlippSpriteWhenChangingDir()
    {
        switch (movementDir)
        {
            case 1f:
                sp.flipX = false;
                break;
            case -1f:
                sp.flipX = true;
                break;
        }
    }
    
    private void Run()
    {
        if (ableToRun)
        {

            animator.SetFloat("Speed", Mathf.Abs(movementDir));
            if (rb.velocity.x < maxMovemetnSepeed && rb.velocity.x > -maxMovemetnSepeed)
            {
                float speedDif = movementAcceleration - rb.velocity.x;
                rb.AddForce(new Vector2(movementDir * speedDif, rb.position.y), ForceMode2D.Force);
            }
        }
    }
    public void ApplyLinearDrag()
    {
        if (Mathf.Abs(movementDir) < 0.4f || changingDirection)
        {
            rb.drag = movementDecceleration;
        }
        else
        {
            rb.drag = 0f;
        }

    }

    private void CheckForJump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && ableToJump)
        {
            Jump();
        }
    }
    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        ableToRoll = false;
        GetComponent<CombatScript>().canAttack = false;
    }

    private void CheckForRoll()
    {
        //Regenerate Stamina
        if (rollStamina < maxRollStamina && Time.time >= lastRollTime + rollStaminaRegenRate)
        {
            rollStamina++;
            staminaBar.SetStamina(rollStamina);
            lastRollTime = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && rollStamina > 0 && ableToRoll)
        {
            Roll();
        }
    }
    private void Roll()
    {
        if (canRoll)
        {
            timeOfRoll = Time.time;
            isRolling = true;
            animator.SetBool("IsRolling", true);
            rollStamina--;

            staminaBar.SetStamina(rollStamina);

            gameObject.layer = LayerMask.NameToLayer("PlayerRoll");

            ableToRoll = false;
            ableToJump = false;
            GetComponent<CombatScript>().canAttack = false;
        }
    }
    private void CheckForRollEnd()
    {
        if(isRolling)
        {
            if (timeOfRoll + rollDuration <= Time.time)
            {
                isRolling = false;
                animator.SetBool("IsRolling", false);
                lastRollTime = Time.time;

                gameObject.layer = LayerMask.NameToLayer("Player");

                ableToRoll = true;
                ableToJump = true;
                GetComponent<CombatScript>().canAttack = true;
            }
        }
    }

    private bool mTriger = true;
    public void TrigerMovement()
    {
        switch (mTriger)
        {
            case false:
                ableToRun = true;
                ableToJump = true;
                ableToRoll = true;
                mTriger = true;
                break;
            case true:
                ableToRun = false;
                ableToJump = false;
                ableToRoll = false;
                mTriger = false;
                break;
        }
    }
    public void TriggerRoll()
    {
        switch (canRoll)
        {
            case true:
                canRoll = false;
                break;
            case false:
                canRoll = true;
                break;
        }
    }
    public void EnableRollOnSecondBlock()
    {
        if (!canRoll)
            canRoll = true;
        
    }

    public void RestoreStamina()
    {
        rollStamina = maxRollStamina;
        staminaBar.SetStamina(rollStamina);
    }
}
