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
    public Image sunshoreLogo;

    Color32 blue, red, white, lightred;
    //public float timeToReachTarget;
    //private Vector3 logoStartPosition;
    public float vibrationMultiplier;
    public float logoLeftShift;
    public float logoRightShift;

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
        blue = new Color32(0, 46, 255, 255);
        white = new Color32(255, 255, 255, 255);
        red = new Color32(255, 0, 0, 255);
        lightred = new Color32(255, 180, 180, 255);
        //logoStartPosition = sunshoreLogo.transform.position;
        logoLeftShift = sunshoreLogo.transform.position.x - logoLeftShift;
        logoRightShift = sunshoreLogo.transform.position.x + logoRightShift;


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
            if(orbCount > (float)psd.stateConditions["OrbMaxDeposit"])
            {
                orbCount = (float)psd.stateConditions["OrbMaxDeposit"];
            }
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
            psd.ChangeStateConditions("HasReachedMax", true);

        }
        //If play is normal
        else
        {
            float t = (float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["OrbMaxDeposit"];
            psd.ChangeStateConditions("HasReachedMax", true);
            Color32 color = Color32.Lerp(white, lightred, t);
            windPowerBar.GetComponent<Image>().color = color;
            try
            {
                sunshoreLogo.GetComponent<Image>().color = color;
            }
            catch (System.Exception e)
            {

            }


        }
        try
        {
            if ((float)psd.stateConditions["OrbCount"] != 0)
            {
                RectTransform logoTransform = sunshoreLogo.GetComponent<RectTransform>();
                logoTransform.position = new Vector3(Mathf.PingPong(Time.time * vibrationMultiplier * (float)psd.stateConditions["OrbCount"], logoRightShift - logoLeftShift) + logoLeftShift, logoTransform.position.y, logoTransform.position.z);
            }
        }
        catch (System.Exception e)
        {

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
            ///
            psd.ChangeStateConditions("HasNoOrbs", false);
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