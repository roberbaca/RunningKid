using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, doubleTap;
    private Vector2 swipeDelta, startTouch;
    private float lastTap;
    private float sqrDeadZone;

    [SerializeField] private float deadZone = 100f;
    [SerializeField] private float doubleTapDelta = 0.5f;

    public bool Tap { get { return tap; } }
    public bool DoubleTap { get { return doubleTap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
 

    // Start is called before the first frame update
    void Start()
    {
        sqrDeadZone = deadZone * deadZone;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Reseteamos todos las banderas
        doubleTap = false;
        tap = false;
        swipeLeft = false;
        swipeUp = false;
        swipeDown = false;
        swipeRight = false;


#if UNITY_EDITOR
        UpdateStandAlone();
#else
        UpdateMobile();
#endif
    }
    

    private void UpdateStandAlone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
            doubleTap = Time.time - lastTap < doubleTapDelta;
            lastTap = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }


        // reset distance, get the new swipeDelta
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.GetMouseButton(0))
            swipeDelta = (Vector2)Input.mousePosition - startTouch;

        //checking if delta is beyond Dead Zone
        if(swipeDelta.sqrMagnitude > sqrDeadZone)
        {
            // we ´re beyond the deadZone, this is swipe
            // Now we need to figure out in which direction it goes
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or right
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                // up or down
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }
                
            startTouch = swipeDelta = Vector2.zero;
        }
     }

    private void UpdateMobile()
    {
        if (Input.touches.Length != 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
                doubleTap = Time.time - lastTap < doubleTapDelta;
                Debug.Log(Time.time - lastTap);
                lastTap = Time.time;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }
        }

        // reset distence, calculate new one
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.touches.Length != 0)
            swipeDelta = (Vector2)Input.touches[0].position - startTouch;

        //checking if delta is beyond Dead Zone
        if (swipeDelta.sqrMagnitude > sqrDeadZone)
        {
            // we ´re beyond the deadZone, this is swipe
            // Now we need to figure out in which direction it goes
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or right
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                // up or down
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }

            startTouch = swipeDelta = Vector2.zero;
        }

    }

}




