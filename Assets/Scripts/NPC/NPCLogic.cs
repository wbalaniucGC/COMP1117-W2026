using UnityEngine;

public class NPCLogic : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject speechBubble;

    public void Interact()
    {
        // Safety check
        if(speechBubble == null)
        {
            // If the speech bubble doesn't exist, return immediately and do nothing.
            return;
        }

        // Speech bubble DOES exist!
        bool isCurrentlyActive = speechBubble.activeSelf;

        speechBubble.SetActive(!isCurrentlyActive);

        Debug.Log("NPC: Interaction toggled");
    }
}
