using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public Camera mainCamera;
    public float movementSpeed;
    public float dashCd;
    public float dashSpeed;
    public float dashDuration;
    public float smoothRoof;
    public GameObject cameraTarget;
    public Material roofShader;
    public LayerMask roof;
    public bool IsDashing { private set; get; }

    Rigidbody _rigidBody;
    Quaternion _rot;
    Vector3 lookPos;
    Vector3 aux;
    float fade = 1;
    float horizontalInput;
    float verticalInput;
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
        _particles = new List<ParticleSystem>();
        dashDurationAux = dashDuration;
        GetComponentsInChildren(false, _particles);
    }

    private void Update()
    {
        if (Physics.Raycast(cameraTarget.transform.position, Vector3.up, float.MaxValue, roof))
        {
            fade = Mathf.Lerp(fade, 0.1f, smoothRoof);
            roofShader.SetFloat("_AlphaValue", fade);
        }

        else
        {
            fade = Mathf.Lerp(fade, 1, smoothRoof * 1.5f);
            roofShader.SetFloat("_AlphaValue", fade);
        }

        // rotation with mouse or joystick
        if (new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical")) != Vector2.zero)
            JoystickRotation();
        else if (aux != Input.mousePosition)
            MouseRotation();

        aux = Input.mousePosition;

        if (horizontalInput > 0 || verticalInput > 0 || horizontalInput < 0 || verticalInput < 0)
        {
            _animator.SetBool("Run", true);
        }

        else
        {
            _animator.SetBool("Run", false);
        }
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 inputMovement = new Vector3(horizontalInput, 0, verticalInput);
        var velocity = GetVelocity(inputMovement);
        CheckGroundStatus();
        if (!isGrounded)
            velocity.y = velocity.y - 6f;

        _rigidBody.velocity = velocity;
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

    private void JoystickRotation()
    {
        float _angle = Mathf.Atan2(Input.GetAxis("RightStickHorizontal"), -Input.GetAxis("RightStickVertical")) * Mathf.Rad2Deg;
        // detecto imput 
        if (new Vector2(Input.GetAxis("RightStickHorizontal"), Input.GetAxis("RightStickVertical")) != Vector2.zero)
            _rot = Quaternion.AngleAxis(_angle, new Vector3(0, 1, 0));
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, 15 * Time.deltaTime);
    }

    private void MouseRotation()
    {
        Ray rayCam = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayCam, out hit, 100))
            lookPos = hit.point;

        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;
        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.MaxValue))
        {
            isGrounded = hitInfo.distance < 0.1f ? true : false;
        }
    }
}
