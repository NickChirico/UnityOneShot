using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    public LineRenderer AimLine;
    public PopUpDamageText upgradeText;

    public Image redBar;
    public Image yellowBar;
    public Image blueBar;
    public Image greenBar;
    float redFill, yellowFill, blueFill, greenFill;
    public float fillRate;
    public enum Energized { None, Red, Yellow, Green, Blue };
    public Energized currentPower = Energized.None;

    public float movespeed;
    public float range;
    public float reloadTime;

    Vector2 rayOrigin;
    Vector2 upgradeTextSpot;
    bool canShoot = true;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        redBar.color = Color.red;
        blueBar.color = Color.blue;
        yellowBar.color = Color.yellow;
        greenBar.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        rayOrigin = this.transform.position;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1)); // invert cam Z to make 0
        direction = (targetPos - this.transform.position).normalized;

        UpdateAimLine();

        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot(direction);
        }

        upgradeTextSpot = new Vector2(this.transform.position.x-0.5f, this.transform.position.y + 0.75f);

        CheckPower();
        UpdateFillUI();
    }

    private void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal") * movespeed;
        float moveV = Input.GetAxis("Vertical") * movespeed;

        rb.velocity = new Vector2(moveH, moveV);
        
        //direction = new Vector2(moveH, moveV);

        //  SET ANIMATION
        //FindObjectOfType<PlayerAnimation>().SetDirection(direction);

        //  Movement INDICATOR ?
        //Vector2 indicatiorLoc = new Vector2(this.transform.position.x + (moveH * 0.15f), this.transform.position.y + (moveV * 0.15f));
        //destinationIndicator.transform.position = indicatiorLoc;
    }

    private void UpdateAimLine()
    {
        AimLine.SetPosition(0, rayOrigin);
        AimLine.SetPosition(1, rayOrigin + (direction * range));
    }
    public void ToggleAimLineColor(bool isReloading)
    {
        if (isReloading)
        {
            AimLine.startColor = Color.red;
            AimLine.endColor = Color.red;
        }
        else
        {
            AimLine.startColor = Color.yellow;
            AimLine.endColor = Color.yellow;
        }
    }


    private void Shoot(Vector2 dir)
    {
        if (canShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, range);
            ToggleAimLineColor(true);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Hit Enemy
                    ProtoEnemy1 enemy = hit.collider.GetComponent<ProtoEnemy1>();
                    if (enemy != null)
                    {
                        enemy.Die();
                    }
                }
            }

                canShoot = false;
            StartCoroutine(Reload());


        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        if (!canShoot)
        {
            canShoot = true;
            ToggleAimLineColor(false);
        }
    }
    public void Augment(WeaponOrb.OrbType orbType)
    {
        PopUpDamageText T = Instantiate(upgradeText, upgradeTextSpot, Quaternion.identity);
        switch (orbType)
        {
            case WeaponOrb.OrbType.red:
                T.SendMessage("SetTextRun", "Damage Up!");
                // damage up
                redFill += fillRate;
                break;
            case WeaponOrb.OrbType.yellow:
                T.SendMessage("SetTextRun", "FireRate Up!");
                reloadTime -= 0.1f;
                yellowFill += fillRate;
                break;
            case WeaponOrb.OrbType.green:
                T.SendMessage("SetTextRun", "Size Up!");
                this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                greenFill += fillRate;
                break;
            case WeaponOrb.OrbType.blue:
                T.SendMessage("SetTextRun", "Speed Up!");
                movespeed += 0.35f;
                blueFill += fillRate;
                break;
            default:
                break;
        }

    }

    private void CheckPower()
    {
        if (redFill >= 1)
        {
            currentPower = Energized.Red;
            StartCoroutine(Deplete());
        }
        if (yellowFill >= 1)
        {
            currentPower = Energized.Yellow;
            StartCoroutine(Deplete());
        }
        if (greenFill >= 1)
        {
            currentPower = Energized.Green;
            StartCoroutine(Deplete());
        }
        if (blueFill >= 1)
        {
            currentPower = Energized.Blue;
            StartCoroutine(Deplete());
        }
    }
    private void UpdateFillUI()
    { 
        redBar.fillAmount = Mathf.Lerp(redBar.fillAmount, redFill, Time.deltaTime * 4);
        yellowBar.fillAmount = Mathf.Lerp(yellowBar.fillAmount, yellowFill, Time.deltaTime * 4);
        greenBar.fillAmount = Mathf.Lerp(greenBar.fillAmount, greenFill, Time.deltaTime * 4);
        blueBar.fillAmount = Mathf.Lerp(blueBar.fillAmount, blueFill, Time.deltaTime * 4);

        switch (currentPower)
        {
            case Energized.None:
                redBar.color = Color.red;
                blueBar.color = Color.blue;
                yellowBar.color = Color.yellow;
                greenBar.color = Color.green;
                break;
            case Energized.Red:
                redBar.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 0.5f));
                break;
            case Energized.Yellow:
                yellowBar.color = Color.Lerp(Color.yellow, Color.white, Mathf.PingPong(Time.time, 0.5f));
                break;
            case Energized.Blue:
                blueBar.color = Color.Lerp(Color.blue, Color.white, Mathf.PingPong(Time.time, 0.5f));
                break;
            case Energized.Green:
                greenBar.color = Color.Lerp(Color.green, Color.white, Mathf.PingPong(Time.time, 0.5f));
                break;
        }
    }

    public float depleteDuration;
    float startVal = 1, endVal = 0;
    float valToLerp;
    private IEnumerator Deplete()
    {
        float timer = 0;

        while (timer < depleteDuration)
        {
            switch (currentPower)
            {
                case Energized.None:
                    break;
                case Energized.Red:
                    redFill = Mathf.Lerp(startVal, endVal, timer / depleteDuration);
                    break;
                case Energized.Yellow:
                    yellowFill = Mathf.Lerp(startVal, endVal, timer / depleteDuration);
                    break;
                case Energized.Green:
                    greenFill = Mathf.Lerp(startVal, endVal, timer / depleteDuration);
                    break;
                case Energized.Blue:
                    blueFill = Mathf.Lerp(startVal, endVal, timer / depleteDuration);
                    break;
            }
            timer += Time.deltaTime;

            yield return null;
        }

        valToLerp = endVal;
        currentPower = Energized.None;
    }

}
