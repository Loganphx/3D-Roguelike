using System;
using UnityEngine;

public class Bow : MonoBehaviour
{
    private Animator bowAnimator;
    // Position where to fire the arrow from
    private Transform arrowSpawnTransform;
    
    public GameObject arrowPrefab; // Drag and drop the arrow prefab in the Inspector
    public float maxDrawTime = 1f; // Maximum time to fully draw the arrow
    public float arrowForce = 200; // Force applied to the arrow when fired

    private GameObject currentArrow;
    private Rigidbody currentArrowRigidbody;
    private float currentDrawTime; // Time elapsed since the start of drawing

    // Caching the Animator Parameter at compile time is more efficient than constantly looking it up
    private static readonly int Draw = Animator.StringToHash("Draw");

    private void Awake()
    {
        bowAnimator = GetComponent<Animator>();
        arrowSpawnTransform = transform.Find("ArrowSpawn");
        
        if(arrowSpawnTransform == null) Debug.LogError("Failed to find ArrowSpawn", this);
    }

    private void Update()
    {
        // Checking if right click was pressed this frame
        if (Input.GetMouseButtonDown(1))
        {
            // If there's no current arrow, create one
            if (currentArrow == null)
            {
                currentArrow = Instantiate(arrowPrefab, arrowSpawnTransform.position, Quaternion.identity);
                // We parent the arrow to the bow, so that it moves with the bow while aimed
                currentArrow.transform.SetParent(arrowSpawnTransform);
                currentArrow.transform.forward = arrowSpawnTransform.forward;
                
                currentArrowRigidbody = currentArrow.GetComponent<Rigidbody>();
                
                // We set this to kinematic to stop physics while it's not fired.
                currentArrowRigidbody.isKinematic = true;
                currentDrawTime = 0f;
            }
            // Otherwise if there is already an arrow, simply re-enable it
            else if (!currentArrow.activeSelf)
            {
                currentArrow.SetActive(true);
            }
        }
        // Checking if right click is held
        if (Input.GetMouseButton(1) && !Input.GetMouseButtonDown(1))
        {
            // Increase draw time as long as the right mouse button is held
            if (currentDrawTime < maxDrawTime)
            {
                currentDrawTime = Mathf.Min(currentDrawTime + Time.deltaTime, maxDrawTime); // Clamp draw time to max

                // Set the arrow's draw progress (animation)
                SetArrowDrawProgress(currentDrawTime / maxDrawTime);
            }
      
        }
        
        if(!Input.GetMouseButton(1))
        {
            // Releasing right click disables the arrow
            if (currentArrow != null && currentArrow.activeSelf)
            {
                currentArrow.SetActive(false);
                currentDrawTime = 0f;
                SetArrowDrawProgress(0);
            }
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            // If the left mouse button is released, fire the arrow if it's fully drawn
            if (currentArrow != null && currentDrawTime >= maxDrawTime)
            {
                FireArrow();
            }
            
        }
    }

    void SetArrowDrawProgress(float progress)
    {
        // For simplicity, let's assume the bow has an animator with a "Draw" parameter.
        if (bowAnimator != null)
        {
            bowAnimator.SetFloat(Draw, progress);
        }
    }

    void FireArrow()
    {
        currentArrow.transform.SetParent(null, true);
        currentArrowRigidbody.mass = 0.1f; // Adjust the mass as needed for realistic physics
        currentArrowRigidbody.linearDamping = 0.1f; // Adjust the mass as needed for realistic physics
        currentArrowRigidbody.isKinematic = false;
        Vector3 fireDirection = currentArrow.transform.forward;
        currentArrowRigidbody.AddForce(fireDirection * arrowForce);

        // Reset the variables for the next shot
        currentArrow = null;
        currentDrawTime = 0f;
    }
}  