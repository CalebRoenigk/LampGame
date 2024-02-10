using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsUIManager : MonoBehaviour
{
    public static PlayerControlsUIManager Instance;
    public Image UpArrow;
    public Image DownArrow;
    public Image LeftArrow;
    public Image RightArrow;

    public Transform UpPlacement;
    public Transform DownPlacement;
    public Transform LeftPlacement;
    public Transform RightPlacement;

    private float _controlAlpha = 1f;
    
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

    public void UpdatePlayerMovementUI()
    {
        Vector2 halfScreen = new Vector2(-Screen.width / 2f, -Screen.height / 2f);
        if (PlayerController.Instance.MovementOptions.Contains(Vector2Int.up))
        {
            // UP
            UpArrow.gameObject.SetActive(true);
            UpArrow.rectTransform.anchoredPosition = (Vector2)Camera.main.WorldToScreenPoint(UpPlacement.position) + halfScreen;
        }
        else
        {
            UpArrow.gameObject.SetActive(false);
        }
        
        if (PlayerController.Instance.MovementOptions.Contains(Vector2Int.down))
        {
            // Down
            DownArrow.gameObject.SetActive(true);
            DownArrow.rectTransform.anchoredPosition = (Vector2)Camera.main.WorldToScreenPoint(DownPlacement.position) + halfScreen;
        }
        else
        {
            DownArrow.gameObject.SetActive(false);
        }
        
        if (PlayerController.Instance.MovementOptions.Contains(Vector2Int.left))
        {
            // Left
            LeftArrow.gameObject.SetActive(true);
            LeftArrow.rectTransform.anchoredPosition = (Vector2)Camera.main.WorldToScreenPoint(LeftPlacement.position) + halfScreen;
        }
        else
        {
            LeftArrow.gameObject.SetActive(false);
        }
        
        if (PlayerController.Instance.MovementOptions.Contains(Vector2Int.right))
        {
            // Right
            RightArrow.gameObject.SetActive(true);
            RightArrow.rectTransform.anchoredPosition = (Vector2)Camera.main.WorldToScreenPoint(RightPlacement.position) + halfScreen;
        }
        else
        {
            RightArrow.gameObject.SetActive(false);
        }

        if (PlayerController.Instance.Moving)
        {
            _controlAlpha = 0f;
        }
        else
        {
            _controlAlpha = 1f;
        }

        Color imageColor = Color.white;
        imageColor.a = _controlAlpha;
        UpArrow.color = imageColor;
        DownArrow.color = imageColor;
        LeftArrow.color = imageColor;
        RightArrow.color = imageColor;
    }
}
