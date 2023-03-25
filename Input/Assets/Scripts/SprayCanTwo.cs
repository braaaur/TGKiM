using UnityEngine;

public class SprayCanTwo : MonoBehaviour
{
    public ParticleSystem sprayParticleSystem;
    public SpriteRenderer paintSplatPrefab; // Prefab for the paint splat effect
    public float sprayRadius = 0.1f; // Radius of the spray
    public int maxSplats = 10; // Maximum number of paint splats allowed on the screen at once

    private int numSplats; // Current number of paint splats on the screen
    private bool isSpraying; // Is the spray can currently being used?
    private Vector2 sprayDirection; // Direction that the spray is going

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Start spraying
            isSpraying = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Stop spraying
            isSpraying = false;
        }

        if (isSpraying)
        {
            // Calculate the spray direction based on the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            sprayDirection = (mousePos - (Vector2)transform.position).normalized;

            // Raycast to check for collisions with objects in the scene
            RaycastHit2D hit = Physics2D.Raycast(transform.position, sprayDirection, sprayRadius);
            if (hit.collider != null)
            {
                // Leave paint on the object that was hit
                LeavePaint(hit.point);
            }
            if (sprayParticleSystem != null)
            {
                sprayParticleSystem.Play();
            }
        }
        else
        {
            if (sprayParticleSystem != null)
            {
                sprayParticleSystem.Stop();
            }
        }
    }

    void LeavePaint(Vector2 position)
    {
        // Check if we have reached the maximum number of splats
        if (numSplats >= maxSplats)
        {
            return;
        }

        // Instantiate the paint splat prefab at the position of the spray can
        SpriteRenderer paintSplat = Instantiate(paintSplatPrefab, position, Quaternion.identity);

        // Randomly vary the size, rotation, and color of the paint splat
        float size = Random.Range(0.5f, 1.5f);
        paintSplat.transform.localScale = new Vector3(size, size, 1);
        paintSplat.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        paintSplat.color = Random.ColorHSV();

        // Increment the number of paint splats
        numSplats++;
    }
}