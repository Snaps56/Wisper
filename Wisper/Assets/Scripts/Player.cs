using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [Header("Player Controls")]
    public float speed;
	public float throwPower;
    public Transform camera;
    public Vector3 currentMovementForce;

	public bool nearShrine;

    private float orbCount;
    private Rigidbody rb;
    private float orbIncrementSpeed = .1f;
	private float treeSpeed;
	private float treeSlow = 0.7f;
	private float originalSpeed;
	private int treeCount = 0;
	private float vel;
    private GameObject[] pickups;
    private HandleObjects handleObjects;

    private float verticalAcceleration = 0.001f;
    private float verticalSpeed = 0;

    [Header("UI")]
    public Image windPowerBar;
    public GameObject textBox;
    public GameObject miniMap;
    

    void Start()
    {
        miniMap.SetActive(true);
        rb = GetComponent<Rigidbody>();
		//rb.drag = 1;
		//rb.angularDrag = 1;
		throwPower = 100;
        orbCount = 0;
		originalSpeed = speed;
		treeSpeed = treeSlow * speed;
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
<<<<<<< HEAD
        textBox.SetActive(false);
=======
        turnBackText.SetActive(false);
        orbCountText.text = orbCount.ToString()+"/500";
        shakeAmount = 0.05f;
        shake = 0;
>>>>>>> 39ab2c9d7c919e034e2245322cba442cbd9288b0
    }

    void FixedUpdate()
    {
        //MOVEMENT
        //Go Up
        if (Input.GetButton("Jump") || Input.GetButton("AButton"))
        {
            // rb.AddForce(Vector3.up * (speed * 20));
            verticalSpeed += verticalAcceleration;
        }
        else
        {
            if (verticalSpeed >= 0)
            {
                verticalSpeed -= verticalAcceleration;
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
            if (verticalSpeed <= 0)
            {
                verticalSpeed += verticalAcceleration;
            }
        }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Orb")) {
            Destroy(other.gameObject);
<<<<<<< HEAD
            while (windPower < 500)
=======
            if (orbCount < 500)
>>>>>>> 39ab2c9d7c919e034e2245322cba442cbd9288b0
            {
                orbCount += 1;
            }
            windPowerBar.fillAmount = orbCount / 500; 
            speed += orbIncrementSpeed;
			originalSpeed = speed;
			treeSpeed = treeSlow * speed;
			throwPower += 2;
<<<<<<< HEAD
=======
			orbCountText.text = orbCount.ToString() + "/500";
>>>>>>> 39ab2c9d7c919e034e2245322cba442cbd9288b0
        }
        if (other.gameObject.CompareTag("Border"))
        {
            speed = speed * 0.1f;
            textBox.SetActive(true);
        }
        if (other.gameObject.CompareTag ("Tree")) {
			speed = treeSpeed;
			treeCount++;
			Debug.Log ("Speed is reduced to :" + speed);
		}
        if (other.gameObject.CompareTag ("PickUp")) {
			other.gameObject.GetComponent<HandleObjects> ().throwForce = throwPower;
		}
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = true;
		}
    }

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Tree")) {
			treeCount--;
			if (treeCount == 0) {
				speed = originalSpeed;
				Debug.Log ("Speed is back to :" + speed);
			}
		}
        if(other.gameObject.CompareTag("Border"))
        {
            textBox.SetActive(false);
            speed = originalSpeed;
        }
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = false;
		}
	}

	void ModeChange () {
		//Debug.Log ("Current Speed: " + rb.velocity);
		if (rb.velocity.x > 10f || rb.velocity.z > 10f || rb.velocity.x < -10f || rb.velocity.z < -10f) {
			//Debug.Log ("Going fast!");
		}
	}

    
	//void OnCollisionEnter(Collider other)
	//{
	//	if (other.tag == "PassThrough") {
	//		Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
	//	}
	//}
}