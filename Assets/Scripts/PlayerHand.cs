using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Camera playerCamera;
    public float maxRayDistance = 100f;

    public Chunk currentChunk;

    void Start()
    {
    }

    void Update()
    {
        HandleBlockSelection();
    }

    void HandleBlockSelection()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag("Undestroyable")) // The plane so player won't fall into the void
                {
                    return;
                }
                currentChunk.DestroyBlock(hitObject);
            }
        }
    }
}
