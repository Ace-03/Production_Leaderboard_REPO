using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    public Material material;
    public GameObject pointLight;

    void Awake()
    {
        material.DisableKeyword("_EMISSION");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            material.EnableKeyword("_EMISSION");
            pointLight.SetActive(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            material.DisableKeyword("_EMISSION");
            pointLight.SetActive(false);
        }
    }
}
