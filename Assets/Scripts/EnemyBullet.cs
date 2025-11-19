using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int Enemybulletdamage;
    [SerializeField]
    private GameObject bulletDecal;
    private void Start()
    {
        
        Destroy(gameObject, 10f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemybulletdamage = Random.Range(15, 25);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(Enemybulletdamage);
            DestroyGO();
        }
        else
        {
            ContactPoint contact = collision.GetContact(0);
            //spawn decal facing player and stops clash with wall so decal fixed
            GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            DestroyGO();
        }
    }
    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
