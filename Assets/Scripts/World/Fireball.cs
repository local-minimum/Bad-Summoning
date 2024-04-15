using UnityEngine;


public delegate void HitPlayerEvent(Fireball ball);

public class Fireball : MonoBehaviour
{
    public static event HitPlayerEvent OnHitPlayer;

    [SerializeField]
    PlayerFacingBillboard billboard;
    
    [SerializeField]
    AudioClip[] Sounds;

    [SerializeField, Range(0, 2)]
    float textSwapFrequency = 0.4f;
    
    [SerializeField]
    Texture[] BallTextures;

    [SerializeField, Range(0,5)]
    float soundPauseMin = 0.5f;

    [SerializeField, Range(0,5)]
    float soundPauseMax = 2f;

    AudioSource _speaker;
    protected AudioSource Speaker
    {
        get
        {
            if (_speaker == null)
            {
                _speaker = GetComponent<AudioSource>();
            }
            return _speaker;
        }
    }
    public Vector3 Speed { get; set; }
    public int BallDamage { get; set; } 

    private void OnEnable()
    {
        nextSound = Random.Range(soundPauseMin, soundPauseMax) + Time.timeSinceLevelLoad;
        nextBallTex = Time.timeSinceLevelLoad + textSwapFrequency;
        ballTexIdx = 0;
    }

    public void CollideWithPlayer()
    {
        Recycle();
        OnHitPlayer?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            CollideWithPlayer();
        } else
        {
            Recycle();
        } 
    }

    private void Recycle()
    {
        gameObject.SetActive(false);
    }

    float nextSound;
    int ballTexIdx;
    float nextBallTex;

    private void Update()
    {
        transform.position += Speed * Time.deltaTime;
        billboard.Sync();

        if (Time.timeSinceLevelLoad > nextSound)
        {
            nextSound = Random.Range(soundPauseMin, soundPauseMax) + Time.timeSinceLevelLoad;
            Speaker.PlayOneShot(Sounds.GetRandomElement());
        }

        if (Time.timeSinceLevelLoad > nextBallTex)
        {
            ballTexIdx++;
            GetComponent<Renderer>().material.mainTexture = BallTextures.GetWrappingNth(ballTexIdx);
            nextBallTex = Time.timeSinceLevelLoad + textSwapFrequency;
        }
    }
}
