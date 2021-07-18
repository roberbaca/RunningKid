using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private const float LANE_DISTANCE = 3f;
    private const float TURN_SPEED = 0.05f;

    private bool isRunning = false; // bandera para saber si el nivel comenzo
    private bool isGrounded = true; // bandera para saber si el personaje esta en el piso

    // Animator
    private Animator anim;


    // Movimiento
    private CharacterController controller;
    public float jumpForce = 4f;
    private float gravity = 12f;
    private float verticalVelocity;     
    private int desiredLane = 1; // linea en la que se encuentra el personaje (0 = izquierda, 1 = centro, 2 = derecha)



    // Speed Modifier
    public float originalSpeed = 7f;
    public float speed = 7f;
    private float speedIncreaseLastTic;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRunning)
        {
            return;
        }

        /* MODIFIER SPEED*/

        if(Time.time - speedIncreaseLastTic > speedIncreaseTime)
        {
            speedIncreaseLastTic = Time.time;
            speed += speedIncreaseAmount;

            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        // Input por teclado
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(true);
        }


        // Controles tactiles:

        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }

        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }


        // definimos el target en funcion de la posicion actual
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }
        else
        {
            // estamos en la linea del medio
        }

        // calculo del vector de movimiento
        Vector3 moveVector = Vector3.zero;

        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        IsGrounded();

        anim.SetBool("IsGrounded", isGrounded);

        // chequeamos si el personaje esta en el suelo
        if (isGrounded)
        {
            verticalVelocity = -0.1f;
        

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce; // jump
            }

            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                StartSliding(); // slide
                Invoke("StopSliding", 1f);
            }



            //controles tactiles
            if (MobileInput.Instance.SwipeUp)
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce; // jump
            }

            else if (MobileInput.Instance.SwipeDown)
            {
                StartSliding(); // slide
                Invoke("StopSliding", 1f);
            }






        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //fast falling
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = -jumpForce;
            }

            // controles tactiles
            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }

        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;


        // movimiento del personaje
        controller.Move(moveVector * Time.deltaTime);

        // Rotacion del personaje en el sentido de movimiento
        Vector3 dir = controller.velocity;

        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }

        

    }

    private void MoveLane(bool goingRight)
    {
        if(!goingRight)
        {
            desiredLane--;

            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        else
        {
            desiredLane++;

            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }

    }

    private bool IsGrounded()
    {
        // casting a ray
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, controller.bounds.center.y - controller.bounds.extents.y + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1f);

        isGrounded = Physics.Raycast(groundRay, 0.2f + 0.1f);

        return isGrounded;
        
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("Running");
    }

    public void StartSliding()
    {
        anim.SetBool("Sliding", true);

        // al deslizar el personaje, modificar el collider. Disminuimos a la mitad la altura y el centro.
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);

    }

    public void StopSliding()
    {
        anim.SetBool("Sliding", false);

        // Volvemos a escalar el collider a las dimensiones originales
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void Crash()
    {
        // player dies
        anim.SetTrigger("Death");
        isRunning = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch(hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;
        }
    }

}
