using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            FindObjectOfType<LevelManager>().LoadNextLevel();
        }
    }
}
