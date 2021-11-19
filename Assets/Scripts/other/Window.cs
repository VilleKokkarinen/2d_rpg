using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup canvasGroup;

    private NPC npc;

    public virtual void Open(NPC npc)
    {
        this.npc = npc;

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        if(npc != null)
        {
            npc.IsInteracting = false;
            npc = null;
        }
      

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

      
    }
}
