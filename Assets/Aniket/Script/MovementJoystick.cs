using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBackground;
    public Vector2 joystickTouchPosition;
    public Vector2 joystickVector;
    private Vector2 joystickOriginalPosition;
    private float joystickRadius;
    // Start is called before the first frame update
    void Start()
    {
        joystickOriginalPosition = joystickBackground.transform.position;
        joystickRadius = joystickBackground.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBackground.transform.position = Input.mousePosition;
        joystickTouchPosition = Input.mousePosition;
    }

    public void PointerUp()
    {
        joystickVector = Vector2.zero;
        joystick.transform.position = joystickOriginalPosition;
        joystickBackground.transform.position = joystickOriginalPosition;
    }
 
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPosition = pointerEventData.position;
        joystickVector = (dragPosition - joystickTouchPosition).normalized;
        float joystickDistance = Vector2.Distance(dragPosition, joystickTouchPosition);

        if (joystickDistance < joystickRadius)
            joystick.transform.position = joystickTouchPosition + joystickVector * joystickDistance;
        else
            joystick.transform.position = joystickTouchPosition + joystickVector * joystickRadius;
    }
}
