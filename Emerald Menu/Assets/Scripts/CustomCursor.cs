using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture1;
    public Texture2D cursorTexture2;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private int buttonHovered = 0;
    public AudioSource btnHvrSFX;
    /*
    public void OnMouseEnter()
    {
        buttonHovered += 1;
        btnHvrSFX.Play();
    }

    public void OnMouseExit()
    {
        buttonHovered -= 1;
    }

    void Update()
    {
        if (buttonHovered > 0)
        {
            Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(cursorTexture1, hotSpot, cursorMode);
        }
    }
    */

    void Start()
    {
        Cursor.SetCursor(cursorTexture1, hotSpot, cursorMode);
    }

    public void CursorPointer()
    {
        Cursor.SetCursor(cursorTexture1, hotSpot, cursorMode);
    }

    public void CursorHand()
    {
        Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
    }

    public void CursorHand(Button button)
    {
        if (button.interactable == true)
        {
            Cursor.SetCursor(cursorTexture2, hotSpot, cursorMode);
        }
    }
}
