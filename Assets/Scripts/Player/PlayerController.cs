using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<Vector2Int> MovementOptions = new List<Vector2Int>();
    
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
            Vector2Int movementDirection = Vector2Int.zero;
            
            if (Input.GetKey(KeyCode.UpArrow) && MovementOptions.Contains(Vector2Int.up))
            {
                movementDirection = Vector2Int.up;
            } else if (Input.GetKey(KeyCode.DownArrow) && MovementOptions.Contains(Vector2Int.down))
            {
                movementDirection = Vector2Int.down;
            } else if (Input.GetKey(KeyCode.LeftArrow) && MovementOptions.Contains(Vector2Int.left))
            {
                movementDirection = Vector2Int.left;
            } else if (Input.GetKey(KeyCode.RightArrow) && MovementOptions.Contains(Vector2Int.right))
            {
                movementDirection = Vector2Int.right;
            }

            if (movementDirection != Vector2Int.zero)
            {
                NextPosition = GridManager.Instance.FindStopInDirection(CurrentPosition, movementDirection);
                MovementTotalDuration =
                    (Vector2Int.Distance(CurrentPosition, NextPosition) - 1) * MovementAdditionalDurations +
                    MovementDuration;
                Moving = true;
                WireManager.Instance.PlaceWires(CurrentPosition, NextPosition);
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

        transform.position = Vector3.Lerp(new Vector3(CurrentPosition.x, 0f, CurrentPosition.y),
            new Vector3(NextPosition.x, 0f, NextPosition.y), easeTween);
        if (linearTween >= 1f)
        {
            Moving = false;
            transform.position = new Vector3(NextPosition.x, 0f, NextPosition.y);
            CurrentPosition = new Vector2Int(NextPosition.x, NextPosition.y);
            MovementTime = 0f;
            MovementTotalDuration = 0f;
            CheckMovementOptions();
        }
        MovementTime += Time.deltaTime;
    }

    private void CheckMovementOptions()
    {
        MovementOptions.Clear();

        if (!GridManager.Instance.Grid.CheckOccupied(CurrentPosition + Vector2Int.up))
        {
            MovementOptions.Add(Vector2Int.up);
        }
        if (!GridManager.Instance.Grid.CheckOccupied(CurrentPosition + Vector2Int.down))
        {
            MovementOptions.Add(Vector2Int.down);
        }
        if (!GridManager.Instance.Grid.CheckOccupied(CurrentPosition + Vector2Int.left))
        {
            MovementOptions.Add(Vector2Int.left);
        }
        if (!GridManager.Instance.Grid.CheckOccupied(CurrentPosition + Vector2Int.right))
        {
            MovementOptions.Add(Vector2Int.right);
        }
        
        PlayerControlsUIManager.Instance.UpdatePlayerMovementUI();
    }

    private float EaseOutQuint(float t)
    {
        return 1f - Mathf.Pow(1f - t, 5);
    }
}
