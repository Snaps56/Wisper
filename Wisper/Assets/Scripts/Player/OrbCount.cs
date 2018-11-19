using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour { 

    private float orbMax = 50;
	private float orbCount;
    private float orbIncrementSpeed = 0.1f;
    private float orbPickupRadius = 2.0f;
	private float originalSpeed;
	private float startingSpeed;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    //Initilize Variables
    void Start()
    {
        orbCount = 0;
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
    }

    //Sets the orb count
	public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
        windPowerBar.fillAmount = orbCount / orbMax;
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
        Debug.Log ("OrbCount = " + orbCount);
	}

    //Triggers on collision
    private void OnTriggerStay(Collider other)
    {
        //Calculates the distance orb is from player
        float objectDistance = (transform.position - other.transform.position).magnitude;

        //Gathers orb if orb is within range
        if (other.gameObject.CompareTag("Orb") && objectDistance < orbPickupRadius)
        {
            //Destroys Orb
            Destroy(other.gameObject);

            ///Update UI
            if (orbCount < orbMax)
            {
                orbCount += 1;
            }
            windPowerBar.fillAmount = orbCount / orbMax;
            orbCountText.text = orbCount.ToString() + "/" + orbMax;
            ///End Update UI
        }
    }

    //Gets the current orb count
    public float GetOrbCount(){
		return orbCount;
	}

}