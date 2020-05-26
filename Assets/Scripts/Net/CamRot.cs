using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamRot : MonoBehaviour
{
    public float rotSpeed = 300.0f;

    float mx = 0;
    float my = 0;

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            mx += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            my += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
    }
}
