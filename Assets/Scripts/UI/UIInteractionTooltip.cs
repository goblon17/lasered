using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractionTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionIconPrefab;
    [SerializeField]
    private GameObject powerIconPrefab;
    [SerializeField]
    private GameObject colorIconPrefab;
    
    private Dictionary<Transform, InteractionIconController> interactionIcons = new Dictionary<Transform, InteractionIconController>();
    private Dictionary<Transform, ColorIconController> colorIcons = new Dictionary<Transform, ColorIconController>();
    private Dictionary<Transform, PowerIconController> powerIcons = new Dictionary<Transform, PowerIconController>();

    public void ShowInteraction(Transform transform)
    {
        if (interactionIcons.ContainsKey(transform))
        {
            return;
        }

        InteractionIconController interactionIconController = Instantiate(interactionIconPrefab, this.transform).GetComponent<InteractionIconController>();
        interactionIconController.ShowIcon(transform);
        interactionIcons.Add(transform, interactionIconController);
    }

    public void HideInteraction(Transform transform)
    {
        if (!interactionIcons.ContainsKey(transform))
        {
            return;
        }

        interactionIcons[transform].OnHideEvent += (e, _) => Destroy(((MonoBehaviour)e).gameObject);
        interactionIcons[transform].HideIcon();
        interactionIcons.Remove(transform);
    }

    public void ShowColor(Transform transform, System.Func<PlayerData.PlayerColor> colorStateFunc)
    {
        if (colorIcons.ContainsKey(transform))
        {
            return;
        }

        ColorIconController colorIconController = Instantiate(colorIconPrefab, this.transform).GetComponent<ColorIconController>();
        colorIconController.ShowIcon(transform, colorStateFunc);
        colorIcons.Add(transform, colorIconController);
    }

    public void HideColor(Transform transform)
    {
        if (!colorIcons.ContainsKey(transform))
        {
            return;
        }

        colorIcons[transform].OnHideEvent += (e, _) => Destroy(((MonoBehaviour)e).gameObject);
        colorIcons[transform].HideIcon();
        colorIcons.Remove(transform);
    }

    public void ShowPower(Transform transform, System.Func<bool> powerStateFunc)
    {
        if (powerIcons.ContainsKey(transform))
        {
            return;
        }

        PowerIconController powerIconController = Instantiate(powerIconPrefab, this.transform).GetComponent<PowerIconController>();
        powerIconController.ShowIcon(transform, powerStateFunc);
        powerIcons.Add(transform, powerIconController);
    }

    public void HidePower(Transform transform)
    {
        if (!powerIcons.ContainsKey(transform))
        {
            return;
        }

        powerIcons[transform].OnHideEvent += (e, _) => Destroy(((MonoBehaviour)e).gameObject);
        powerIcons[transform].HideIcon();
        powerIcons.Remove(transform);
    }
}
