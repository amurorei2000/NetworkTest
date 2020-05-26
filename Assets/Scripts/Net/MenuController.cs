using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MenuController : MonoBehaviourPun
{

    [PunRPC]
    void EnablePartyPanel()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        GameManager.gm.partyPanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameManager.gm.partyPanel.SetActive(false);
    }
}
