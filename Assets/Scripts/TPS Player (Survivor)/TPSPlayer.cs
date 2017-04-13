using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TPSPlayer : Photon.MonoBehaviour
{


    public int currentHealth, ammo, stamina;
    public static bool setDamageUI = false;
    [SerializeField]
    string bulletName = "Bullet"; 


    void Shoot()
    {
        PhotonNetwork.Instantiate(bulletName, this.transform.position, this.transform.rotation, 0);
    }

    public void TakeDamage()
    {
        currentHealth = currentHealth - 10;
        setDamageUI = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Alien" || col.gameObject.tag == "Queen")
        {
            TakeDamage();
        }
    }
}
