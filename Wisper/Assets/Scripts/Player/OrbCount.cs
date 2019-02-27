using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour { 

    private int orbMax = 0;
	private int orbCount;
    private PersistantStateData psd;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    // public AudioSource wub;

    //Initilize Variables
    void Start()
    {
        Debug.Log("bitch ");
        psd = PersistantStateData.persistantStateData;

        orbMax = (int)psd.stateConditions["TotalTasks"] * 5;
        psd.ChangeStateConditions("TotalOrbCount", orbMax);

        orbCount = (int)psd.stateConditions["OrbCount"];
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
    }

    //Sets the orb count
	public void SetOrbCount(int newOrbCount) {
		orbCount = newOrbCount;
        windPowerBar.fillAmount = orbCount / orbMax;
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
	}

   // //Triggers on collision
   // private void OnTriggerEnter(Collider other)
   // {
   //     //Calculates the distance orb is from player
   //     //float objectDistance = (transform.position - other.transform.position).magnitude;

   //     //Gathers orb if orb is within range
   //     if (other.gameObject.CompareTag("Orb") /*&& objectDistance < orbPickupRadius*/)
   //     {
   //         //Destroys Orb
   //         Destroy(other.gameObject);
			////Debug.Log ("Added 1 orb");

   //         ///Update UI
   //         if (orbCount < orbMax)
   //         {
   //             orbCount ++;
   //             //Debug.Log("count increased");
   //         }
   //         // wub.Play();
   //         windPowerBar.fillAmount = orbCount / orbMax;
   //         orbCountText.text = orbCount.ToString() + "/" + orbMax;
   //         ///End Update UI
   //     }
   // }


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
            if ((int)psd.stateConditions["OrbCount"] < (int)psd.stateConditions["TotalOrbCount"])
            {
                orbCount++;
                psd.ChangeStateConditions("OrbCount", orbCount);
                //Debug.Log("count increased");
            }
            // wub.Play();
            windPowerBar.fillAmount = ((float)psd.stateConditions["OrbCount"] / (float)psd.stateConditions["TotalOrbCount"]);
            orbCountText.text = (int)psd.stateConditions["OrbCount"] + "/" + (int)psd.stateConditions["TotalOrbCount"];
            ///End Update UI
        }
    }

    //Gets the current orb count
    public float GetOrbCount(){
		return orbCount;
	}
    public float GetMaxOrbCount()
    {
        return orbMax;
    }
}