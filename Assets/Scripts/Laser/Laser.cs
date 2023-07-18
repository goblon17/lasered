using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(LineRenderer))]
public class Laser : ElympicsMonoBehaviour, IInitializable
{
    [SerializeField]
    private float maxReflectionCount;

    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> directions = new List<Vector3>();

    private ElympicsVector3 startPosition = new ElympicsVector3();
    private ElympicsVector3 startDirection = new ElympicsVector3();

    private ElympicsInt playerColorInt = new ElympicsInt((int)PlayerData.PlayerColor.None);

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        CalculateLaserPath();
        DrawLine();
    }

    public void Begin(Vector3 pos, Vector3 dir, PlayerData.PlayerColor playerColor)
    {
        lineRenderer.positionCount = 0;
        playerColorInt.Value = (int)playerColor;
        startPosition.Value = pos;
        startDirection.Value = dir;
        CalculateLaserPath();
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
        print(startDirection.Value);
        if (startPosition.Value == null || startDirection.Value == null)
        {
            return;
        }
        positions.Add(startPosition.Value);
        directions.Add(startDirection.Value);
        for (int i = 0; i < positions.Count; i++)
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
        if (Physics.Raycast(positions[i], directions[i], out RaycastHit hitInfo))
        {
            positions.Add(hitInfo.point);
            directions.Add(Vector3.Reflect(directions[i], hitInfo.normal));
        }
        else
        {
            positions.Add(positions[i] + directions[i] * 1000);
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
}
