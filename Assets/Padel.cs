using UnityEngine;
using UnityEngine.Windows;

public class Padel : MonoBehaviour
{

    public Vector2 velocity;
    public string axisName;
    Vector2 startingVelocity;

    public Ball ball;

    private float myInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        startingVelocity = velocity;
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {
        EventBus.Instance.OnGoal += ResetPadel;
        EventBus.Instance.OnReflection += IncreasePadelSpeed;
    }

    private void OnDisable()
    {
        EventBus.Instance.OnGoal -= ResetPadel;
        EventBus.Instance.OnReflection -= IncreasePadelSpeed;
    }

    void Start()
    {

    }

    void Update()
    {

        myInput = UnityEngine.Input.GetAxis(axisName);
    }

    private void FixedUpdate()
    {
        Debug.Log("Padel speed = " + velocity);
        if (myInput == 1)
        {
           
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        else if (myInput == -1)
        {
            rb.MovePosition(rb.position - velocity * Time.fixedDeltaTime);
        }

        if (transform.position.y > 3.8) transform.position = new Vector3( transform.position.x , 3.8f  , 0) ; 
        if(transform.position.y < -3.8) transform.position = new Vector3(transform.position.x, -3.8f, 0);
    }


    public   void ResetPadel(int x)
    {
        velocity = startingVelocity;

        if ( axisName == "Vertical P1")
        {
            transform.position =new Vector3(-7, 0, 0);
        }

        else if (axisName == "Vertical P2")
        {
            transform.position =new Vector3(7 , 0 , 0);
        }
    }

    private void  IncreasePadelSpeed()
    {
        Debug.Log("before : " + velocity);
        velocity *= ball.speedMultiplier;
        Debug.Log("After : " + velocity); 
    }

}

//ki tousel magnituded 10 wa9tha 1.01 multiplier  , ki tousel 100 , 00.01 multiplier and so on ?