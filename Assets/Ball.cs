using NUnit.Framework.Internal;
using UnityEngine;
public class Ball : MonoBehaviour
{

    Rigidbody2D rb;

    public float minVelocity = 2f;
    public float maxVelocity = 5f;
    [SerializeField]  public  float speedMultiplier = 1.01f;

    public Padel Padel1; 
    public Padel Padel2;





    public Vector2 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    void Start()
    {
        LaunchBall();
    }

    private void FixedUpdate()
    {
        lastVel = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Vector2 normal = collision.contacts[0].normal;


        Vector2 refecltVelocity = Vector2.Reflect(lastVel, normal); 

        rb.linearVelocity = refecltVelocity * speedMultiplier;

        //Debug.Log("Refelcted !" + speedMultiplier + "  current speed = " + rb.linearVelocity); 

        EventBus.Instance.TriggerReflection(); 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.name == "Left Gaol")
        {
            EventBus.Instance.TriggerGoal(-1);


            Invoke("LaunchBall", 0f);
        }
        if(  collision.gameObject.name == "Right Goal")
        {
            EventBus.Instance.TriggerGoal(1);

            Invoke("LaunchBall", 2f);
        }
    }


    public void ReestBall()
    {

        LaunchBall();

    }

    private void LaunchBall()
    {
        transform.position = Vector2.zero;
        float angle = Random.Range(0, Mathf.PI * 2);
        float speed = Random.Range(minVelocity, maxVelocity);

        Vector2 initial_velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;

        rb.linearVelocity = initial_velocity;
    }




    //is this file too croweded ?
}
