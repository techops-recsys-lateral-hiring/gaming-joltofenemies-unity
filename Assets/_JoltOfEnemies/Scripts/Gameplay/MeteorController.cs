using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MeteorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer = null;
    [SerializeField] private Collider2D _collider = null;
    [SerializeField] private Sprite[] _sprites = null;
    [Range(10f, 20f)]
    [SerializeField] private float _speedAngularMin = 15f;
    [Range(20f, 45f)]
    [SerializeField] private float _speedAngularMax = 20f;

    private float _speedAngular = 0f;
    
    // Start is called before the first frame update
    private void Start()
    {
        _speedAngular = Random.Range(_speedAngularMin, _speedAngularMax);
        if ((int)_speedAngular % 2 == 0)
        {
            _speedAngular *= -1;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0f, 0f, _speedAngular * Time.deltaTime);
    }
}
