using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorGeneratorController : MonoBehaviour
{
    [Header("Balancing")]
    [SerializeField, Range(0.5f, 20f)] private float safeRadius = 1.5f; 
    [SerializeField] private bool isDebug = true;
    [SerializeField] private Color safeRadiusColor = Color.red;
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform _meteorsParent;
    [SerializeField] private MeteorController[] _meteorPrefabs;
    
    private List<MeteorController> _meteors;
    public MeteorController[] Meteors => _meteors.ToArray();
    
    private Coroutine rockCreationCoroutine = null;

    public void Generate(int quantity = 1)
    {
        StopCoroutine(rockCreationCoroutine);
        Clear();
        rockCreationCoroutine = StartCoroutine(GenerateRocks(quantity));
    }

    public void Clear()
    {
        if (_meteors == null || _meteors.Count == 0) return;

        foreach (var r in _meteors)
        {
            Destroy(r.gameObject);
        }
        _meteors.Clear();
    }

    private IEnumerator GenerateRocks(int quantity)
    {
        _meteors = new List<MeteorController>();
        var camBounds = GetOrthoBounds(mainCamera);

        for (var i = 0; i < quantity; i++)
        {
            var template = GetRandomTemplate(_meteorPrefabs);
            var rock = Instantiate(template, _meteorsParent);
            var position = GetRandomPosition(camBounds, transform.position, safeRadius);
            
            rock.transform.position = position;
            _meteors.Add(rock);
            
            yield return null;
        }
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
    

    private static MeteorController GetRandomTemplate(MeteorController[] templates)
    {
        return templates[Random.Range(0, templates.Length)];
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
        if (!isDebug) return;
        
        var initialColor = Gizmos.color;
        Gizmos.color = safeRadiusColor;
        var points = GetRadiusPoints(transform.position, safeRadius);
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