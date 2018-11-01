using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Header("Player Controls")]
    public float speed;
    public Transform camera;
    public Vector3 currentMovementForce;

	//public bool nearShrine;

    private float orbMax = 50;
	private float orbCount;
    private Rigidbody rb;
    private float orbIncrementSpeed = 0.1f;
	private float treeSpeed;
	private float treeSlow = 0.7f;
	private float originalSpeed;
	private float startingSpeed;
    private float originalVAcceleration;
	private int treeCount = 0;
	private float vel;
    //private GameObject[] pickups;
    //private HandleObjects handleObjects;
    //private Vector3 positionStamp;
    //private float shake;
    //private GameObject dialogueManager;

    private float verticalAcceleration;
    private float verticalSpeed = 0;

    //[Header("Collision Handeling")]
    //public Collider playerCollider;
    //public float shakeAmount;

    //[Header("UI")]
    //public Image windPowerBar;
    //public GameObject turnBackText;
    //public GameObject miniMap;
    //public Text orbCountText;
    

    void Start()
    {
       // miniMap.SetActive(true);
        rb = GetComponent<Rigidbody>();
		//rb.drag = 1;
		//rb.angularDrag = 1;
        //orbCount = 0;
		verticalAcceleration = GetComponent<PlayerCollision> ().GetVerticalAccel ();
		startingSpeed = speed;
		originalSpeed = speed;
        originalVAcceleration = 0.001f;
		treeSpeed = treeSlow * speed;
        //pickups = GameObject.FindGameObjectsWithTag("PickUp");
        //turnBackText.SetActive(false);
        //orbCountText.text = orbCount.ToString()+"/" + orbMax;
        //shakeAmount = 0.05f;
        //shake = 0;
        //dialogueManager = GameObject.Find("DialogueManager");
    }

    void FixedUpdate()
    {
		verticalAcceleration = GetComponent<PlayerCollision> ().GetVerticalAccel ();
		orbCount = GetComponent<PlayerCollision> ().GetOrbCount ();
        //MOVEMENT
        //Go Up
        if (Input.GetButton("Jump") || Input.GetButton("AButton"))
        {
            // rb.AddForce(Vector3.up * (speed * 20));
            verticalSpeed += verticalAcceleration;
        }
        else
        {
            if (verticalSpeed > 0)
            {
                verticalSpeed -= verticalAcceleration;
				if (verticalSpeed < 0) {
					verticalSpeed = 0;
				}
            }
        }
        transform.position += Vector3.up * verticalSpeed;
        //Go Down
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetButton("BButton")) && transform.position.y > 3)
        {
            // rb.AddForce(-Vector3.up * (speed * 29));
            verticalSpeed -= verticalAcceleration;
        }
        else
        {
            if (verticalSpeed < 0)
            {
                verticalSpeed += verticalAcceleration;
				if (verticalSpeed > 0) {
					verticalSpeed = 0;
				}
            }
        }
		//Debug.Log (verticalSpeed);
        //Moving Forward and Backwards
        if (Input.GetButton("Sprint"))
        {
            //Debug.Log("Running");
            rb.AddForce(camera.forward * Input.GetAxis("Vertical") * (speed * 2));
            rb.AddForce(camera.up * Input.GetAxis("Vertical") * (speed * 2));
            rb.AddForce(camera.right * Input.GetAxis("Horizontal") * (speed * 2));
        }
        else
        {
            //Debug.Log("Walking");
            rb.AddForce(camera.forward * Input.GetAxis("Vertical") * speed);
            rb.AddForce(camera.up * Input.GetAxis("Vertical") * speed);
            rb.AddForce(camera.right * Input.GetAxis("Horizontal") * speed);
        }
        rb.AddForce(-rb.velocity);

        currentMovementForce = camera.forward * Input.GetAxis("Vertical") * speed +
            camera.up * Input.GetAxis("Vertical") * speed +
            camera.right * Input.GetAxis("Horizontal") * speed;

        ModeChange ();

    }

	void ModeChange () {
		//Debug.Log ("Current Speed: " + rb.velocity);
		if (rb.velocity.x > 10f || rb.velocity.z > 10f || rb.velocity.x < -10f || rb.velocity.z < -10f) {
			//Debug.Log ("Going fast!");
		}
	}

	public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
		//windPowerBar.fillAmount = orbCount / orbMax;
		//orbCountText.text = orbCount.ToString() + "/" + orbMax;
		//Assuming that the passed in newOrbCount is 0, which it should be
		verticalAcceleration = 0.001f;
		speed = startingSpeed;
		originalSpeed = startingSpeed;
		treeSpeed = treeSlow * startingSpeed;
		Debug.Log ("OrbCount = " + orbCount);
		Debug.Log ("Speed is back to :" + speed);
	}

	public float GetOrbCount(){
		return orbCount;
	}


	//void OnCollisionEnter(Collider other)
	//{
	//	if (other.tag == "PassThrough") {
	//		Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
	//	}
	//}
}