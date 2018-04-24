using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    public Transform mainCamera;
    public float movementSpeed;
    public float dashCd;
    public float dashSpeed;
    public float dashDuration;
    public float movingTurnSpeed;
    public float stationaryTurnSpeed;
    public Material roofShader;
    public bool IsDashing { private set; get; }

    Rigidbody _rigidBody;
    Vector3 camForward;
    Vector3 relativeMove;
    Vector3 groundNormal;
    float horizontalInput;
    float verticalInput;
    float turnAmount;
    float forwardAmount;
    float dashTimer;
    float dashDurationAux;
    bool onePress;
    bool isGrounded;
    bool canActiveDashParticles = true;
    Animator _animator;
    List<ParticleSystem> _particles;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        dashDurationAux = dashDuration;

        _particles = new List<ParticleSystem>();

        GetComponentsInChildren(false, _particles);

    }

    private void Update()
    {
        if (roofShader.GetFloat("_AlphaValue") < 0.5f)
            roofShader.SetFloat("_AlphaValue", 1f);

        GetInputs();

    }

    private void FixedUpdate()
    {
        CalculateMoveDir();
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void CalculateMoveDir()
    {
        if (mainCamera != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(mainCamera.forward, new Vector3(1, 0, 1)).normalized;
            relativeMove = verticalInput * camForward + horizontalInput * mainCamera.right;
        }
        else
            //use world directions 
            relativeMove = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
        //move towards Dir
        Move(relativeMove);
    }
    void Move(Vector3 relMove)
    {
        if (relMove.magnitude > 1f)
            relMove.Normalize();

        // si esta dasheando tiene otro velocity.
        var velocity = GetVelocity(relMove);
        if (!isGrounded)
            velocity.y = velocity.y - 6f;

        // aplico movimiento
        _rigidBody.velocity = velocity;
        //animaciones mal hechas jaja
        if (horizontalInput > 0 || verticalInput > 0 || horizontalInput < 0 || verticalInput < 0)
        {
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }

        //rotation 
        relMove = transform.InverseTransformDirection(relMove);
        CheckGroundStatus();
        relMove = Vector3.ProjectOnPlane(relMove, groundNormal);
        turnAmount = Mathf.Atan2(relMove.x, relMove.z);
        forwardAmount = relMove.z;
        ApplyExtraTurnRotation();
    }

    Vector3 GetVelocity(Vector3 relMove)
    {
        Vector3 relVel;
        dashTimer += Time.deltaTime;
        IsDashing = false;

        // bool para que tengas que soltar el trigger despues de cada dash
        if (Input.GetAxis("RTrigger") == 0 && !onePress)
            onePress = true;

        // mientras que mantega apretado el input del dash, si no esta en cd y si no cumplio la duracion del dash
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("RTrigger") < 0 && dashTimer > dashCd && dashDuration > 0f && onePress)
        {
            IsDashing = true; // estoy dasheando
            _animator.SetBool("OnDash", true);

            if (_animator.GetBool("Run") && _animator.GetBool("OnDash") && canActiveDashParticles)
            {
                canActiveDashParticles = false;

                for (int i = 0; i < _particles.Count - 1; i++)
                {
                    if (_particles[i].name == "DashParticles")
                        if (!_particles[i].isPlaying)
                            _particles[i].Play();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            canActiveDashParticles = true;

        // estoy dasheando ? y todavia hay duracion
        if (IsDashing && dashDuration > 0f)
        {
            relVel = relMove * dashSpeed;
            dashDuration -= Time.deltaTime;
            return relVel;
        } // si solte el botton o me quede si duracion reseteo el dash.
        else if (!IsDashing && dashDuration < dashDurationAux)
        {
            dashDuration = dashDurationAux;
            dashTimer = 0f;
            onePress = false;
            _animator.SetBool("OnDash", false);
        }
        _animator.SetBool("OnDash", false);
        // sino.. retorno la velocidad normal del player
        return relVel = relMove * movementSpeed;
    }

    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.MaxValue))
        {
            if (hitInfo.distance < 0.1f)
            {
                groundNormal = hitInfo.normal;
                isGrounded = true;
            }
            else
            {
                groundNormal = Vector3.up;
                isGrounded = false;
            }
        }
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }
}
