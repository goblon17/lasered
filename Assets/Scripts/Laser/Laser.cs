using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;

[RequireComponent(typeof(LineRenderer))]
public class Laser : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    private class ChildLaser
    {
        public bool InUse = false;
        public Laser Laser = null;
    }

    [SerializeField]
    private float maxReflectionCount;
    [SerializeField]
    public int childLasersLimit;
    [SerializeField]
    private LayerMask laserLayerMask;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private string laserPrefabResourcePath;
    [SerializeField]
    private LineRenderer lineRenderer;

    public int ChildLaserCount => childLasers.Count + childLasers.Sum(x => x.Laser.ChildLaserCount);

    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> directions = new List<Vector3>();

    private ElympicsVector3 startPosition = new ElympicsVector3(comparer: new ElympicsVector3EqualityComparer(0));
    private ElympicsVector3 startDirection = new ElympicsVector3(comparer: new ElympicsVector3EqualityComparer(0));

    private ElympicsInt playerColorInt = new ElympicsInt((int)PlayerData.PlayerColor.None);

    private float damage = 0;

    private Receiver currentlyActivatedReceiver = null;
    private int currentReceiverArgument;

    private List<ChildLaser> childLasers = new List<ChildLaser>();

    private void Update()
    {
        DrawLine();
    }

    public void Begin(Vector3 pos, Vector3 dir, PlayerData.PlayerColor playerColor, float damage)
    {
        if (this != null)
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            lineRenderer.positionCount = 0;
        }
        playerColorInt.Value = (int)playerColor;
        startPosition.Value = pos;
        startDirection.Value = dir;
        this.damage = damage;
    }

    public void Stop()
    {
        if (this != null)
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            lineRenderer.positionCount = 0;
        }
        startPosition.Value = Vector3.zero;
        startDirection.Value = Vector3.zero;
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
            bool endedOnReceiver = false;

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
            else if (hitInfo.collider.TryGetComponent(out CubeDiffraction cubeDiffraction))
            {
                var diffraction = cubeDiffraction.HandleLaserDiffraction(directions[i]);

                positions.Add(diffraction.StartPoint);
                directions.Add(diffraction.StartDirection);

                positions.Add(diffraction.EndPoint);
                directions.Add(diffraction.EndDirection);

                var availableChild = childLasers.FirstOrDefault(x => !x.InUse);
                if (availableChild == null && ChildLaserCount < childLasersLimit)
                {
                    ChildLaser childLaser = new ChildLaser();
                    childLaser.Laser = ElympicsInstantiate(laserPrefabResourcePath, ElympicsPlayer.All).GetComponent<Laser>();
                    childLasers.Add(childLaser);
                    childLasers.ForEach(x => x.Laser.childLasersLimit = childLasersLimit - ChildLaserCount);
                }
                availableChild = childLasers.FirstOrDefault(x => !x.InUse);
                if (availableChild != null)
                {
                    availableChild.InUse = true;
                    availableChild.Laser.Begin(diffraction.AdditionalPoint, diffraction.AdditionalDirection, (PlayerData.PlayerColor)playerColorInt.Value, damage);
                }

            }
            else if (hitInfo.collider.TryGetComponent(out Receiver receiver))
            {
                positions.Add(receiver.Point);
                directions.Add(Vector3.zero);
                if (currentlyActivatedReceiver != receiver)
                {
                    if (currentlyActivatedReceiver != null)
                    {
                        currentlyActivatedReceiver.Deactivate(currentReceiverArgument);
                    }
                    currentReceiverArgument = playerColorInt.Value - 1;
                    receiver.Activate(currentReceiverArgument);
                    currentlyActivatedReceiver = receiver;
                }
                else if (currentReceiverArgument != playerColorInt.Value - 1)
                {
                    currentlyActivatedReceiver.Deactivate(currentReceiverArgument);
                    currentReceiverArgument = playerColorInt.Value - 1;
                    currentlyActivatedReceiver.Activate(currentReceiverArgument);
                }
                endedOnReceiver = true;
            }
            else if (hitInfo.collider.CompareTag("Reflective"))
            {
                positions.Add(hitInfo.point);
                directions.Add(Vector3.Reflect(directions[i], hitInfo.normal));
            }
            else
            {
                positions.Add(hitInfo.point);
                directions.Add(Vector3.zero);
            }

            if (!endedOnReceiver && directions[directions.Count - 1] == Vector3.zero && currentlyActivatedReceiver != null)
            {
                currentlyActivatedReceiver.Deactivate(currentReceiverArgument);
                currentlyActivatedReceiver = null;
            }
        }
        else
        {
            positions.Add(positions[i] + directions[i] * maxDistance);
            directions.Add(Vector3.zero);

            if (currentlyActivatedReceiver != null)
            {
                currentlyActivatedReceiver.Deactivate(currentReceiverArgument);
                currentlyActivatedReceiver = null;
            }
        }
    }

    private void ResetChildLasers()
    {
        childLasers.RemoveAll(x => 
        {
            if (!x.InUse)
            {
                if (Elympics.IsServer)
                {
                    ElympicsDestroy(x.Laser.gameObject);
                }
            }
            return !x.InUse;
        });
        childLasers.ForEach(x => x.Laser.childLasersLimit = childLasersLimit - ChildLaserCount);
        foreach (ChildLaser childLaser in childLasers)
        {
            childLaser.InUse = false;
            childLaser.Laser.Stop();
        }
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    public void Initialize()
    {
        ChangeColor(playerColorInt.Value);
        playerColorInt.ValueChanged += (_, e) => ChangeColor(e);
    }

    public void ElympicsUpdate()
    {
        ResetChildLasers();
        CalculateLaserPath();
    }

    private void OnDestroy()
    {
        if (currentlyActivatedReceiver != null)
        {
            currentlyActivatedReceiver.Deactivate(currentReceiverArgument);
            currentlyActivatedReceiver = null;
        }
    }
}
