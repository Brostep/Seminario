using UnityEngine;

public class AuxiliaryPassageBehaviour : MonoBehaviour
{
    public GameObject passage;
    public int triggerPoints;

    private void Start()
    {
        if (passage.GetComponent<Rigidbody>().useGravity)
            passage.GetComponent<Rigidbody>().useGravity = false;
    }

    private void Update()
    {
        if (triggerPoints <= 0)
        {
            GetComponent<MeshRenderer>().gameObject.SetActive(false);
            GetComponent<MeshCollider>().gameObject.SetActive(false);
            passage.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.layer == 9)
            triggerPoints--;
    }
}
