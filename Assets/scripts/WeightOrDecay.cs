using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightOrDecay : MonoBehaviour
{
    public GameObject Weight;
    public GameObject Decay;
    public HingeJoint2D HingeJoint;
    public DistanceJoint2D DistantJoint;
    public float ChanceForDecay = 0.50f; // 50% chance for Selected to be 1

    public bool IsWeight = false; // Set to true for always 1, false for always 2
    public bool IsDecay = false; // Set to true for always 2, false for always 1

    private GameObject activeObject;
    

    // Start is called before the first frame update
    void Start()
    {
        if (IsDecay)
        {
            Weight.SetActive(false);
            Decay.SetActive(true);
            HingeJoint.connectedBody = Decay.GetComponent<Rigidbody2D>();
            DistantJoint.connectedBody = Decay.GetComponent<Rigidbody2D>();
        }
        else if (IsWeight)
        {
            Weight.SetActive(true);
            Decay.SetActive(false);
            HingeJoint.connectedBody = Weight.GetComponent<Rigidbody2D>();
            DistantJoint.connectedBody = Weight.GetComponent<Rigidbody2D>();
        }
        else
        {
            float randomValue = Random.value;

            if (randomValue <= ChanceForDecay)
            {
                Weight.SetActive(false);
                Decay.SetActive(true);
                HingeJoint.connectedBody = Decay.GetComponent<Rigidbody2D>();
                DistantJoint.connectedBody = Decay.GetComponent<Rigidbody2D>();
            }
            else
            {
                Weight.SetActive(true);
                Decay.SetActive(false);
                HingeJoint.connectedBody = Weight.GetComponent<Rigidbody2D>();
                DistantJoint.connectedBody = Weight.GetComponent<Rigidbody2D>();
            }
        }
    }
}
