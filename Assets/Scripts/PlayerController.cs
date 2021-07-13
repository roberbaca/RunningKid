using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private const float LANE_DISTANCE = 3f;
    private const float TURN_SPEED = 0.05f;

    // Animator
    private Animator anim;


    // Movimiento
    private CharacterController controller;
    public float jumpForce = 4f;
    private float gravity = 12f;
    private float verticalVelocity;
    public float speed = 7f;
    private int desiredLane = 1; // linea en la que se encuentra el personaje (0 = izquierda, 1 = centro, 2 = derecha)
    private bool isGrounded;

 


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // Input para saber en que linea esta el personaje
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
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


        isGrounded = IsGrounded();

        anim.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            verticalVelocity = -0.1f;
        

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce; // jump
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

        return (Physics.Raycast(groundRay, 0.2f + 0.1f));
        
    }
}
