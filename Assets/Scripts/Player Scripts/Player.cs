using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sp;

    private PlayerStateManager SM;
    private UI_Manager ui;
    private SeraphController seraphControl;


    public PopUpDamageText damageText;


    public int maxHealth;
    public float invulnDuration;
    public float invulnBuffer;
    public float knockedForce;

    private int currentHealth;
    private bool canBeDamaged = true;
    private Vector2 hitPoint;

    private bool nimble = false;

    private void Awake()
    {
        SM = this.GetComponent<PlayerStateManager>();
    }
    void Start()
    {
        ui = UI_Manager.GetUIManager;
        seraphControl = SeraphController.GetSeraphController;

        currentHealth = maxHealth;
    }

    void Update()
    {
    }

    public void TakeDamage(ShootableEntity entity, int damage)
    {
        seraphControl.ActivateArmorSeraphs(entity, this.transform.position);

        currentHealth -= damage;
        ui.UpdateHealth(currentHealth, maxHealth);

        Vector2 hitpoint = new Vector2(this.transform.position.x, this.transform.position.y + 0.35f);
        PopUpDamageText T = Instantiate(damageText, hitpoint, Quaternion.identity);
        T.SendMessage("SetTextRun", damage);

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
}
