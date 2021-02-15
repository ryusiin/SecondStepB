using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Component_RotateCanvas : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
