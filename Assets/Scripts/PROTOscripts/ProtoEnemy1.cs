using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoEnemy1 : MonoBehaviour
{
    ProtoPlayer player;
    SpriteRenderer sp;
    public SpriteRenderer exclamationPoint;
    public WeaponOrb Orb;

    public enum EnemyType { Grow, Speed, Damage, Fire }
    public EnemyType MyType;

    public float detectRadius;
    public float patrolMoveSpeed;
    public float attackMoveSpeed;

    public float patrolTimer = 3f;
    public float patrolTimerWait = 2f;
    float t, t1;
    bool dying = false;
    private void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        player = FindObjectOfType<ProtoPlayer>();
        exclamationPoint.enabled = false;

        int num = Random.Range(1, 5);

        switch (num)
        {
            case 1:
                MyType = EnemyType.Damage;
                sp.color = Color.red;
                break;
            case 2:
                MyType = EnemyType.Fire;
                sp.color = Color.yellow;
                break;
            case 3:
                MyType = EnemyType.Grow;
                sp.color = Color.green;
                break;
            case 4:
                MyType = EnemyType.Speed;
                sp.color = Color.blue;
                break;
            default:
                break;
        }

        patrolTimer = Random.Range(patrolTimer, patrolTimer + 0.5f);
        patrolTimerWait = Random.Range(patrolTimerWait, patrolTimer + 0.25f);
    }

    private void Update()
    {
        if (dying)
        {
            sp.color = Color.Lerp(sp.color, Color.black, Time.deltaTime*2);
            return;
        }

        if ((this.transform.position - player.transform.position).magnitude < detectRadius)
        {
            // ATTACK
            exclamationPoint.enabled = true;

            /*switch (MyType)
            {
                case EnemyType.Damage:
                    break;
                case EnemyType.Fire:
                    break;
                case EnemyType.Grow:
                    break;
                case EnemyType.Speed:
                    transform.LookAt(player.transform.position);
                    transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation
                    transform.Translate(new Vector3(attackMoveSpeed * Time.deltaTime, 0, 0));
                    break;
                default:
                    break;
            }*/
            transform.LookAt(player.transform.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);//correcting the original rotation
            transform.Translate(new Vector3(attackMoveSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            // PATROL
            exclamationPoint.enabled = false;

            if (t < patrolTimer)
            {
                t += Time.deltaTime;
                transform.Translate(new Vector3(patrolMoveSpeed * Time.deltaTime, 0, 0));
            }
            else if (t1 < patrolTimerWait)
            {
                t1 += Time.deltaTime;
            }
            else
            {
                t1 = 0;
                t = 0;
                transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)), Space.Self);//correcting the original 

                /*switch (MyType)
                {
                    case EnemyType.Damage:
                        break;
                    case EnemyType.Fire:
                        break;
                    case EnemyType.Grow:
                        break;
                    case EnemyType.Speed:
                        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0), Space.Self);//correcting the original 
                        break;
                    default:
                        break;
                }*/
            }
        }
    }

    public EnemyType GetEnemy()
    {
        return MyType;
    }

    public void Die()
    {
        dying = true;
        exclamationPoint.enabled = false;

        StartCoroutine(DieCo());
    }

    IEnumerator DieCo()
    {
        yield return new WaitForSeconds(0.5f);

        // DROP ORB
        WeaponOrb O = Instantiate(Orb, this.transform.position, Quaternion.identity);
        O.SetType(MyType);

        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }

}
