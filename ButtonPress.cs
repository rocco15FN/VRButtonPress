using UnityEngine;
using System.Collections;
using System;

public class ButtonPress : MonoBehaviour
{

    public bool lockX;
    public bool lockY;
    public bool lockZ;

    public float returnSpeed;
    public float activationDistance;

    public Color inactiveColor;
    public Color activeColor;

    public GameObject IndicatorObject;

    private bool pressed = false;
    private bool released = false;
    private Vector3 startPosition;

    void Start()
    {
        // Remember start position of button
        startPosition = transform.localPosition;
    }

    void Update()
    {
        released = false;

        // Use local position freez instead of global, so button can be rotated in any direction
        Vector3 localPos = transform.localPosition;
        if (lockX) localPos.x = startPosition.x;
        if (lockY) localPos.y = startPosition.y;
        if (lockZ) localPos.z = startPosition.z;
        transform.localPosition = localPos;

        // Retrun button to startPosition
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);

        //Get distance of button press
        Vector3 distance = transform.localPosition - startPosition;
        float distanceY = Math.Abs(distance.y);
        float pressComplete = Mathf.Clamp(1 / activationDistance * distanceY, 0f, 1f);

        //Call ButtonPressed() once button is pressed
        if (pressComplete >= 0.7f && !pressed)
        {
            pressed = true;
            StartCoroutine(ChangeColor(gameObject, inactiveColor, activeColor, 0.2f));
        }
        else if (pressComplete <= 0.2f && pressed)
        {
            pressed = false;
            released = true;
            StartCoroutine(ChangeColor(gameObject, activeColor, inactiveColor, 0.3f));
        }

        //Gradually color the indicator when button is pressed
        if (IndicatorObject) IndicatorObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, activeColor, pressComplete);
    }


    public bool IsPressed
    {
        get
        {
            return pressed;
        }
    }

    public bool IsReleased
    {
        get
        {
            return released;
        }
    }


    private IEnumerator ChangeColor(GameObject obj, Color from, Color to, float duration)
    {
        float timeElapsed = 0.0f;
        float t = 0.0f;

        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;
            t = timeElapsed / duration;
            obj.GetComponent<Renderer>().material.color = Color.Lerp(from, to, t);
            yield return null;
        }

    }

}
