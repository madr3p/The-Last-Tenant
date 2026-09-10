using System.Collections;
using UnityEngine;

public class GhostEncounter : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Jumpscare")]
    public float jumpscareDistance = 0.75f;
    public float jumpscareDuration = 2f;
    public float lungeDuration = 0.1f;
    public float lungeDelay = 0f;

    [Header("Respawn")]
    public float respawnDelay = 10f;

    private Transform player;
    private PlayerMovement playerMovement;
    private PlayerLook playerLook;

    private Renderer[] ghostRenderers;
    private Collider ghostCollider;

    private bool hasTriggered;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerMovement = player.GetComponent<PlayerMovement>();
        playerLook = player.GetComponentInChildren<PlayerLook>();

        ghostRenderers = GetComponentsInChildren<Renderer>();
        ghostCollider = GetComponent<Collider>();

        SpawnGhost();
    }

    private Transform lastSpawnPoint;

private void SpawnGhost()
{
    if (spawnPoints.Length == 0)
        return;

    Transform spawnPoint;

    if (spawnPoints.Length == 1)
    {
        spawnPoint = spawnPoints[0];
    }
    else
    {
        do
        {
            spawnPoint =
                spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        while (spawnPoint == lastSpawnPoint);
    }

    lastSpawnPoint = spawnPoint;

    transform.position = spawnPoint.position;
    transform.rotation = spawnPoint.rotation;

    Debug.Log(
    "Ghost spawned at: " + spawnPoint.name +
    " | Position: " + spawnPoint.position
    );
    
}

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        TriggerJumpscare();
    }

    private void TriggerJumpscare()
    {
        playerMovement.enabled = false;
        playerLook.enabled = false;

        StartCoroutine(JumpscareRoutine());

        Debug.Log("GHOST JUMPSCARE TRIGGERED!");
    }

    private IEnumerator JumpscareRoutine()
    {
        yield return new WaitForSeconds(lungeDelay);

        Vector3 direction =
            (player.position - transform.position).normalized;

        direction.y = 0f;
        direction.Normalize();

        Vector3 targetPosition =
            player.position - direction * jumpscareDistance;

        Vector3 startPosition = transform.position;

        float elapsed = 0f;

        while (elapsed < lungeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / lungeDuration;

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            yield return null;
        }

        transform.position = targetPosition;

        playerLook.ShakeCamera();

        transform.LookAt(
            new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            )
        );

        yield return new WaitForSeconds(
            jumpscareDuration - lungeDelay - lungeDuration
        );

        HideGhost();

        playerMovement.enabled = true;
        playerLook.enabled = true;

        yield return new WaitForSeconds(respawnDelay);

        SpawnGhost();

        ShowGhost();

        hasTriggered = false;

        Debug.Log("GHOST RESPAWNED!");
    }

    private void HideGhost()
    {
        foreach (Renderer renderer in ghostRenderers)
        {
            renderer.enabled = false;
        }

        if (ghostCollider != null)
        {
            ghostCollider.enabled = false;
        }
    }

    private void ShowGhost()
    {
        foreach (Renderer renderer in ghostRenderers)
        {
            renderer.enabled = true;
        }

        if (ghostCollider != null)
        {
            ghostCollider.enabled = true;
        }
    }
}