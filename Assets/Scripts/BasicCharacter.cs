using System;
using UnityEngine;

public class BasicCharacter : MonoBehaviour {
  private static readonly int WALK_PROPERTY = Animator.StringToHash ("Walk");

  [Header ("Stats")]

  [SerializeField]
  private bool candleOn = true;

  [SerializeField]
  private float speed = 2f;

  [SerializeField]
  private float fear = 0.0f;

  [SerializeField]
  private float stamina = 100f;

  [SerializeField]
  private int bravery = 1;

  [Header ("Relations")]
  [SerializeField]
  private Animator animator = null;

  [SerializeField]
  private Rigidbody physicsBody = null;

  [SerializeField]
  private SpriteRenderer spriteRenderer = null;

  [SerializeField]
  private Light candleLight = null;
  [SerializeField]
  private UIBarManager fearBar;
  [SerializeField]
  private UIBarManager staminaBar;

  private Vector3 _movement;

  private void Start () {
    fearBar.SetMaxSliderValue (100, 0);
    staminaBar.SetMaxSliderValue (100, 100);
  }

  private void Update () {

    if (!candleOn) {
      fear += 1 * Time.deltaTime;
      fearBar.SetSliderValue (fear);

    fearBar.SetPulse (true);
    }

    if (Input.GetKeyDown (KeyCode.C)) {
      if (candleOn) {
        candleOn = false;
        candleLight.enabled = false;
      } else {
        candleOn = true;
        candleLight.enabled = true;
        
      }
    }

    // Vertical
    float inputY = 0;
    if (Input.GetKey (KeyCode.UpArrow))
      inputY = 1;
    else if (Input.GetKey (KeyCode.DownArrow))
      inputY = -1;

    // Horizontal
    float inputX = 0;
    if (Input.GetKey (KeyCode.RightArrow)) {
      inputX = 1;
      spriteRenderer.flipX = false;
    } else if (Input.GetKey (KeyCode.LeftArrow)) {
      inputX = -1;
      spriteRenderer.flipX = true;
    }

    // Normalize
    _movement = new Vector3 (inputX, 0, inputY).normalized;

    animator.SetBool (WALK_PROPERTY,
      Math.Abs (_movement.sqrMagnitude) > Mathf.Epsilon);
  }

  private void FixedUpdate () {
    physicsBody.velocity = _movement * speed;
  }

  private void LateUpdate () {
    // transform.LookAt(Camera.main.transform.position);
  }

  public void Scare (float scareValue) {
    fear += scareValue;
    fearBar.SetPulse (true);
  }

}