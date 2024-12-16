using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject thePlayer;                   
    public GameObject cutsceneCam;                
    public GameObject enemy;                       
    public Transform walkTarget;                   
    public float cutsceneDuration = 10f;          
    public float walkDuration = 3f;                
    public AudioSource ambientSound;               
    public AudioSource[] cutsceneSounds;           
    public float[] cutsceneSoundDelays;            

    private bool cutscenePlayed = false;           
    private Animator enemyAnimator;                

    void Start()
    {
        
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }
    }
    void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player") && !cutscenePlayed)
        {
            StartCoroutine(PlayCutscene());
        }
    }

    
    private IEnumerator PlayCutscene()
    {
        cutscenePlayed = true;
        if (ambientSound != null)
        {
            ambientSound.Pause();
        }
        cutsceneCam.SetActive(true);
        thePlayer.SetActive(false);

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsWalking", true);
        }

        StartCoroutine(PlayCutsceneSounds());
        yield return StartCoroutine(MoveEnemyToTarget());

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsWalking", false);
        }

        
        yield return new WaitForSeconds(cutsceneDuration - walkDuration);

        cutsceneCam.SetActive(false);
        thePlayer.SetActive(true);

        if (ambientSound != null)
        {
            ambientSound.UnPause();
        }

        GetComponent<Collider>().enabled = false;
    }

    
    private IEnumerator PlayCutsceneSounds()
    {
        for (int i = 0; i < cutsceneSounds.Length; i++)
        {
            if (cutsceneSounds[i] != null)
            {
                yield return new WaitForSeconds(cutsceneSoundDelays[i]);
                cutsceneSounds[i].Play();
            }
        }
    }

    private IEnumerator MoveEnemyToTarget()
    {
        Vector3 startPosition = enemy.transform.position;
        float elapsed = 0f;

        while (elapsed < walkDuration)
        { 
            enemy.transform.position = Vector3.Lerp(startPosition, walkTarget.position, elapsed / walkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = walkTarget.position;
    }
}







