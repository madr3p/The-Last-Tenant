using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("Interaction")]
    public float talkDistance = 5f;

    [Header("UI")]
    public GameObject crosshair;
    public GameObject dialogueScreen;
    public GameObject talkPrompt;
    public TMPro.TMP_Text subtitleText;

    [Header("Dialogue")]
    public float dialogueDuration = 2f;
    public float typingSpeed = 0.03f;

    private Camera playerCamera;
    private GameInputActions inputActions;

    private Coroutine dialogueCoroutine;

    private bool isTyping;
    private bool skipLine;

    private void Awake()
    {
        playerCamera = Camera.main;
        inputActions = new GameInputActions();

        dialogueScreen.SetActive(false);
        subtitleText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void LateUpdate()
    {
        CheckForNPC();
    }

    private void CheckForNPC()
    {
        // Dialogue is currently playing
        if (subtitleText.gameObject.activeSelf)
        {
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                if (isTyping)
                {
                    skipLine = true;
                }
            }

            return;
        }

        bool lookingAtNPC = IsLookingAtNPC();

        if (lookingAtNPC)
        {
            dialogueScreen.SetActive(true);
            talkPrompt.SetActive(true);
            crosshair.SetActive(false);

            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                Ray ray = new Ray(
                    playerCamera.transform.position,
                    playerCamera.transform.forward
                );

                if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    talkDistance))
                {
                    NPC npc =
                        hit.collider.GetComponentInParent<NPC>();

                    if (npc != null)
                    {
                        StartDialogue(npc);
                    }
                }
            }
        }
        else
        {
            dialogueScreen.SetActive(false);
            talkPrompt.SetActive(false);
        }
    }

    public bool IsLookingAtNPC()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            talkDistance))
        {
            NPC npc =
                hit.collider.GetComponentInParent<NPC>();

            return npc != null;
        }

        return false;
    }

    private void StartDialogue(NPC npc)
    {
        talkPrompt.SetActive(false);

        subtitleText.gameObject.SetActive(true);

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        dialogueCoroutine =
            StartCoroutine(PlayDialogue(npc.dialogueLines));
    }

    private IEnumerator PlayDialogue(string[] lines)
    {
        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeLine(line));

            yield return StartCoroutine(WaitForNextLine());
        }

        subtitleText.text = "";
        subtitleText.gameObject.SetActive(false);

        dialogueScreen.SetActive(false);

        dialogueCoroutine = null;
    }

    private IEnumerator TypeLine(string line)
    {
        subtitleText.text = "";

        isTyping = true;
        skipLine = false;

        foreach (char letter in line)
        {
            if (skipLine)
            {
                subtitleText.text = line;
                break;
            }

            subtitleText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipLine = false;
    }

    private IEnumerator WaitForNextLine()
    {
        float timer = 0f;

        while (timer < dialogueDuration)
        {
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                yield break;
            }

            timer += Time.deltaTime;

            yield return null;
        }
    }
}