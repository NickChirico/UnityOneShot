using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public SpriteRenderer sp;

    private PlayerStateManager SM;
    private UI_Manager ui;
    private SeraphController seraphControl;

    [Header("PLAYER")]
    public float invulnDuration;
    public float invulnBuffer;
    public float knockedForce;

    private bool canBeDamaged = true;
    private Vector2 hitPoint;

    private bool nimble = false;

    private void Awake()
    {
        SM = this.GetComponent<PlayerStateManager>();
    }
    public override void Start()
    {
        base.Start();
        ui = UI_Manager.GetUIManager;
        seraphControl = SeraphController.GetSeraphController;
    }

    //void Update()
    //{
    //}

    public override void TakeDamage(Entity attacker, int damageAmount, float posture)
    {
        base.TakeDamage(attacker, damageAmount, posture);
        seraphControl.ActivateArmorSeraphs(attacker, this.transform.position);
        ui.UpdateHealth(currentHealth, MaxHealth);
        SM.ChangeState(SM.Damaged);
        StartCoroutine(FlashRed());
    }

    public bool CanBeDamaged()
    { return canBeDamaged; }

    public bool IsNimble()
    { return nimble; }

    IEnumerator FlashRed()
    {
        sp.color = Color.red;
        canBeDamaged = false;
        yield return new WaitForSeconds(invulnDuration + invulnBuffer);
        canBeDamaged = true;
        sp.color = Color.white;
    }

    public void Nimble(bool b)
    {
        nimble = b;
        Physics2D.IgnoreLayerCollision(3, 6, b);// 3 = player, 6 = hittableEntity
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        ui.UpdateHealth(currentHealth, maxHealth);
    }
}
