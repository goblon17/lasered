using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionIconPrefab;
    
    private Dictionary<IInteractable, InteractionIconController> interactionIcons = new Dictionary<IInteractable, InteractionIconController>();

    public void ShowInteraction(IInteractable interactable, Transform transform)
    {
        if (interactionIcons.ContainsKey(interactable))
        {
            return;
        }

        InteractionIconController interactionIconController = Instantiate(interactionIconPrefab, this.transform).GetComponent<InteractionIconController>();
        interactionIconController.ShowIcon(transform);
        interactionIcons.Add(interactable, interactionIconController);
    }

    public void HideInteraction(IInteractable interactable)
    {
        if (!interactionIcons.ContainsKey(interactable))
        {
            return;
        }

        interactionIcons[interactable].OnHideEvent += (e, _) => Destroy(((MonoBehaviour)e).gameObject);
        interactionIcons[interactable].HideIcon();
        interactionIcons.Remove(interactable);
    }
}
