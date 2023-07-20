using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(LineRenderer))]
public class Laser : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [SerializeField]
    private float maxReflectionCount;
    [SerializeField]
    private LayerMask laserLayerMask;
    [SerializeField]
    private float maxDistance;

    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> directions = new List<Vector3>();

    private ElympicsVector3 startPosition = new ElympicsVector3(comparer: new ElympicsVector3EqualityComparer(0));
    private ElympicsVector3 startDirection = new ElympicsVector3(comparer: new ElympicsVector3EqualityComparer(0));

    private ElympicsInt playerColorInt = new ElympicsInt((int)PlayerData.PlayerColor.None);

    private LineRenderer lineRenderer;
    private float damage = 0;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawLine();
    }

    public void Begin(Vector3 pos, Vector3 dir, PlayerData.PlayerColor playerColor, float damage)
    {
        lineRenderer.positionCount = 0;
        playerColorInt.Value = (int)playerColor;
        startPosition.Value = pos;
        startDirection.Value = dir;
        this.damage = damage;
    }

    private void ChangeColor(int color)
    {
        lineRenderer.startColor = PlayerData.PlayerColorToColor((PlayerData.PlayerColor)color);
        lineRenderer.endColor = PlayerData.PlayerColorToColor((PlayerData.PlayerColor)color);
    }

    private void CalculateLaserPath()
    {
        positions.Clear();
        directions.Clear();
        if (startPosition.Value == null || startDirection.Value == null)
        {
            return;
        }
        positions.Add(startPosition.Value);
        directions.Add(startDirection.Value);
        for (int i = 0; i < positions.Count && i < directions.Count; i++)
        {
            CalculateNextPosition(i);
        }
    }

    private void CalculateNextPosition(int i)
    {
        if (directions[i] == Vector3.zero || i >= maxReflectionCount)
        {
            return;
        }
        if (Physics.Raycast(positions[i], directions[i], out RaycastHit hitInfo, maxDistance, laserLayerMask))
        {
            if (hitInfo.collider.TryGetComponent(out PlayerData playerData))
            {
                if (playerData.PlayerColorInt != playerColorInt.Value)
                {
                    if (Elympics.IsServer)
                    {
                        playerData.GetComponent<PlayerHealth>().TakeDamage(damage);
                    }
                }
                positions.Add(hitInfo.point);
                directions.Add(Vector3.zero);
            }
            else if (hitInfo.collider.TryGetComponent(out CubeReflection cubeReflection))
            {
                positions.Add(cubeReflection.Point);
                directions.Add(cubeReflection.Direction);
            }
            else
            {
                positions.Add(hitInfo.point);
                directions.Add(Vector3.zero);
                //directions.Add(Vector3.Reflect(directions[i], hitInfo.normal));
            }
        }
        else
        {
            positions.Add(positions[i] + directions[i] * maxDistance);
            directions.Add(Vector3.zero);
        }
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void Initialize()
    {
        playerColorInt.ValueChanged += (_, e) => ChangeColor(e);
    }

    public void ElympicsUpdate()
    {
        CalculateLaserPath();
    }
}
