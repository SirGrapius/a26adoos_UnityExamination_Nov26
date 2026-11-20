using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextMeshPro interactText;
    [SerializeField] private TextMeshPro dialogueBox;
    [SerializeField] private BoxCollider2D interactBox;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LimitedPlatformerMovement player;
    
    [SerializeField] private List<string> dialogues;
    [SerializeField] private List<string> victoryDialogues;
    [SerializeField] private List<string> currentDialogues;
    private int dialogueIndex;

    private bool dialogueCooldown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        interactText = GetComponentInChildren<TextMeshPro>();
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<TextMeshPro>();
        interactText.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.inDialogue && !dialogueCooldown && Keyboard.current[Key.E].isPressed)
        {
            dialogueIndex++;
        }
        
        dialogueBox.text = currentDialogues[dialogueIndex];

        if (dialogueIndex >= currentDialogues.Count - 1)
        {
            EndDialogue();
        }
    }

    public void StartDialogue()
    {
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        
        if (player.hasGem)
        {
            currentDialogues = victoryDialogues;
        }
        else
        {
            currentDialogues = dialogues;
        }
        interactText.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(true);
        player.inDialogue = true;
        dialogueBox.text = currentDialogues[dialogueIndex];
    }

    private IEnumerator Cooldown()
    {
        dialogueCooldown = true;
        yield return new WaitForSeconds(0.25f);
        dialogueCooldown = false;
    }

    private void EndDialogue()
    {
        player.inDialogue = false;
        
        dialogueBox.gameObject.SetActive(false);
        dialogueIndex = 0;
        interactText.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<LimitedPlatformerMovement>();
            interactText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = null;
            interactText.gameObject.SetActive(false);
        }
    }
}
