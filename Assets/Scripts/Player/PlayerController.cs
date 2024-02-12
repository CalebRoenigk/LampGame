using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    public bool Moving = false;
    public Vector2Int CurrentPosition = Vector2Int.zero;
    public Vector2Int NextPosition = Vector2Int.zero;
    public float MovementDuration = 0.375f;
    public float MovementAdditionalDurations = 0.1f;
    public float MovementTotalDuration = 0f;
    public float MovementTime = 0f;
    public Queue<WirePlacementData> WirePlacements = new Queue<WirePlacementData>();
    public List<Vector2Int> MovementOptions = new List<Vector2Int>();
    public int Power = 8;
    [SerializeField] private WireReference _wireReference;
    [SerializeField] private MeshFilter _meshFilter;
    
    // Start is called before the first frame update
    void Start()
    {
        CheckMovementOptions();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Moving)
        {
            if (Power > 0)
            {
                Vector2Int movementDirection = Vector2Int.zero;
            
                if (Input.GetKey(KeyCode.UpArrow) && MovementOptions.Contains(Vector2Int.up))
                {
                    movementDirection = Vector2Int.up;
                    WireData wireData = _wireReference.GetWireData(WireDirection.Up, WireSegment.Start);
                    _meshFilter.mesh = wireData.WireMesh;
                    transform.rotation = Quaternion.Euler(wireData.Rotation.x, wireData.Rotation.y, wireData.Rotation.z);
                } else if (Input.GetKey(KeyCode.DownArrow) && MovementOptions.Contains(Vector2Int.down))
                {
                    movementDirection = Vector2Int.down;
                    WireData wireData = _wireReference.GetWireData(WireDirection.Down, WireSegment.Start);
                    _meshFilter.mesh = wireData.WireMesh;
                    transform.rotation = Quaternion.Euler(wireData.Rotation.x, wireData.Rotation.y, wireData.Rotation.z);
                } else if (Input.GetKey(KeyCode.LeftArrow) && MovementOptions.Contains(Vector2Int.left))
                {
                    movementDirection = Vector2Int.left;
                    WireData wireData = _wireReference.GetWireData(WireDirection.Left, WireSegment.Start);
                    _meshFilter.mesh = wireData.WireMesh;
                    transform.rotation = Quaternion.Euler(wireData.Rotation.x, wireData.Rotation.y, wireData.Rotation.z);
                } else if (Input.GetKey(KeyCode.RightArrow) && MovementOptions.Contains(Vector2Int.right))
                {
                    movementDirection = Vector2Int.right;
                    WireData wireData = _wireReference.GetWireData(WireDirection.Right, WireSegment.Start);
                    _meshFilter.mesh = wireData.WireMesh;
                    transform.rotation = Quaternion.Euler(wireData.Rotation.x, wireData.Rotation.y, wireData.Rotation.z);
                }

                if (movementDirection != Vector2Int.zero)
                {
                    int powerRemaining = Power;
                    NextPosition = GridManager.Instance.FindStopInDirection(CurrentPosition, movementDirection, Power, out powerRemaining);
                    Power = powerRemaining;
                    MovementTotalDuration =
                        (Vector2Int.Distance(CurrentPosition, NextPosition) - 1) * MovementAdditionalDurations +
                        MovementDuration;
                    Moving = true;
                    WirePlacements.Clear();
                    int totalWireCount = Mathf.RoundToInt(Vector2Int.Distance(NextPosition, CurrentPosition))+1;
                    Vector2Int placementPosition = new Vector2Int(CurrentPosition.x, CurrentPosition.y);
                    for (int i = 0; i < totalWireCount; i++)
                    {
                        WireSegment segment = WireSegment.Start;
                        if (i != 0)
                        {
                            segment = WireSegment.Middle;
                        }

                        if (i == totalWireCount - 1)
                        {
                            segment = WireSegment.End;
                        }
                        
                        float placementTime = Mathf.Clamp((Vector2Int.Distance(CurrentPosition, placementPosition-movementDirection) - 1), 0 , totalWireCount) * MovementAdditionalDurations + (MovementDuration/totalWireCount);
                        WirePlacements.Enqueue(new WirePlacementData(placementTime, ConvertDirectionToWireDirection(movementDirection), segment, placementPosition));
                        placementPosition += movementDirection;
                    }
                } 
            }
        }
        else
        {
            PlayerControlsUIManager.Instance.UpdatePlayerMovementUI();
            MoveToNext();
        }
    }

    private void MoveToNext()
    {
        float linearTween = Mathf.Clamp(MovementTime / MovementTotalDuration, 0f, 1f);
        float easeTween = EaseOutQuint(linearTween);

        transform.position = Vector3.Lerp(new Vector3(CurrentPosition.x, 0f, CurrentPosition.y), new Vector3(NextPosition.x, 0f, NextPosition.y), easeTween);
        
        // Wire placement
        if (WirePlacements.Count > 0)
        {
            if (MovementTime >= WirePlacements.Peek().Time)
            {
                WirePlacementData placementData = WirePlacements.Dequeue();
                WireManager.Instance.PlaceWire(placementData.Position, placementData.Direction, placementData.Segment);
            }
        }

        if (linearTween >= 1f)
        {
            Moving = false;
            transform.position = new Vector3(NextPosition.x, 0f, NextPosition.y);
            CurrentPosition = new Vector2Int(NextPosition.x, NextPosition.y);
            MovementTime = 0f;
            MovementTotalDuration = 0f;
            CheckMovementOptions();
            CheckCellEffects();

            if (WirePlacements.Count > 0)
            {
                while (WirePlacements.Count > 0)
                {
                    WirePlacementData placementData = WirePlacements.Dequeue();
                    WireManager.Instance.PlaceWire(placementData.Position, placementData.Direction, placementData.Segment);
                }
            }
        }
        MovementTime += Time.deltaTime;
    }

    private void CheckMovementOptions()
    {
        MovementOptions.Clear();

        if (GridManager.Instance.Grid.CheckCanMove(CurrentPosition + Vector2Int.up))
        {
            MovementOptions.Add(Vector2Int.up);
        }
        if (GridManager.Instance.Grid.CheckCanMove(CurrentPosition + Vector2Int.down))
        {
            MovementOptions.Add(Vector2Int.down);
        }
        if (GridManager.Instance.Grid.CheckCanMove(CurrentPosition + Vector2Int.left))
        {
            MovementOptions.Add(Vector2Int.left);
        }
        if (GridManager.Instance.Grid.CheckCanMove(CurrentPosition + Vector2Int.right))
        {
            MovementOptions.Add(Vector2Int.right);
        }
        
        PlayerControlsUIManager.Instance.UpdatePlayerMovementUI();
    }

    private float EaseOutQuint(float t)
    {
        return 1f - Mathf.Pow(1f - t, 5);
    }

    // Checks if the cell should have any effects on the player
    private void CheckCellEffects()
    {
        GridManager.Instance.Grid.CheckPlayerEffects(CurrentPosition);
    }

    private WireDirection ConvertDirectionToWireDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return WireDirection.Up;
        }
        
        if (direction == Vector2Int.down)
        {
            return WireDirection.Down;
        }
        
        if (direction == Vector2Int.left)
        {
            return WireDirection.Left;
        }
        
        return WireDirection.Right;
    }
}
