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
    private CircleCollider2D ballCollider; 

    

    //AI stuff
    Vector2 collisionAt;
    public bool isAiControlled; // serizeilized field 5ir


    private void Awake()
    {
        startingVelocity = velocity;
        rb = GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
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
        if (isAiControlled)
        {
            if (axisName == "Vertical P2")
            {
                AiMove(6, 7);
            }

            if (axisName == "Vertical P1")
            {
                AiMove(-7, -6);
            }

        }

        else myInput = UnityEngine.Input.GetAxis(axisName);
    }

    private void FixedUpdate()
    {
        collisionAt = NextCollision(); 
        Debug.Log("next collisoin ==" + collisionAt);
        //Debug.Log("Padel speed = " + velocity);
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
        velocity *= ball.speedMultiplier;
        
    }


    private int GetAiInput()
    {
        return 0; 
    }


    private Vector2 NextCollision()

    {
        Vector2 reflectedVel= ball.lastVel.normalized; 

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;  // include triggers
        filter.SetLayerMask(~(1 << LayerMask.NameToLayer("Ball")));

        int iteration = 0;
        int maxIterations = 100;

        Vector2 distance;

        RaycastHit2D[] hits = new RaycastHit2D[10]; // store results here
        Physics2D.CircleCast(
            ball.transform.position,
            ballCollider.radius,
            reflectedVel,
            filter,
            hits,
            Mathf.Infinity
        );

        if (hits[0].collider == null)
        {
            Debug.LogWarning("CircleCast did not hit anything!");
            return Vector2.zero;
        }
        while (!hits[0].collider.name.Contains("AI wall helper"))
        {
            reflectedVel = Vector2.Reflect(reflectedVel, hits[0].normal);

            distance = hits[0].normal * (ballCollider.radius + 0.01f) ;


            Physics2D.CircleCast(
                    hits[0].point + distance,
                    0.5f,
                    reflectedVel.normalized,
                    filter,
                    hits,
                    Mathf.Infinity
                );

            ; 

            iteration++;

            if (hits[0].collider == null)
            {
                Debug.LogWarning("CircleCast did not hit anything!");
                return Vector2.zero; 
            }  

            //Debug.Log(" hits at" + hits[0].point + "hit by " + hits[0].collider.name);
            if (iteration >= maxIterations)
            {
                Debug.Log("itetation excceded"); return Vector2.zero;
            }
        }


        return hits[0].point;

    }

    void AiMove(float x1, float x2)
    {
        if (collisionAt.x > x1 && collisionAt.x < x2)
        {
            if (transform.position.y > collisionAt.y)
            {
                Debug.LogWarning(transform.position.y + "  but collision is happenit at " + collisionAt.y);
                myInput = -1;
            }

            else
            {
                myInput = 1;
            }
        }

        //colision not the 6-7 range
        else
        {
            if (transform.position.y < 0)
            {
                myInput = 1;
            }
            else
            {
                myInput = -1;
            }
        }



    }

}

//ki tousel magnituded 10 wa9tha 1.01 multiplier  , ki tousel 100 , 00.01 multiplier and so on ?