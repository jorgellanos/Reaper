using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables y Referencias
    // Variables
    public float Health, stamina, damage;
    [SerializeField] private bool hit, strong, interact;
    [SerializeField] private string direccionAtq, direccion, fightSequence;
    private int comboNum;
    private float timeHit;
    private float time;

    // Keycode Array
    [SerializeField] private string[] combo = new string[2];

    // Referencias
    private Animator an;
    private Interact i;
    private Rigidbody2D rb;
    [SerializeField] private Scythe scythe;
    #endregion

    // Use this for initialization
    void Start()
    {
        an = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        scythe.damage = damage;
        comboNum = 0;
        direccion = "Right";
        hit = false;
        time = 2;
        timeHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Anims
        AnimCtrl();

        // Cronometro para COMBOS
        Timer();

        // Interact
        

        // Fight
        ComboFighting();
    }

    #region Triggers
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Moving Ground")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Moving Ground")
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interact")
        {
            interact = true;
            i = collision.gameObject.GetComponent<Interact>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interact")
        {
            interact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interact")
        {
            interact = false;
        }
    }
    #endregion

    public void Hurt(float damage)
    {
        Health -= damage;
        an.Play("Hurt");
        Vector3 dir = transform.right;
        //rb.AddForce(new Vector2(-dir.x * 3f, 1.5f), ForceMode2D.Impulse);
    }

    // Control de animaciones
    public void AnimCtrl()
    {
        an.SetBool("Hitting", hit);
        an.SetInteger("Combo", comboNum);
    }

    public void Timer()
    {
        //Tiempo para el combo
        if (timeHit > 0)
        {
            timeHit -= Time.deltaTime;
        }
        else
        {
            timeHit = 0;
            for (int i = 0; i < combo.Length; i++)
            {
                combo[i] = string.Empty;
                fightSequence = string.Empty;
            }
        }
    }

    public void ComboEnd()
    {
        comboNum = 0;
    }

    private void AddToCombo(string action)
    {
        for (int i = 0; i < combo.Length;)
        {
            if (combo[i] == string.Empty)
            {
                combo[i] = action;
                return;
            }
            else
            {
                i++;
            }
        }
    }

    public void ComboFighting()
    {
        if (Input.GetKeyDown("s"))
        {
            AddToCombo("Hit");
            timeHit = 1f;
            GroundFight();
        }
    }

    public void Guard()
    {
        
    }

    public void GroundFight()
    {
        for (int i = 0; i < combo.Length; i++)
        {
            fightSequence += combo[i];
        }

        switch (fightSequence)
        {
            case "Hit":
                comboNum = 1;
                break;

            case "HitHit":
                comboNum = 2;
                break;
        }
    }

    public void AirFight()
    {
        switch (direccionAtq)
        {
            case "Arriba":
                an.Play("Manuel_AH1");
                break;

            case "Derecha":
                an.Play("Manuel_AH2");
                break;

            case "Izquierda":
                an.Play("Manuel_AH2");
                break;

            case "Abajo":
                an.Play("Manuel_AH3");
                break;

            case "":
                an.Play("Manuel_AH2");
                break;
        }

    }

    // Falta testear
    public void Push(float forceX, float forceY)
    {
        rb.AddForce(new Vector2(forceX, forceY));
    }

    // Direccion de ataque
    #region Estados
    public void Estados()
    {
        if (Input.GetKeyDown("up"))
        {
            direccionAtq = "Arriba";
        }

        if (Input.GetKeyDown("down"))
        {
            direccionAtq = "Abajo";
        }

        if (Input.GetKeyDown("left shift"))
        {
            strong = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            strong = false;
        }

        if (!Input.anyKey)
        {
            direccionAtq = "";
        }
    }
    #endregion
}
