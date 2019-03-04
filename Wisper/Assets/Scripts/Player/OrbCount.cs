using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour { 

    private float TotalOrbCount = 0;
	private float orbCount = 0;
    private float orbMaxDeposit = 0;
    private PersistantStateData psd;

    [Header("Trail")]
    public GameObject trail;
    public Material trailRed;
    public Material trailWhite;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    // public AudioSource wub;

    //Initilize Variables
    void Start()
    {
        psd = PersistantStateData.persistantStateData;

        //TotalOrbCount = (float)psd.stateConditions["TotalTasks"] * 5;
        //psd.ChangeStateConditions("TotalOrbCount", TotalOrbCount);

        //orbMaxDeposit = TotalOrbCount / 2;
        //psd.ChangeStateConditions("OrbMaxDeposit", orbMaxDeposit);

        orbMaxDeposit = (float)psd.stateConditions["OrbMaxDeposit"];

        orbCount = (float)psd.stateConditions["OrbCount"];
        orbCountText.text = (float)psd.stateConditions["OrbCount"] + "/" + orbMaxDeposit;
        windPowerBar.fillAmount = ((float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["OrbMaxDeposit"]);

    }

    //Sets the orb count
    public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
        windPowerBar.fillAmount = orbCount / orbMaxDeposit;
        orbCountText.text = orbCount.ToString() + "/" + orbMaxDeposit;
	}

    private void Update()
    {
        if ((float)psd.stateConditions["OrbCount"] >=  (float)psd.stateConditions["OrbMaxDeposit"])
        {
            trail.GetComponent<TrailRenderer>().material = trailRed;
        }
        else
        {
            trail.GetComponent<TrailRenderer>().material = trailWhite;
        }
    }


    //Triggers on collision
    private void OnTriggerEnter(Collider other)
    {
        //Calculates the distance orb is from player
        //float objectDistance = (transform.position - other.transform.position).magnitude;

        //Gathers orb if orb is within range
        if (other.gameObject.CompareTag("Orb") /*&& objectDistance < orbPickupRadius*/)
        {
            //Destroys Orb
            Destroy(other.gameObject);
            //Debug.Log ("Added 1 orb");

            ///Update UI
            if ((float)psd.stateConditions["OrbCount"] < (float)psd.stateConditions["OrbMaxDeposit"])
            {
                orbCount++;
                psd.ChangeStateConditions("OrbCount", orbCount);
                Debug.Log("count increased");
            }
            // wub.Play();
            windPowerBar.fillAmount = ((float)psd.stateConditions["OrbCount"] / Mathf.Ceil((float)psd.stateConditions["OrbMaxDeposit"]));
            orbCountText.text = (float)psd.stateConditions["OrbCount"] + "/" + Mathf.Ceil((float)psd.stateConditions["OrbMaxDeposit"]);
            ///End Update UI
        }
    }

    //Gets the current orb count
    public float GetOrbCount(){
		return orbCount;
	}
    public float GetMaxOrbCount()
    {
        return orbMaxDeposit;
    }
}