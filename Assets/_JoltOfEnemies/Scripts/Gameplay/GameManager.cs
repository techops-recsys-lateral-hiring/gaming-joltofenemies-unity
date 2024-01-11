using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _camera = null;
    [SerializeField] private Transform _parentMetors = null;
    [SerializeField] private Transform _parentEnemies = null;
    [SerializeField] private PlayerController _player = null;
    [SerializeField] private ViewHud _hud = null;

    [Header("Params")]
    [SerializeField] private int _initialMeteors = 10;
    [Range(0.1f, 10f)]
    [SerializeField] private float _safeRadius = 3f;
    [SerializeField] private bool _isDebug = false;
    
    [Space(20)]
    [Header("Prefabs")]
    [SerializeField] private MeteorController[] _prefabMeteor = null;
    [SerializeField] private EnemyController[] _prefabEnemy = null;

    private List<MeteorController> _meteors = null;
    private Rect _worldConstrains;

    private void Awake()
    {
        _meteors = new List<MeteorController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnMeteors(_initialMeteors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMeteors(int num)
    {
        var camBounds = GetOrthoBounds(_camera);
        for (var i = 0; i < num; i++)
        {
            var prefab = _prefabMeteor[0];
            var position = GetRandomPosition(camBounds, transform.position, _safeRadius);
            var meteor = GameObject.Instantiate(prefab, position, Quaternion.identity, _parentMetors);
            meteor.name = $"Meteor_{i + 1}";
            _meteors.Add(meteor);
        }
        
        _hud.SetMeteorCounter(_meteors.Count);
    }

    public static Bounds GetOrthoBounds(Camera camera)
    {
        var screenAspect = Screen.width / (float)Screen.height;
        var cameraHeight = camera.orthographicSize * 2f;
        var bounds = new Bounds(
            camera.transform.position,
            new Vector3(
                cameraHeight * screenAspect,
                cameraHeight,
                0
            )
        );

        return bounds;
    }
    
    private static Vector2 GetRandomPosition(Bounds bounds, Vector2 origin, float safeRadius = 1f)
    {
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var y = Random.Range(bounds.min.y, bounds.max.y);
        var position = new Vector2(x, y);

        var direction = position - origin;
        if (direction.magnitude <= safeRadius)
        {
            position += direction.normalized * safeRadius;
        }

        return position;
    }
    
    private void OnDrawGizmos()
    {
        if (!_isDebug) return;
        
        var initialColor = Gizmos.color;
        Gizmos.color = Color.red;
        var points = GetRadiusPoints(transform.position, _safeRadius);
        Gizmos.DrawLineList(points);
        Gizmos.color = initialColor;
    }
    
    private Vector3[] GetRadiusPoints(Vector3 position, float radius)
    {
        const int degrees = 360;
        var points = new List<Vector3>();
        for (var i = 0; i < degrees; i++)
        {
            var y = Mathf.Sin(Mathf.Deg2Rad * i);
            var x = Mathf.Cos(Mathf.Deg2Rad * i);
            var point = new Vector3(x, y, 0f) * radius;
            point += position;
            points.Add(point);
        }

        return points.ToArray();
    }
    
}
