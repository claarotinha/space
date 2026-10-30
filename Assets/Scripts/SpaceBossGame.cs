using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class SpaceBossGame : MonoBehaviour
{
    public RectTransform field, player, boss;
    public Image playerImage, bossImage, bossHealthFill;
    public Text livesText, statusText;
    public GameObject resultPanel;
    public Text resultText;
    public Animator bossAnimator;
    public RectTransform projectilePrefab;
    public float playerSpeed = 380f;
    public float shotInterval = .35f;
    public int bossMaxHealth = 240;
    public int bossHP, hits;
    public bool ended, transitioning;
    public float invincibleUntil, playerInvincibleUntil;
    private float nextShot, bossNextShot, nextPattern;
    private int phase = 0, behavior = 0;
    private ObjectPool<RectTransform> playerPool, enemyPool;
    private readonly List<Shot> active = new List<Shot>();
    private struct Shot { public RectTransform rect; public Vector2 velocity; public bool hostile; }
    private Vector2 playerStart, bossStart;
    private static readonly Color[] phaseColors = { new Color(.3f,.9f,.4f), new Color(1f,.7f,.15f), new Color(1f,.25f,.3f) };
    void Start()
    {
        playerStart = player.anchoredPosition; bossStart = boss.anchoredPosition;
        playerPool = MakePool(new Color(.3f,1f,1f));
        enemyPool = MakePool(new Color(1f,.3f,.4f));
        Restart();
    }
    ObjectPool<RectTransform> MakePool(Color color)
    {
        return new ObjectPool<RectTransform>(() => {
            var r = Instantiate(projectilePrefab, field);
            r.GetComponent<Image>().color = color;
            r.gameObject.SetActive(false);
            return r;
        }, r => r.gameObject.SetActive(true), r => r.gameObject.SetActive(false),
        r => Destroy(r.gameObject), true, 24, 200);
    }
    bool Pressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKey(KeyCode.Space);
#else
        return false;
#endif
    }
    Vector2 MoveInput()
    {
        float x = 0, y = 0;
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        if(k != null) {
            x = (k.dKey.isPressed || k.rightArrowKey.isPressed ? 1 : 0) - (k.aKey.isPressed || k.leftArrowKey.isPressed ? 1 : 0);
            y = (k.wKey.isPressed || k.upArrowKey.isPressed ? 1 : 0) - (k.sKey.isPressed || k.downArrowKey.isPressed ? 1 : 0);
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        x = Input.GetAxisRaw("Horizontal"); y = Input.GetAxisRaw("Vertical");
#endif
        return new Vector2(x,y).normalized;
    }
    void Update()
    {
        if (ended) return;
        float dt = Time.deltaTime;
        player.anchoredPosition += MoveInput() * playerSpeed * dt;
        player.anchoredPosition = new Vector2(Mathf.Clamp(player.anchoredPosition.x,-420,420), Mathf.Clamp(player.anchoredPosition.y,-275,180));
        if (Pressed() && Time.time >= nextShot) { nextShot = Time.time + shotInterval; Spawn(player.anchoredPosition + Vector2.up*32, Vector2.up*570, false); }
        if (transitioning && Time.time >= invincibleUntil) {
            transitioning = false;
            bossImage.color = phaseColors[phase];
            SetBehavior(phase*3);
        }
        for(int i=active.Count-1;i>=0;i--) {
            var s=active[i]; s.rect.anchoredPosition += s.velocity*dt;
            var p=s.rect.anchoredPosition;
            bool outside = Mathf.Abs(p.x)>510 || Mathf.Abs(p.y)>350;
            if(!outside && !s.hostile && !transitioning && Vector2.Distance(p,boss.anchoredPosition)<42) {
                outside=true; DamageBoss();
            }
            if(!outside && s.hostile && Time.time >= playerInvincibleUntil && Vector2.Distance(p,player.anchoredPosition)<25) {
                outside=true; DamagePlayer();
            }
            if(outside) { (s.hostile ? enemyPool : playerPool).Release(s.rect); active.RemoveAt(i); }
        }
        if(!transitioning) bossImage.color = phaseColors[phase];
        else bossImage.color = (Mathf.FloorToInt(Time.time*7)%2==0) ? Color.white : new Color(.4f,.4f,.4f);
        playerImage.color = Time.time < playerInvincibleUntil && Mathf.FloorToInt(Time.time*9)%2==0 ? Color.gray : new Color(.25f,.75f,1f);
    }
    void Spawn(Vector2 position, Vector2 velocity, bool hostile)
    {
        var r=(hostile ? enemyPool : playerPool).Get();
        r.anchoredPosition=position;
        active.Add(new Shot {rect=r,velocity=velocity,hostile=hostile});
    }
    void DamageBoss()
    {
        if(ended || transitioning) return;
        bossHP = Mathf.Max(0,bossHP-1);
        bossHealthFill.fillAmount = (float)bossHP/bossMaxHealth;
        if(bossHP==0) { Finish(true); return; }
        int target = bossHP*100 > bossMaxHealth*50 ? 0 : bossHP*100 > bossMaxHealth*20 ? 1 : 2;
        if(target != phase) {
            phase=target; transitioning=true; invincibleUntil=Time.time+2.25f;
            bossAnimator.SetInteger("Behavior",9);
            bossHealthFill.color=phaseColors[phase];
        }
    }
    void DamagePlayer()
    {
        if(ended) return;
        hits++; livesText.text="VIDAS: "+Mathf.Max(0,4-hits);
        playerInvincibleUntil=Time.time+1.1f;
        if(hits>=4) Finish(false);
    }
    void Finish(bool won)
    {
        ended=true; resultPanel.SetActive(true);
        resultText.text=won ? "VITORIA!" : "DERROTA!";
    }
    void SetBehavior(int index)
    {
        behavior=index; bossAnimator.SetInteger("Behavior",index);
        bossNextShot=Time.time+.5f; nextPattern=Time.time+5f;
    }
    public void BossStateUpdate(int index)
    {
        if(ended || transitioning || index!=behavior) return;
        int local=index%3;
        float t=Time.time;
        float speed = phase==0 ? 90 : phase==1 ? 140 : 190;
        float x=local==0 ? Mathf.Sin(t*.9f)*240 : local==1 ? Mathf.Sin(t*1.6f)*290 : Mathf.Sin(t*2.2f)*330;
        boss.anchoredPosition = new Vector2(Mathf.MoveTowards(boss.anchoredPosition.x,x,speed*Time.deltaTime), 230);
        if(t>=bossNextShot) {
            float interval = phase==0 ? .85f : phase==1 ? .57f : .38f;
            bossNextShot=t+interval;
            Vector2 origin=boss.anchoredPosition+Vector2.down*35;
            float shotSpeed=phase==0 ? 170 : phase==1 ? 225 : 275;
            if(local==0) {
                Vector2 direction=(player.anchoredPosition-origin).normalized;
                Spawn(origin,direction*shotSpeed,true);
                if(phase==2) { Spawn(origin,Rotate(direction,-16)*shotSpeed,true); Spawn(origin,Rotate(direction,16)*shotSpeed,true); }
            } else if(local==1) {
                int count=3+phase*2;
                for(int j=0;j<count;j++) {
                    float a=-55+j*110f/(count-1);
                    Spawn(origin,Rotate(Vector2.down,a)*shotSpeed,true);
                }
            } else {
                int count=4+phase*2;
                for(int j=0;j<count;j++) {
                    float a=(j-(count-1)*.5f)*14f + Mathf.Sin(t*2f)*22;
                    Spawn(origin,Rotate(Vector2.down,a)*shotSpeed,true);
                }
            }
        }
        if(t>=nextPattern) SetBehavior(phase*3+(local+1)%3);
    }
    static Vector2 Rotate(Vector2 v,float angle) {
        float a=angle*Mathf.Deg2Rad;
        return new Vector2(v.x*Mathf.Cos(a)-v.y*Mathf.Sin(a),v.x*Mathf.Sin(a)+v.y*Mathf.Cos(a));
    }
    public void Restart()
    {
        for(int i=active.Count-1;i>=0;i--) {
            var s=active[i]; (s.hostile ? enemyPool : playerPool).Release(s.rect);
        }
        active.Clear();
        bossHP=bossMaxHealth; hits=0; phase=0; ended=false; transitioning=false;
        invincibleUntil=0; playerInvincibleUntil=0;
        boss.anchoredPosition=bossStart; player.anchoredPosition=playerStart;
        bossHealthFill.fillAmount=1; bossHealthFill.color=phaseColors[0];
        livesText.text="VIDAS: 4"; statusText.text="WASD / SETAS - MOVER   |   ESPACO - ATIRAR";
        resultPanel.SetActive(false); nextShot=Time.time;
        SetBehavior(0);
    }
}
