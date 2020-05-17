using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviourPun, IPunObservable
{
    public GameObject bulletFactory;
    public Transform firePos;
    public bool useRay = false;
    //public GameObject bulletEffect;

    ParticleSystem ps;
    AudioSource shootAudio;

    void Start()
    {
        //ps = bulletEffect.GetComponent<ParticleSystem>();
        shootAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!useRay)
                {
                    GameObject bullet = PhotonNetwork.Instantiate("PlayerBullet", firePos.position, Quaternion.identity);
                    bullet.transform.forward = firePos.forward;
                    shootAudio.Play();
                }
                else
                {
                    shootAudio.Play();
                    //ShootRay(ps, shootAudio);
                }
            }
        }
    }

    void ShootRay(ParticleSystem eff, AudioSource aud)
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                print("Fire Ray !!!");
            }
            else
            {
                //bulletEffect.transform.position = hitInfo.point;
                //bulletEffect.transform.up = hitInfo.normal;
                //eff.Play();
                //aud.Play();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
