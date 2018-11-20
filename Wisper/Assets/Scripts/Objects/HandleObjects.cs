using UnityEngine;
using System.Collections;

public class HandleObjects : MonoBehaviour
{
    [Header("Linked Objects")]
    public Transform player;
    public Transform playerCam;

    [Header("Handling")]
    public float throwForce = 10000;
    bool hasPlayer = false;
    bool beingCarried = false;
    private bool touched = false;
    public float carryCooldownTime = 1;
    float currentCooldownTime = 0;
    bool startCooldown = false;
    float liftHeight = 0.2f;
    bool rightMouseDown = false;

    void Start()
    {

    }

    void Update()
    {

        // check if Mouse Right Click is held down, used for lifting object
        if (!rightMouseDown)
        {
            if (Input.GetMouseButtonDown(1))
            {
                rightMouseDown = true;
            }
        }
        if (rightMouseDown && Input.GetMouseButtonUp(1))
        {
            rightMouseDown = false;
        }

        //Ditects the distance from the object to the player
        float dist = Vector3.Distance(gameObject.transform.position, player.position);
        if (dist <= 2f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        // pick up any objects within range and that haven't been recently picked up;
        // Hold Right Trigger
        if (hasPlayer && !startCooldown && (Input.GetAxis("TriggerL") > 0 || rightMouseDown))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            beingCarried = true;
            transform.position += new Vector3(0, liftHeight, 0);
        }
        // objects are currently being carried
        if (beingCarried)
        {
            //Rotates object while its being held
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * 3);
            if (touched)
            {
                dropObject();
                touched = false;
            }
            // Pressing Right Trigger while holding will throw objects
            if (Input.GetAxis("TriggerR") > 0 || Input.GetMouseButton(0))
            {
                throwObject();
            }
            // drop item by letting go of left trigger
            if (Input.GetAxis("TriggerL") == 0 && !rightMouseDown)
            {
                dropObject();
            }
        }
        // prevent objects from being immediately picked up after throwing
        if (startCooldown)
        {
            currentCooldownTime -= Time.deltaTime;
            if (currentCooldownTime <= 0)
            {
                startCooldown = false;
                currentCooldownTime = carryCooldownTime;
            }
        }

        // Pressing Right Trigger to push objects
        if ((Input.GetAxis("TriggerR") > 0 || Input.GetMouseButton(0)) && hasPlayer && !startCooldown)
        {
            transform.position += new Vector3(0, liftHeight * 0.5f, 0);
            throwObject();
        }

		IgnoreCollision ();
    }

    //Throws the object
    void throwObject()
    {
        // drop object first before throwing
        dropObject();
        startCooldown = true;

        // creates force vector so that player throws carried objects straight ahead rather than at ground
        Vector3 forceVector = playerCam.forward * throwForce;
        forceVector.y *= 0;
        GetComponent<Rigidbody>().AddForce(forceVector);

        // adds cooldown to objects to prevent them from being immediately picked up after throwing
        startCooldown = true;
    }

    //Drop the object
    void dropObject()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;
        beingCarried = false;
    }

    //Activate on collision with character
    void OnTriggerEnter()
    {
        if (beingCarried)
        {
            touched = true;
        }
    }

    //Ignores collision with player
	void IgnoreCollision() {
		Physics.IgnoreCollision (player.GetComponent<Collider>(), this.GetComponent<Collider>());
	}
}