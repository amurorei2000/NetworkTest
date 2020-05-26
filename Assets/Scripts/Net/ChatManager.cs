using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public ScrollRect sRect;
    public Text chatText;
    public InputField myField;

    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        sRect.verticalNormalizedPosition = 0.0f;
    }

    public void InputText()
    {
        if (myField.text.Length > 0)
        {
            pv.RPC("TextProcess", RpcTarget.All, myField.text);
            myField.text = "";
        }
    }

    [PunRPC]
    void TextProcess(string dialogue, PhotonMessageInfo info)
    {
        chatText.text += "\r\n" + info.Sender.NickName + ": " + dialogue;
    }
}
