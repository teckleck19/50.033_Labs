using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Physics
    public float horizantalSpeed;
    //public float maxSpeed = 10;
    public float upSpeed = 10;
    
    //Components
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Collider2D marioCollider;
    //State
    private bool onGroundState = false;
    private bool faceRightState = true;
    private bool countScoreState = false;
    private int score = 0;
    private int lives = 3;
    private bool gameoverState = false;

    //Other GameObjects
    public Transform enemyLocation;
    public Text scoreText;
    public Text livesText;
    public Text gameOverText;
    public Button replayButton;
    private Vector3 originalPosition;
    public float min_X;
    public float max_X;
    
    
  
    // Start is called before the first frame update
    void Start(){
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    void Update() {
        //Update and not FixedUpdate since this logic has nothing to do with the Physics Engine:
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }  

        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log("Score: "+score);
            }
        }
    }
    void FixedUpdate(){

        

        //dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        /* if (Mathf.Abs(moveHorizontal) > 0){
          Vector2 movement = new Vector2(moveHorizontal, 0);
          if (marioBody.velocity.magnitude < maxSpeed)
                  marioBody.AddForce(movement * speed);
        } */
        if(!(transform.position.x<=min_X || transform.position.x>=max_X)){
          transform.position += new Vector3(moveHorizontal,0,0) * Time.deltaTime * horizantalSpeed;
        }
        if (transform.position.x<=min_X){
          transform.position = new Vector3(min_X+0.1f,transform.position.y,0);
        }
        if (transform.position.x>=max_X){
          transform.position = new Vector3(max_X-0.1f,transform.position.y,0);
        }
        

        if (Input.GetKeyDown("space") && onGroundState){
          marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
          countScoreState = true;
        }

        /* if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
          // stop
          marioBody.velocity = Vector2.zero;
        } */

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) {
          onGroundState = true;
          countScoreState = false; // reset score state
          scoreText.text = "Score: " + score.ToString();
        }
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
          lives--;
          if (lives>0){
            livesText.text = "Lives: " + lives.ToString(); 
            transform.position = originalPosition;
            //Time.timeScale = 0.0f;
          }else{
            livesText.text = "Lives: " + lives.ToString(); 
            marioBody.AddForce(Vector2.up * (upSpeed-10.0f), ForceMode2D.Impulse);
            marioCollider.enabled = false;
            gameoverState = true;
            StartCoroutine(gameOverJump());
            // Debug.Log("GAME OVER");
            // livesText.text = "Lives: " + lives.ToString(); 
            // Time.timeScale = 0.0f;
            // gameOverText.gameObject.SetActive(true);
            // replayButton.gameObject.SetActive(true);
          }
        }
        
    }

    public void ReplayButtonClicked()
    {    
      SceneManager.LoadScene("Mario_1");
    }

    IEnumerator gameOverJump(){
      yield return new WaitForSecondsRealtime(1);
      Debug.Log("GAME OVER");
      Time.timeScale = 0.0f;
      gameOverText.gameObject.SetActive(true);
      replayButton.gameObject.SetActive(true);
    }
}
