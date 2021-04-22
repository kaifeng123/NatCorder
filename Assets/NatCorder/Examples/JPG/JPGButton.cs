using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class JPGButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public UnityEvent onTouchDown, onTouchUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // Start counting
        Debug.Log(" OnPointerDown ");
        onTouchDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        // Reset pressed
        onTouchUp?.Invoke();
    }
}
