using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class OrbCount : MonoBehaviour { 

    private float orbMax = 50;
	private float orbCount;
    private float orbIncrementSpeed = 0.1f;
	private float originalSpeed;
	private float startingSpeed;

    [Header("UI")]
    public Image windPowerBar;
    public Text orbCountText;

    void Start()
    {
        orbCount = 0;
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
    }
    void Update()
    {
            
    }
    
	public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
        windPowerBar.fillAmount = orbCount / orbMax;
        orbCountText.text = orbCount.ToString() + "/" + orbMax;
        Debug.Log ("OrbCount = " + orbCount);
	}

    private void OnTriggerEnter(Collider other)
    {
        float objectDistance = (transform.position - other.transform.position).magnitude;
        if (other.gameObject.CompareTag("Orb") && objectDistance < 2f)
        {
            Destroy(other.gameObject);
            if (orbCount < orbMax)
            {
                orbCount += 1;
            }
            windPowerBar.fillAmount = orbCount / orbMax;
            orbCountText.text = orbCount.ToString() + "/" + orbMax;
        }
    }

    public float GetOrbCount(){
		return orbCount;
	}

}