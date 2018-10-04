using UnityEngine;
using UnityEngine.UI;
public class Mallory : MonoBehaviour
{

    [Header("Movement")]
    public float speed;
	public float throwPower;
    private float windPower;
    private Rigidbody rb;
    private float orbIncrementSpeed = 1.2f;
	private float treeSpeed;
	private float treeSlow = 0.7f;
	private float originalSpeed;
	private int triggerCount = 0;

    private GameObject[] pickups;

    private HandleObjects handleObjects;

    [Header("UI")]
    public Image windPowerBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		throwPower = 100;
        windPower = 0;
		originalSpeed = speed;
		treeSpeed = treeSlow * speed;
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
    }

    void FixedUpdate()
    {
        //Go Up
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("AButton")) {
            transform.Translate(Vector3.up * Time.deltaTime * speed/2, Space.World);
        }
        //Go Down
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetButton("BButton")) && transform.position.y > 3) {
            transform.Translate(Vector3.down * Time.deltaTime * speed/2, Space.World);
        }

        //Top Speed Reset
        if (rb.velocity.magnitude > 15) {
            //rb.velocity.magnitude = 15;
        }


        //XBOX MOVEMENT


        //Moving Forward and Backwards
        if (Input.GetButton("Sprint"))
        {
            //Debug.Log("Running");
            rb.AddForce(Camera.main.transform.forward * Input.GetAxis("Vertical") * (speed * 2));
            rb.AddForce(Camera.main.transform.up * Input.GetAxis("Vertical") * (speed * 2));
            rb.AddForce(Camera.main.transform.right * Input.GetAxis("Horizontal") * (speed * 2));
        }
        else
        {
            //Debug.Log("Walking");
            rb.AddForce(Camera.main.transform.forward * Input.GetAxis("Vertical") * speed);
            rb.AddForce(Camera.main.transform.up * Input.GetAxis("Vertical") * speed);
            rb.AddForce(Camera.main.transform.right * Input.GetAxis("Horizontal") * speed);
        }
        rb.AddForce(-rb.velocity);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Orb"))
        {
            other.gameObject.SetActive(false);
            windPower += 100;
            windPowerBar.fillAmount = windPower / 500; 
            speed *= orbIncrementSpeed;
			originalSpeed = speed;
			treeSpeed = treeSlow * speed;
<<<<<<< HEAD
            //other.gameObject.GetComponent<HandleObjects>().throwForce *= 2;
			//HandleObjects.throwForce *= 2;
			//FindObjectOfType<HandleObjects> ().throwForce *= 2;
			throwPower *= 2;
		} else if (other.gameObject.CompareTag ("Tree")) {
=======
            for(int i = 0; i < pickups.Length; i++)
            {
                pickups[i].GetComponent<HandleObjects>().throwForce *= 2;
            }
		}
        else if(other.gameObject.CompareTag("Orb2"))
        {
            other.gameObject.SetActive(false);
            windPower += 50;
            windPowerBar.fillAmount = windPower / 500;
            speed *= orbIncrementSpeed;
            originalSpeed = speed;
            treeSpeed = treeSlow * speed;
            for (int i = 0; i < pickups.Length; i++)
            {
                pickups[i].GetComponent<HandleObjects>().throwForce *= 1.5f;
            }
        }
        else if (other.gameObject.CompareTag("Orb3"))
        {
            other.gameObject.SetActive(false);
            windPower += 25;
            windPowerBar.fillAmount = windPower / 500;
            speed *= orbIncrementSpeed;
            originalSpeed = speed;
            treeSpeed = treeSlow * speed;
            for (int i = 0; i < pickups.Length; i++)
            {
                pickups[i].GetComponent<HandleObjects>().throwForce *= 1.25f;
            }
        }
        else if (other.gameObject.CompareTag ("Tree")) {
>>>>>>> 2c2cb6a1d57109bd4f0cb3b703a1859afad18710
			speed = treeSpeed;
			triggerCount++;
			Debug.Log ("Speed is reduced to :" + speed);
		} else if (other.gameObject.CompareTag ("PickUp")) {
			other.gameObject.GetComponent<HandleObjects> ().throwForce = throwPower;
		}
    }

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Tree")) {
			triggerCount--;
			if (triggerCount == 0) {
				speed = originalSpeed;
				Debug.Log ("Speed is back to :" + speed);
			}
		}
	}

	//void OnCollisionEnter(Collider other)
	//{
	//	if (other.tag == "PassThrough") {
	//		Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
	//	}
	//}
}