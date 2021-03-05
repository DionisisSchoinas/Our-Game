using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float speed = 8f;

  
    // Start is called before the first frame update
    void Start()
    {
     
        rb = FindObjectOfType<Rigidbody>();
    }
    public void FireSimple(Transform firePoint)
    {
        
        GameObject tmp = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }


}