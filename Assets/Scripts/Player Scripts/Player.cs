using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpriteRenderer sp;
    PlayerStateManager SM;
    UI_Manager ui;

    public PopUpDamageText damageText;


    public int maxHealth;
    public float invulnDuration;
    public float invulnBuffer;
    public float knockedForce;

    private int currentHealth;
    private bool canBeDamaged = true;
    private Vector2 hitPoint;

    private void Awake()
    {
        SM = this.GetComponent<PlayerStateManager>();
    }
    void Start()
    {
        ui = UI_Manager.GetUIManager;

        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
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

    IEnumerator FlashRed()
    {
        sp.color = Color.red;
        canBeDamaged = false;
        yield return new WaitForSeconds(invulnDuration + invulnBuffer);
        canBeDamaged = true;
        sp.color = Color.white;
    }
}
