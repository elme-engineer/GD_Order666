
using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    // This method will be called to trigger the explosion and cat ammo logic
    public void TriggerExplosion(Vector3 position, ParticleSystem explosionEffect, GameObject catAmmo)
    {
        StartCoroutine(WaitForExplosionAndShowAmmo(explosionEffect, position, catAmmo));
    }

    // Coroutine to wait for the explosion to finish and show the cat ammo
   private IEnumerator WaitForExplosionAndShowAmmo(ParticleSystem explosion, Vector3 position, GameObject catAmmo)
{
    yield return new WaitForSeconds(explosion.main.duration);

    // Check if the cat ammo exists
    if (catAmmo != null)
    {
        // Adjust the Z position to make the cat ammo a little higher
        Vector3 newPosition = new Vector3(position.x, position.y+ 1.5f, position.z ); // Adjust 1f to your desired height

        // Set the new position with the adjusted Z
        catAmmo.transform.position = newPosition;

        // Activate the cat ammo
        catAmmo.SetActive(true);

        Debug.Log("Cat Ammo position: " + catAmmo.transform.position);
    }
        // Destroy the explosion effect after it finishes
        Destroy(explosion.gameObject);
    }
}

