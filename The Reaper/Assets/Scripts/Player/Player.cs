using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables y Referencias
    // Variables
    [Header("Player Stats")]
    [SerializeField] private float Health;
    [SerializeField] private float stamina;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [Space]
    [Header("Player State")]
    [SerializeField] private bool hit;
    [SerializeField] private float strong;
    [SerializeField] private bool interact;
    [SerializeField] private string direccionAtq;
    [SerializeField] private string direccion;
    [SerializeField] private string fightSequence;
    [SerializeField] private int comboNum;
    [SerializeField] private float timeHit;
    [SerializeField] private float timePerCombo;

    [Header("Combo Chain")]
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
        timeHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Anims
        AnimCtrl();
        // Direccion de ataque
        Estados();
        // Fight
        ComboFighting();
        // Cronometro para COMBOS
        ComboTimer();
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

    public void PlayerDamageRecieved(float damage)
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

    public void ComboTimer()
    {
        //Tiempo para el combo
        if (timeHit > 0)
        {
            timeHit -= Time.deltaTime;
        }
        else
        {
            timeHit = 0;
            comboNum = 0;
            hit = false;
            for (int i = 0; i < combo.Length; i++)
            {
                combo[i] = string.Empty;
                fightSequence = string.Empty;
            }
            hit = false;
        }
    }

    private void AddToCombo(string action)
    {
        for (int i = 0; i < combo.Length;)
        {
            if (combo[i] == string.Empty)
            {
                fightSequence += action;
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
            hit = true;
            AddToCombo("Hit");
            timeHit = timePerCombo;
            GroundFight();
        }
    }

    public void Guard()
    {
        
    }

    public void GroundFight()
    {
        switch (fightSequence)
        {
            case "Hit":
                comboNum = 1;
                break;

            case "HitHit":
                comboNum = 2;
                break;

            case "UpUpHit":
                Debug.Log("UpperCUT");
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
            AddToCombo("Up");
        }

        if (Input.GetKeyDown("down"))
        {
            AddToCombo("Down");
        }

        if (Input.GetKeyDown("left shift"))
        {
            //AddToCombo("Up");
        }
        else if (Input.GetKeyUp("left shift"))
        {
           //AddToCombo("Up");
        }

        if (!Input.anyKey)
        {
            direccionAtq = "";
        }
    }
    #endregion
}
