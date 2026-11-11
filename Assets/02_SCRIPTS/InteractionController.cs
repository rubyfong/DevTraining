using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private float _InteractionDistance = 5f;
    public IInteractable CurrentTarget;

    private void Update()
    {
        DisableNPCOutlines();

        UpdateCurrentInteractable();

        CheckForInteractionInput();
    }

    private void DisableNPCOutlines()
    {
        foreach (GameObject npc in NPCManager.Instance.NPCSpawned)
        {
            IInteractable interactable = npc.transform.GetComponent<IInteractable>();
            if (interactable != CurrentTarget || CurrentTarget == null)
            {
                npc.transform.GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void UpdateCurrentInteractable()
    {
        var ray = Cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out var hit, _InteractionDistance);

        if (hit.collider == null)
        {
            CurrentTarget = null;
        }
        else if (hit.collider != null && hit.transform.TryGetComponent(out IInteractable interactable))
        {
            CurrentTarget = interactable;
            hit.transform.GetComponent<Outline>().enabled = true;
        }
    }

    private void CheckForInteractionInput()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && CurrentTarget != null)
        {
            CurrentTarget.Interact();
        }
    }
}
