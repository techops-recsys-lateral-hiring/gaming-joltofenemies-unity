using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D _collider = null;
    [SerializeField] private Transform _mountPointShot = null;
    [SerializeField] private SpriteRenderer _renderer = null;
    [Space(20)]
    [Header("Settings")]
    [SerializeField] private float _speedRotation = 90f;

    private float inputH = 0f;
    private float inputV = 0f;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInput();
        
    }

    private void ProcessInput()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
    }
}
