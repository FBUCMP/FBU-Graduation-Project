using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saliva : MonoBehaviour
{
    public GameObject residue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {           
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.tag != "Player")
        {
            var contact = collision.GetContact(0);
            float angle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
            angle -= 90;
            GameObject newResidue = Instantiate(residue, contact.point, Quaternion.Euler(0, 0, angle));
            Destroy(gameObject);
        }
        
    }
}
