using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour
{
    

    
    
    public int enemyNumber1;
    public int enemyNumber2;
    

    public string nomeDaCena;
    
    

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("colidiu");
            TipoInimigo.TI.tipoInimigo1 = enemyNumber1;
            TipoInimigo.TI.tipoInimigo2 = enemyNumber2;
            
            SceneManager.LoadScene(nomeDaCena);
            

        }
    }
}