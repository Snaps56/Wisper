using UnityEngine;
using UnityEngine.UI;
public class Mallory : MonoBehaviour
{

    [Header("Movement")]
    public float speed;
    private float windPower;
    private Rigidbody rb;
    private float orbIncrementSpeed = 1.2f;
	private float treeSpeed;
	private float treeSlow = 0.7f;
	private bool isSlowedByTree = false;
	private float originalSpeed;
	private int triggerCount;

    [Header("UI")]
    public Image windPowerBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        windPower = 0;
		originalSpeed = speed;
		treeSpeed = treeSlow * originalSpeed;
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
			treeSpeed = treeSlow * originalSpeed;
            //other.gameObject.GetComponent<HandleObjects>().throwForce *= 2;
			HandleObjects.instance.throwForce *= 2;
		} else if (other.gameObject.CompareTag ("Tree")) {
			speed = treeSpeed;
			triggerCount++;
			Debug.Log ("Trigger Count: " + triggerCount);
			Debug.Log ("Speed is reduced to :" + speed);
		}
    }

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Tree")) {
			triggerCount--;
			Debug.Log ("Trigger Count: " + triggerCount);
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