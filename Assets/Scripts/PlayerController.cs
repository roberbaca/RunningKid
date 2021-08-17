using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator anim;

    private bool isRunning = false;         // bandera para saber si el nivel comenzo
    public bool isSpinning = false;        // bandera para saber si player esta atacando
    private bool isGrounded = true;         // bandera para saber si player esta en el piso
    
    // Movimiento
    private Vector3 direction;
    private Vector3 targetPosition;
    public float jumpForce = 4f;            // fuerza del salto
    public float speed = 7f;                // velocidad de movimiento
    private const float turnSpeed = 0.05f;   // velocidad de rotacion del player cuando se mueve entre lineas


    public float gravity = 12f;              // graveded
    private float verticalVelocity;          // velocidad de caida
    private int lane = 1;                    // linea en la que se encuentra el player (0 = izquierda, 1 = centro, 2 = derecha)
    private const float laneDistance = 2f;   // ancho de cada linea por la que el personaje puede correr


    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        if(!isRunning)
        {
            return;
        }


        // Input por teclado
        if (Input.GetKeyDown(KeyCode.LeftArrow))
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
        targetPosition = transform.position.z * Vector3.forward;

        if (lane == 0) // si deseamos mover el player a la izquierda...
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (lane == 2)  // si deseamos mover el player a la derecha...
        {
            targetPosition += Vector3.right * laneDistance;
        }
        else
        {
            // estamos en la linea del medio
        }


        

        IsGrounded();
        anim.SetBool("IsGrounded", isGrounded);

        // calculo del vector de movimiento
        direction = Vector3.zero;
        //direction.x = (targetPosition - transform.position).normalized.x * speed;
        direction.x = (targetPosition - transform.position).x * speed;

        // chequeamos si el personaje esta en el suelo
        if (isGrounded)
        {
            verticalVelocity = -0.1f; // para mantenerno pegados al suelo todo el tiempo
        
            if (Input.GetKeyDown(KeyCode.Space)) // jump
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce; 
            }

            else if (Input.GetKeyDown(KeyCode.LeftControl)) // slide
            {
                StartSliding(); 
                Invoke("StopSliding", 1f);
            }

            else if (Input.GetKeyDown(KeyCode.G)) // spin
            {
                StartSpinning(); 
                Invoke("StopSpinning", 1f);
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

            else if (MobileInput.Instance.DoubleTap)
            {
                StartSpinning();
                Invoke("StopSpinning", 1f);
            }

        }
        else // si no estamos en el suelo...
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            //fast falling. Podemos tocar el boton Space para caer mas rapido ???
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
      
        direction.y = verticalVelocity; // aplicamos gravedad
        direction.z = speed;

        /*
        if (isGrounded)
        {
           
        }
        else
        {
            direction.z = speed * 0.8f; // si estamos en el aire, avanzamos mas lento en el eje Z
        }
        */

        // movimiento del personaje
        controller.Move(direction * Time.deltaTime);


        // Rotacion del personaje en el sentido de movimiento. Para atacar y romper cajas...
        if (!isSpinning)
        {
            Vector3 dir = controller.velocity; // giramos el personaje cuando cambiamos de linea

            if (dir != Vector3.zero)
            {
                dir.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, dir, turnSpeed);

            }
        }

     
    }

    private void MoveLane(bool goingRight)
    {

        // si nos movemos a la izquierda...
        if(!goingRight)
        {
            lane--;

            if (lane == -1) // no nos podemos mover, ya estamos en el margen izquierdo
            {
                lane = 0;
            }
        }
        else // nos movemos a la derecha
        {
            lane++;

            if (lane == 3) // no nos podemos mover, ya estamos en el margen derecho
            {
                lane = 2;
            }
        }

    }

  

    private bool IsGrounded()
    {
        // casting a ray
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, controller.bounds.center.y - controller.bounds.extents.y + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1f);

        isGrounded = Physics.Raycast(groundRay, 0.3f);        

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

        // al deslizar el personaje, modificamos el collider para no colisionar con objetos. Disminuimos a la mitad la altura y el centro.
        //controller.height /= 2;
        controller.height /= 3;
        //controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
        controller.center = new Vector3(controller.center.x, controller.center.y / 7.5f, controller.center.z);


    }

    public void StopSliding()
    {
        anim.SetBool("Sliding", false);

        // Volvemos a escalar el collider a las dimensiones originales...
        //controller.height *= 2;
        controller.height *= 3;
        //controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
        controller.center = new Vector3(controller.center.x, controller.center.y * 7.5f, controller.center.z);
    }



    public void StartSpinning()
    {
        anim.SetBool("Spinning", true);
        isSpinning = true;
    }

    public void StopSpinning()
    {
        anim.SetBool("Spinning", false);
        isSpinning = false;
    }


    public void Crash()
    {
        // player dies        
        anim.SetTrigger("Death");
        controller.height = 0;
        controller.center = new Vector3(controller.center.x, controller.center.y, controller.center.z);
        isRunning = false;
        //GameManager.Instance.isDead = true;
        GameManager.Instance.OnGameOver();
    }


    public void DisableCollisions()
    {
        controller.detectCollisions = false;        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       
        if(isRunning)
        {
            switch (hit.gameObject.tag)
            {
                case "Obstacle":
                    Crash();
                    break;
                case "FracturedBox":
                    //DisableCollisions();
                    break;              
            }
        }

       

    }

}
