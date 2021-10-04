using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    private bool isGameStarted = false;      // bandera para saber si el nivel comenzo
    public bool isSpinning = false;          // bandera para saber si player esta atacando
    public bool isOnFloor = true;            // bandera para saber si player esta en el piso
    
    // Movimiento
    private Vector3 direction;               // vector de movimiento
    private Vector3 targetPosition;          // posicion deseada  
    private float verticalVelocity;          // velocidad de caida
    private int lane = 1;                    // linea en la que se encuentra el jugador (0 = izquierda, 1 = centro, 2 = derecha)
    private const float laneDistance = 2f;   // ancho de cada linea por la que el personaje puede correr

    [SerializeField] float jumpForce = 4f;   // fuerza del salto
    [SerializeField] float speed = 7f;       // velocidad de movimiento
    [SerializeField] float turnSpeed = 5f;   // velocidad de rotacion del jugador cuando se mueve entre lineas
    [SerializeField] float gravity = 12f;    // graveded
   

    // Sound SFX
    [SerializeField] AudioSource spinSFX;
    [SerializeField] AudioSource woahSFX;
    [SerializeField] AudioSource jumpSFX;
    [SerializeField] AudioSource whooshSFX;

    // isgrounded?
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        if(!isGameStarted)
        {
            return;
        }

       

        if (isOnFloor && !GameManager.Instance.isGamePaused)
        {

            verticalVelocity = -0.1f; // para mantenernos pegados al suelo todo el tiempo

            // Input por teclado
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) // izquierda
            {
                MoveLane(false);
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) // derecha
            {
                MoveLane(true);
            }             

           
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) // saltar
            {
                Jump();
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // deslizar
            {
                StartSliding();
                Invoke("StopSliding", 1f);
            }

            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) // girar
            {
                StartSpinning();
                Invoke("StopSpinning", 1f);
            }


            //controles tactiles        
            if (MobileInput.Instance.SwipeLeft) // izquierda
            {

                MoveLane(false);
            }

            if (MobileInput.Instance.SwipeRight) // derecha
            {

                MoveLane(true);
            }

            if (MobileInput.Instance.SwipeUp) // saltar
            {
                Jump();
            }

            if (MobileInput.Instance.SwipeDown) // deslizar
            {
                StartSliding();
                Invoke("StopSliding", 1f);
            }

            if (MobileInput.Instance.DoubleTap) // girar
            {
                StartSpinning();
                Invoke("StopSpinning", 1f);
            }

        }
        else // si no estamos en el suelo entonces aplicamos gravedad en el eje vertical
        {
            verticalVelocity -= (gravity * Time.deltaTime);
        }
              

        // calculamos la posicion deseada en funcion de la posicion actual
        targetPosition = transform.position.z * Vector3.forward;


        switch (lane)
        {
            case 0: // si deseamos mover el player a la izquierda...
                targetPosition += Vector3.left * laneDistance;
                break;

            case 2:// si deseamos mover el player a la derecha...
                targetPosition += Vector3.right * laneDistance;
                break;
        }    

        // calculo del vector de movimiento (direction)
        direction = Vector3.zero;     

        // en el eje X
        direction.x = (targetPosition - transform.position).x * speed;        

        // en el eje Y
        direction.y = verticalVelocity; 

        // en el eje Z
        direction.z = speed;      

        // movimiento del personaje
        controller.Move(direction * Time.deltaTime);


        // rotaacion del personaje en el sentido de movimiento
        Vector3 vel = controller.velocity;

        if (vel != Vector3.zero) // si estamos en movimiento
        {
            vel.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, vel, turnSpeed);
        }

        // chequeamos si el personaje esta en el suelo...       
        GroundCheck();
        anim.SetBool("IsGrounded", isOnFloor);

    }

    private void MoveLane(bool movingRight)
    {
        // si nos movemos a la izquierda...        
        if(!movingRight)
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

        lane = Mathf.Clamp(lane, 0, 2); // para asegurarnos que devuelva un numero entre 0 y 2
    }
  

    private bool GroundCheck()
    {
        // chequeamos si el personaje esta en el suelo...
        isOnFloor = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        return isOnFloor;
    }


    public void Jump()
    {
        jumpSFX.Play();
        anim.SetTrigger("Jump");
        verticalVelocity = jumpForce;
    }

    public void StartRunning()
    {
        isGameStarted = true;    
        anim.SetTrigger("Running");
    }

    public void StopRunning()
    {
        isGameStarted = false;
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
        // el jugador colisiono contra algun obstaculo y nos muestra el Game Over screen
        woahSFX.Play();
        anim.SetTrigger("Death");
        controller.height = 0;
        controller.center = new Vector3(controller.center.x, controller.center.y, controller.center.z);
        isGameStarted = false;
        GameManager.Instance.OnGameOver();
    }
  

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {    
        // chequeo de colisiones
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;               
        }
    } 
}
