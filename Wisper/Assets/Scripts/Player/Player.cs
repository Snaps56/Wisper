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
	private float startingSpeed;
    private float originalVAcceleration;
	private int treeCount = 0;
	private float vel;
    private GameObject[] pickups;
    private HandleObjects handleObjects;
    private Vector3 positionStamp;
    private float shake;
    private DialogueManager dialogueManager;

    private float verticalAcceleration = 0.001f;
    private float verticalSpeed = 0;

    [Header("Collision Handeling")]
    public Collider playerCollider;
    public float shakeAmount;

    [Header("UI")]
    public Image windPowerBar;
    public GameObject turnBackText;
    public GameObject miniMap;
    public Text orbCountText;
    

    void Start()
    {
        miniMap.SetActive(true);
        rb = GetComponent<Rigidbody>();
		//rb.drag = 1;
		//rb.angularDrag = 1;
		throwPower = 100;
        orbCount = 0;
		startingSpeed = speed;
		originalSpeed = speed;
        originalVAcceleration = verticalAcceleration;
		treeSpeed = treeSlow * speed;
        pickups = GameObject.FindGameObjectsWithTag("PickUp");
        turnBackText.SetActive(false);
        orbCountText.text = orbCount.ToString()+"/500";
        shakeAmount = 0.05f;
        shake = 0;
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
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
            if (verticalSpeed > 0)
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
            if (verticalSpeed < 0)
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
        float objectDistance = (transform.position - other.transform.position).magnitude;
        if (other.gameObject.CompareTag("Orb") && objectDistance < 2f) {
            Destroy(other.gameObject);
            if (orbCount < 500)
            {
                orbCount += 1;
            }
            windPowerBar.fillAmount = orbCount / 500; 
            speed += orbIncrementSpeed;
            //Disabled increment verticalAcceleration because it caused the player to sink
            //verticalAcceleration += 0.0001f;
			originalSpeed = speed;
            originalVAcceleration = verticalAcceleration;
			treeSpeed = treeSlow * speed;
			throwPower += 2;
            orbCountText.text = orbCount.ToString() + "/500";
        }
        if (other.gameObject.CompareTag("Border"))
        {
            //shake = 1;
            positionStamp = this.transform.position;
            //if (speed > preTreeSpeed/2 )
            //{
            //    speed = speed * 0.1f;
            //    verticalAcceleration = 0.001f;
            //}

            if (speed > originalSpeed/2 )
            {
                speed = speed * 0.1f;
                verticalAcceleration = 0.001f;
            }
            //if (shake > 0)
            //{
            //    this.transform.position = this.transform.position + Random.insideUnitSphere * shakeAmount;
            //}
            //else
            //{
            //    shake -= Time.deltaTime * 0.1f;
            //}
            turnBackText.SetActive(true);
        }
        if (other.gameObject.CompareTag("BorderTele"))
        {
            rb.velocity = new Vector3(0, 0, 0);
            this.transform.position = positionStamp;
        }
        if (other.gameObject.CompareTag ("Tree")) {
			speed = treeSpeed;
			treeCount++;
			Debug.Log ("Speed is reduced to :" + speed);
		}
  //      if (other.gameObject.CompareTag ("PickUp")) {
		//	other.gameObject.GetComponent<HandleObjects>().throwForce = throwPower;
		//}
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = true;
		}
        if (other.gameObject.CompareTag("NPC"))
        {
            NPCDialogues npcDialogues = other.gameObject.GetComponent<NPCDialogues>();
            if (npcDialogues != null)   // If this npc has dialogues
            {
                if (!npcDialogues.getInDialogueRange())
                {
                    npcDialogues.setInDialogueRange(true);  // Flags dialogues attached to npc as in range. Used as a lock to prevent unnecessary updates to dialogue manager.
                    dialogueManager.addInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        shake = 1;
        if (other.gameObject.CompareTag("Border"))
        {
            if (shake > 0)
            {
                this.transform.position = this.transform.position + Random.insideUnitSphere * shakeAmount;
            }
            else
            {
                shake -= Time.deltaTime * 0.1f;
            }
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
            turnBackText.SetActive(false);
            speed = originalSpeed;
            verticalAcceleration = originalVAcceleration;
            shake = 0;
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

	public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
		windPowerBar.fillAmount = orbCount / 500;
		//Assuming that the passed in newOrbCount is 0, which it should be
		speed = startingSpeed;
		originalSpeed = startingSpeed;
		treeSpeed = treeSlow * startingSpeed;
		Debug.Log ("OrbCount = " + orbCount);
		Debug.Log ("Speed is back to :" + speed);
	}


	//void OnCollisionEnter(Collider other)
	//{
	//	if (other.tag == "PassThrough") {
	//		Physics.IgnoreCollision(other.GetComponent<Collider>(), this.GetComponent<Collider>());
	//	}
	//}
}