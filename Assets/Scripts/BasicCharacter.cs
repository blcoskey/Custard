using System;
using UnityEngine;

public class BasicCharacter : MonoBehaviour {
  private static readonly int WALK_PROPERTY = Animator.StringToHash ("Walk");

  [Header ("Stats")]

  [SerializeField]
  private bool candleOn = true;
  [SerializeField]
  private float candleScareValue = 5.0f;

  [SerializeField]
  private float speed = 2f;
  [SerializeField]
  private float sprintSpeed = 4f;
  [SerializeField]
  private bool isSprinting = false;

  [SerializeField]
  private float maxFear = 100f;

  [SerializeField]
  private float maxStamina = 100f;

  [SerializeField]
  private float fear = 0.0f;

  [SerializeField]
  private float stamina = 100f;

  [SerializeField]
  private float staminaDrain = 5f;

  [SerializeField]
  private float fitness = 1f;

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
    fearBar.SetMaxSliderValue (maxFear, 0);
    staminaBar.SetMaxSliderValue (maxStamina, stamina);
  }

  private void Update () {

    if (!candleOn) {
      var scareValue = (candleScareValue / bravery) * Time.deltaTime;
      Scare (scareValue);
    } else {
      ScareRecovery ();
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

    if (Input.GetKey (KeyCode.LeftShift)) {
      Sprint ();
    } else { RecoverSprint (); }

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
    if (isSprinting) {
      physicsBody.velocity = _movement * sprintSpeed;
    } else {
      physicsBody.velocity = _movement * speed;
    }
  }

  private void LateUpdate () {
    // transform.LookAt(Camera.main.transform.position);
  }

  public void Scare (float scareValue) {
    fear += scareValue;
    fearBar.SetPulse (true);
    fearBar.SetSliderValue (fear);
  }

  public void ScareRecovery () {
    if (fear > 0) {
      fear -= candleScareValue * Time.deltaTime;
      fearBar.SetSliderValue (fear);
    } else {
      fearBar.SetPulse (false);
    }
  }

  private void Sprint () {
    isSprinting = true;
    stamina -= staminaDrain * Time.deltaTime / fitness;
    staminaBar.SetSliderValue (stamina);
    staminaBar.SetPulse (true);
  }

  private void RecoverSprint () {
    isSprinting = false;

    if (stamina < maxStamina) {
      stamina += staminaDrain * Time.deltaTime;
      staminaBar.SetSliderValue (stamina);
    }

    if (stamina < maxStamina) {
      staminaBar.SetPulse (true);
    } else {
      staminaBar.SetPulse (false);
    }
  }

}