using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Life,
    
}
public class AwardItem : MonoBehaviour
{
    public float speed = 8f;
    public ItemType type;
    private Rigidbody rigidbody;// Start is called before the first frame update
    private bool startmove = false;
    private Transform player;
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (startmove)
        {
            transform.position = Vector3.Lerp(transform.position, player.position,speed*Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position)<0.7f)
                {
                player.GetComponent<PlayerAward>().GetAward(type);
                Destroy(this.gameObject);
            }
        }
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag=="Ground")
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            SphereCollider col = this.GetComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 2;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            startmove = true;
            player = other.transform;

        }
        
    }
}
