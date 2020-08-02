using System;
using UnityEngine;

public class BasicCharacter : MonoBehaviour {
  [Header ("AnimationVariables")]
  [SerializeField]
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
  public KeyType keyType = KeyType.None;

  [Header ("Relations")]
  [SerializeField]
  private Animator animator;

  [SerializeField]
  private Rigidbody physicsBody;

  [SerializeField]
  private SpriteRenderer spriteRenderer;

  [SerializeField]
  private Light candleLight;
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
      var scareValue = candleScareValue;
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

    if (Input.GetKey (KeyCode.LeftShift) && stamina > 0) {
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
      // spriteRenderer.flipX = false;
    } else if (Input.GetKey (KeyCode.LeftArrow)) {
      inputX = -1;
      // spriteRenderer.flipX = true;
    }

    // Normalize
    _movement = new Vector3 (inputX, 0, inputY).normalized;

    SetAnimation ();
  }

  private void FixedUpdate () {
    if (isSprinting) {
      physicsBody.velocity = _movement * sprintSpeed;
    } else {
      physicsBody.velocity = _movement * speed;
    }
  }

  public void Scare (float scareValue, bool chasing = false) {
    fear += (scareValue / bravery) * Time.deltaTime;
    if (chasing) {
      fearBar.SetIntensePulse (true);
      fearBar.SetPulse (false);
    } else {
      fearBar.SetPulse (true);
      fearBar.SetIntensePulse (false);
    }
    fearBar.SetSliderValue (fear);
  }

  public void ScareRecovery () {
    if (fear > 0) {
      fear -= candleScareValue * Time.deltaTime;
      fearBar.SetSliderValue (fear);
    } else {
      fearBar.SetPulse (false);
      fearBar.SetIntensePulse (false);
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

  public void AddKey (KeyType keyType) {
    this.keyType = keyType;
  }

  public void UseKey () {
    this.keyType = KeyType.None;
  }

  /// <summary>
  /// LateUpdate is called every frame, if the Behaviour is enabled.
  /// It is called after all Update functions have been called.
  /// </summary>
  void LateUpdate () {
    spriteRenderer.transform.LookAt (Camera.main.transform.position);
  }

  public bool PlayerIsDead () {
    if (fear >= maxFear)
      return true;

    return false;
  }

  private void SetAnimation () {
    animator.SetBool (WALK_PROPERTY,
      Math.Abs (_movement.sqrMagnitude) > Mathf.Epsilon);

    animator.SetBool ("HasRedKey", keyType == KeyType.Red);
    animator.SetBool ("HasGreenKey", keyType == KeyType.Green);
    animator.SetBool ("HasBlueKey", keyType == KeyType.Blue);
    animator.SetBool ("CandleOn", candleOn);
  }

}