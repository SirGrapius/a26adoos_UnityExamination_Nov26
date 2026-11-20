using System;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            LimitedPlatformerMovement player =  other.gameObject.GetComponent<LimitedPlatformerMovement>();
            if (player.invincible == false)
            {
                StartCoroutine(player.TakeDamage(damage));
            }
        }
    }
}
