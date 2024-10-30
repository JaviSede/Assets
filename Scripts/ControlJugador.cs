using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.SceneManagement;


public class ControlJugador : MonoBehaviour
{
    public int velocidad;
    public int fuerzaSalto;
    public int puntuacion;
    public int numVidas;
    private bool vulnerable;
    private Rigidbody2D fisica;
    private SpriteRenderer sprite;
    private Animator animacion;

    // Start is called before the first frame update
    private void Start()
    {
        vulnerable = true;
        fisica = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animacion = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        float entradaX = Input.GetAxis("Horizontal");
        fisica.velocity = new Vector2(entradaX * velocidad, fisica.velocity.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TocarSuelo())
        {
            fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
        // si va hacia la derecha flipX = false y sino true para dar el volteo
        if (fisica.velocity.x > 0) sprite.flipX = false;
        else if (fisica.velocity.x < 0) sprite.flipX = true;

        // Llamar a la funcion para animar al jugador
        AnimarJugador();
    }

    private void AnimarJugador() {
        if (!TocarSuelo()) animacion.Play("jugadorSaltar");
        else if ((fisica.velocity.x > 1 || fisica.velocity.x < -1) && fisica.velocity.y == 0)
            animacion.Play("jugadorCorrer");
        else if ((fisica.velocity.x < 1 || fisica.velocity.x > -1) && fisica.velocity.y == 0)
            animacion.Play("jugadorParado");
    }

    private bool TocarSuelo()
    {
        RaycastHit2D tocar = Physics2D.Raycast(transform.position + new Vector3(0, -2f, 0), Vector2.down, 0.2f);
        return tocar.collider != null;
    }

    public void QuitarVida()
    {
        if (vulnerable)
        {
            vulnerable = false;
            numVidas--;
            if (numVidas == 0) FinJuego();
            Invoke("HacerVulnerable", 1f);
            sprite.color = Color.red;
        }
    }

    private void HacerVulnerable()
    {
        vulnerable = true;
        sprite.color = Color.white;
    }

    public void FinJuego(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncrementarPuntos(int cantidad)
    {
        puntuacion += cantidad;
    }

}