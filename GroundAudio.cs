using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAudio : MonoBehaviour
{
    public LayerMask groundLayer;

    public AudioClip[] GrassLeaves;
    public AudioClip[] Wood;
    public AudioClip[] Tiles;

    [Tooltip("Minimum velocity required for audio to play.")]
    public float velocityTreshold = .01f;

    public PlayerMovement character;
    AudioSource audioSource;

    Vector2 currectPlayerPosition => new Vector2(character.transform.position.x, character.transform.position.y);
    Vector2 lastPlayerPosition;

    public float walkingPitch = 1f;
    public float runningPitch = 1.3f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float velocity = Vector3.Distance(currectPlayerPosition, lastPlayerPosition);
        Debug.Log("Velocity: " + velocity);
        if(velocity >= velocityTreshold && character.IsGrounded)
        {
            SetPitch(character.IsRunning ? runningPitch : walkingPitch);
            CheckGround();
        }

        lastPlayerPosition = currectPlayerPosition;
    }

    void PlayFootstepSound(AudioClip[] footstepSound)
    {
        if(!audioSource.isPlaying)
        {
            AudioClip clip = footstepSound[Random.Range(0, footstepSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void SetPitch(float value)
    {
        audioSource.pitch = value;
    }

    void CheckGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            if(hit.collider.CompareTag("Wood"))
            {
                PlayFootstepSound(Wood);
            }
            else if(hit.collider.CompareTag("Tiles"))
            {
                PlayFootstepSound(Tiles);
            }
            else if(hit.collider.CompareTag("Terrain"))
            {
                PlayFootstepSound(GrassLeaves);
            }
        }
    }
}
