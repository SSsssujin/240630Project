using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TextActivater : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        TryGetComponent(out _meshRenderer);
        _meshRenderer.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _meshRenderer.enabled = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
