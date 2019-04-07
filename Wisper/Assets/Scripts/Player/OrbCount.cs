using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour { 

    private float TotalOrbCount = 0;
	private float orbCount = 0;
    private float orbMaxDeposit = 0;
    private PersistantStateData psd;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    Color32 color1, color2;

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
        color1 = new Color32(0, 46, 255, 255);
        color2 = new Color32(255, 0, 0, 255);
    }

    //Sets the orb count
    public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
        psd.ChangeStateConditions("OrbCount", newOrbCount);
        windPowerBar.fillAmount = orbCount / orbMaxDeposit;
        orbCountText.text = orbCount.ToString() + "/" + orbMaxDeposit;
	}

    //Increase the orb count
    public void IncreaseOrbCount()
    {
        if (orbCount < (float)psd.stateConditions["OrbMaxDeposit"])
        {
            orbCount += 5;
        }
        psd.ChangeStateConditions("OrbCount", orbCount);
        windPowerBar.fillAmount = orbCount / orbMaxDeposit;
        orbCountText.text = orbCount.ToString() + "/" + orbMaxDeposit;
    }

    //Decrease the orb count
    public void DecreaseOrbCount()
    {
        if (orbCount > 0)
        {
            orbCount -= 5;
            psd.ChangeStateConditions("OrbCount", orbCount);
        }
        windPowerBar.fillAmount = orbCount / orbMaxDeposit;
        orbCountText.text = orbCount.ToString() + "/" + orbMaxDeposit;
    }

    private void Update()
    {
        //If player hits max orbs
        if ((float)psd.stateConditions["OrbCount"] >=  (float)psd.stateConditions["OrbMaxDeposit"])
        {
            windPowerBar.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        //If play is normal
        else
        {
            float t = (float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["OrbMaxDeposit"];
            Color32 color = Color32.Lerp(color1, color2, t);
            windPowerBar.GetComponent<Image>().color = color;
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
                psd.ChangeStateConditions("HasReachedMax", false);
                Debug.Log("count increased");
            }
            else if ((float)psd.stateConditions["OrbCount"] == (float)psd.stateConditions["OrbMaxDeposit"])
            {
                psd.ChangeStateConditions("HasReachedMax", true);
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