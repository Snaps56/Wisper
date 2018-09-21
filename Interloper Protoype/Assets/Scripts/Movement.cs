using UnityEngine;
public class Movement : MonoBehaviour
{

    public float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");


        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //Debug.Log("The speed of wind is " + rb.velocity.magnitude);

        ////Moving Forward
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(Camera.main.transform.forward * (speed*2));
        //    //rb.AddForce( movement * (speed * 2));

        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(Camera.main.transform.forward * speed);
        //    //rb.AddForce(movement * speed);
        //}


        ////Moving Back
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)) {
        //    rb.AddForce(-Camera.main.transform.forward * (speed * 2));
        //    //rb.AddForce( movement * (speed * 2));

        //}
        //else if (Input.GetKey(KeyCode.S)) {
        //    rb.AddForce(-Camera.main.transform.forward * speed);
        //    //rb.AddForce(movement * speed);
        //}


        ////Moving Right
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)) {
        //    rb.AddForce(Camera.main.transform.right * (speed * 2));
        //    //rb.AddForce( movement * (speed * 2));

        //}
        //else if (Input.GetKey(KeyCode.D)) {
        //    rb.AddForce(Camera.main.transform.right * speed);
        //    //rb.AddForce(movement * speed);
        //}

        ////Moving Left
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)) {
        //    rb.AddForce(-Camera.main.transform.right * (speed * 2));
        //    //rb.AddForce( movement * (speed * 2));

        //}
        //else if (Input.GetKey(KeyCode.A)) {
        //    rb.AddForce(-Camera.main.transform.right * speed);
        //    //rb.AddForce(movement * speed);
        //}


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
            rb.AddForce(Camera.main.transform.right * Input.GetAxis("Horizontal") * (speed * 2));
        }
        else
        {
            //Debug.Log("Walking");
            rb.AddForce(Camera.main.transform.forward * Input.GetAxis("Vertical") * speed);
            rb.AddForce(Camera.main.transform.right * Input.GetAxis("Horizontal") * speed);
        }

    }
}