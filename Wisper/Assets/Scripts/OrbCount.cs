using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour
{

    private float TotalOrbCount = 0;
    private float orbCount = 0;
    private float totalOrbs = 0;

    private PersistantStateData psd;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    Color32 blue, red, white;

    //Orb Explode Prefab
    public GameObject orbExplode;

    private AudioSource orbAudioSource;
    public AudioClip orbAudioClip;

    //Initilize Variables
    void Start()
    {
        psd = PersistantStateData.persistantStateData;
        orbAudioSource = GetComponent<AudioSource>();
        totalOrbs = (float)psd.stateConditions["TotalOrbCount"];
        orbCount = (float)psd.stateConditions["OrbCount"];
        orbCountText.text = psd.stateConditions["OrbCount"].ToString()/* + "/" + totalOrbs*/;
        windPowerBar.fillAmount = ((float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["TotalOrbCount"]);
        blue = new Color32(0, 46, 255, 255);
        white = new Color32(255, 255, 255, 255);
        red = new Color32(255, 0, 0, 255);
    }

    //Sets the orb count
    public void SetOrbCount(float newOrbCount)
    {
        Debug.Log("PSD: " + psd + ", newOrbCount: " + newOrbCount);
        orbCount = newOrbCount;
        PersistantStateData.persistantStateData.ChangeStateConditions("OrbCount", newOrbCount);
        windPowerBar.fillAmount = orbCount / totalOrbs;
        orbCountText.text = orbCount.ToString(); /*+ "/" + totalOrbs;*/
    }

    //Increase the orb count
    public void IncreaseOrbCount()
    {
        if (orbCount < (float)psd.stateConditions["TotalOrbCount"])
        {
            orbCount += 5;
            if (orbCount > (float)psd.stateConditions["TotalOrbCount"])
            {
                orbCount = (float)psd.stateConditions["TotalOrbCount"];
            }
        }

        psd.ChangeStateConditions("OrbCount", orbCount);
        windPowerBar.fillAmount = orbCount / totalOrbs;
        orbCountText.text = orbCount.ToString() /*+ "/" + totalOrbs.ToString()*/;
    }

    //Decrease the orb count
    public void DecreaseOrbCount()
    {
        if (orbCount > 0)
        {
            orbCount -= 5;
            psd.ChangeStateConditions("OrbCount", orbCount);
        }
        windPowerBar.fillAmount = orbCount / totalOrbs;
        orbCountText.text = orbCount.ToString() /*+ "/" + totalOrbs.ToString()*/;
    }

    private void Update()
    {
        //If player hits max orbs
        if ((float)psd.stateConditions["OrbCount"] >= (float)psd.stateConditions["TotalOrbCount"])
        {
            windPowerBar.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            psd.ChangeStateConditions("HasReachedMax", true);

        }
        //If play is normal
        else
        {
            float t = (float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["TotalOrbCount"];
            Color32 color = Color32.Lerp(white, red, t);
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
            Debug.Log("Beginning orb collection process");
            //Destroys Orb
            Destroy(other.gameObject);
            Instantiate(orbExplode, transform.position, Quaternion.identity);
            orbAudioSource.PlayOneShot(orbAudioClip);
            

            Debug.Log ("Added 1 orb");

            ///Update UI

            if ((float)psd.stateConditions["OrbCount"] < (float)psd.stateConditions["TotalOrbCount"])
            {
                orbCount++;
                psd.ChangeStateConditions("OrbCount", orbCount);
                psd.ChangeStateConditions("HasReachedMax", false);
                //Debug.Log("count increased");
            }
            else if ((float)psd.stateConditions["OrbCount"] == (float)psd.stateConditions["TotalOrbCount"])
            {
                psd.ChangeStateConditions("HasReachedMax", true);
            }

            windPowerBar.fillAmount = ((float)psd.stateConditions["OrbCount"] / Mathf.Ceil((float)psd.stateConditions["TotalOrbCount"]));
            orbCountText.text = psd.stateConditions["OrbCount"].ToString()/* + "/" + Mathf.Ceil((float)psd.stateConditions["TotalOrbCount"])*/;
            ///End Update UI
            psd.ChangeStateConditions("HasNoOrbs", false);
        }
    }

    //Gets the current orb count
    public float GetOrbCount()
    {
        return (float)PersistantStateData.persistantStateData.stateConditions["OrbCount"];
        //return orbCount;
    }
    public float GetMaxOrbCount()
    {
        return totalOrbs;
    }
}