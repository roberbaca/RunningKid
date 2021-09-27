using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator anim;

    private bool isRunning = false;         // bandera para saber si el nivel comenzo
    public bool isSpinning = false;         // bandera para saber si player esta atacando
    private bool isGrounded = true;         // bandera para saber si player esta en el piso
    
    // Movimiento
    private Vector3 direction;
    private Vector3 targetPosition;
    private Vector3 dir;
    public float jumpForce = 4f;             // fuerza del salto
    public float speed = 7f;                 // velocidad de movimiento
    public float turnSpeed = 0.05f;          // velocidad de rotacion del player cuando se mueve entre lineas


    public float gravity = 12f;              // graveded
    private float verticalVelocity;          // velocidad de caida
    private int lane = 1;                    // linea en la que se encuentra el player (0 = izquierda, 1 = centro, 2 = derecha)
    private const float laneDistance = 2f;   // ancho de cada linea por la que el personaje puede correr

    private Vector3 rotation;


    // Sound SFX
    public AudioSource spinSFX;
    public AudioSource woahSFX;
    public AudioSource jumpSFX;
    public AudioSource whooshSFX;

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


        if(isGrounded) // solo nos podemos mover de carril si estamos en el piso
        {  
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

        
        // chequeamos si el personaje esta en el suelo...
        IsGrounded();
        anim.SetBool("IsGrounded", isGrounded);

        // calculo del vector de movimiento
        direction = Vector3.zero;     
        direction.x = (targetPosition - transform.position).x * speed;

        // si el personaje esta en el suelo...
        if (isGrounded && !GameManager.Instance.isGamePaused)
        {
            verticalVelocity = -0.1f; // para mantenerno pegados al suelo todo el tiempo
        
            // Controles por teclado
            if (Input.GetKeyDown(KeyCode.Space)) // jump
            {            
                Jump();
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
            if (MobileInput.Instance.SwipeUp) // jump
            {
                Jump();
            }

            else if (MobileInput.Instance.SwipeDown) // slide
            {               
                StartSliding(); 
                Invoke("StopSliding", 1f);
            }

            else if (MobileInput.Instance.DoubleTap) // spin
            {
                StartSpinning();
                Invoke("StopSpinning", 1f);
            }

        }
        else // si no estamos en el suelo entonces aplicamos gravedad en el eje vertical
        {
            verticalVelocity -= (gravity * Time.deltaTime);
        }

        // aplicamos gravedad
        direction.y = verticalVelocity; 
        direction.z = speed;      

        // movimiento del personaje
        controller.Move(direction * Time.deltaTime);
        dir = controller.velocity; 
       

        
        if (!isSpinning)
        {
            if (dir != Vector3.zero)
            {
                //Vector3 NextDir = new Vector3(-1f, 0, 0);

                dir.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, dir, turnSpeed);

                //transform.rotation = Quaternion.LookRotation(NextDir);

            }
        }

     
    }

    private void MoveLane(bool goingRight)
    {

        // si nos movemos a la izquierda...
        /*
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
        */

        lane += (goingRight) ? 1 : -1;

        lane = Mathf.Clamp(lane, 0, 2);
    }

  

    private bool IsGrounded()
    {

        // casting a ray
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, controller.bounds.center.y - controller.bounds.extents.y + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1f);

        isGrounded = Physics.Raycast(groundRay, 0.3f);        

        return isGrounded;                     
    }

    public void Jump()
    {
        jumpSFX.Play();
        anim.SetTrigger("Jump");
        verticalVelocity = jumpForce;
    }

    public void StartRunning()
    {
        isRunning = true;    
        anim.SetTrigger("Running");
    }

    public void StopRunning()
    {
        isRunning = false;
        anim.SetTrigger("Victory");
    }


    public void StartSliding()
    {
        anim.SetBool("Sliding", true);
        whooshSFX.Play();

        // al deslizar el personaje, modificamos el collider para no colisionar con objetos. Disminuimos a la mitad la altura y el centro.    
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    public void StopSliding()
    {
        anim.SetBool("Sliding", false);

        // Volvemos a escalar el collider a las dimensiones originales... 
        controller.height *= 2;       
        controller.center = new Vector3(controller.center.x, controller.center.y * 2f, controller.center.z);
    }

    public void StartSpinning()
    {
        spinSFX.Play();
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
        woahSFX.Play();
        anim.SetTrigger("Death");
        controller.height = 0;
        controller.center = new Vector3(controller.center.x, controller.center.y, controller.center.z);
        isRunning = false;
        GameManager.Instance.OnGameOver();
    }
  

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {    
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;               
        }
    } 
}
