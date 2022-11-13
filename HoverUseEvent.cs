using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverUseEvent : MonoBehaviour
{
    public bool avaliable;
    public KeyCode interactBind;
    public UnityEvent onUse;
    public bool removeComponentOnUse;
    public bool removeGameObjectOnUse;

    private void Update()
    {
        if (avaliable)
        {
            if (Input.GetKeyDown(interactBind))
            {
                onUse.Invoke();

                if (removeComponentOnUse)
                {
                    Destroy(this);
                }
                else if (removeGameObjectOnUse)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
