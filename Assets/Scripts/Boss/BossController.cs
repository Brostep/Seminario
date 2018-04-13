using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public GameObject slowLeftAndRight;
    public GameObject slowBack;
    public GameObject boneThrower;
    public List<GameObject> spawnersPosition;
    public GameObject worm;
    public GameObject Food;
    public GameObject Spit;
    public GameObject Spit2;
    public GameObject Spit3;
    public GameObject vomit2;
    public GameObject SlamArea;
    public GameObject particleTakeDamage;
    public List<GameObject> vomitLeft;
    public List<GameObject> vomitRight;
    public List<GameObject> vomitBack;
    public BulletHellSpawner bulletHellSpawner;
    public List<GameObject> wormsInScene;
    public bool spitDestroy;
    public GameObject thirdPersonBulletPattern;

    public int healthPoints;
    private int _currentHealthPoints;

    ParticleSystem psTakeDamage;

    public Image lifeBar;
    public Image totalLifeBar;
    public Image bossBackBar;

    public int wormsPerSpawner;
    int currentSpawners;
    int faseCounter;
    public int spawnersAlive;
    List<GameObject> spawners;
    Animator anim;
    bool canTakeDamage;
    public bool endPattern;
    public bool fase2Enabled;
    int damage = 1;

    private void Start()
    {
        _currentHealthPoints = healthPoints;
        anim = GetComponent<Animator>();
        //psTakeDamage = GetComponentInChildren<ParticleSystem>();
        spawners = new List<GameObject>();
        wormsInScene = new List<GameObject>();
        lifeBar.enabled = false;
        totalLifeBar.enabled = false;
        bossBackBar.enabled = false;
    }

    private void Update()
    {
        if (canTakeDamage)
        {
            CheckHealthPoints();
        }

        if (fase2Enabled)
        {
            if (currentSpawners > spawnersAlive)
            {
                currentSpawners = spawnersAlive;
                RageFase2();
            }

            if (spitDestroy)
            {
                spitDestroy = false;
                anim.SetBool("EndFase", true);
                ResetFase();
            }
            if (endPattern)
            {
                EndThirdPersonBulletPattern();
                endPattern = false;
            }
        }
    }

    private void CheckHealthPoints()
    {
        switch (_currentHealthPoints)
        {
            case 160:
                canTakeDamage = false;
                bulletHellSpawner.startShooting = false;
                SimpleRage();
                break;
            case 120:
                bulletHellSpawner.startShooting = false;
                canTakeDamage = false;
                SimpleRage();
                break;
            case 80:
                canTakeDamage = false;
                bulletHellSpawner.startShooting = false;
                boneThrower.SetActive(false);
                SimpleRage();
                break;
            case 0:
                anim.SetBool("Death", true);
                break;
        }
    }

    private void SimpleRage()
    {
        anim.SetBool("SimpleRage", true);
    }

    private void EndSimpleRage()
    {
        switch (_currentHealthPoints)
        {
            case 160:
                anim.SetBool("SimpleRage", false);
                EnableVomit();
                ChangeBulletHellPatternTo(2);
                _currentHealthPoints--;
                canTakeDamage = true;
                break;
            case 120:
                anim.SetBool("SimpleRage", false);
                EnableBoneThrower();
                ChangeBulletHellPatternTo(3);
                _currentHealthPoints--;
                canTakeDamage = true;
                break;
            case 80:
                EnableFase2();
                break;
        }
    }

    private void EnableVomit()
    {
        slowLeftAndRight.SetActive(true);
        StartCoroutine(VomitingLeft());
        StartCoroutine(VomitingRight());
    }

    private void ChangeBulletHellPatternTo(int p)
    {
        bulletHellSpawner.startShooting = true;
        bulletHellSpawner.pattern = p;
    }

    private void EnableFase2()
    {
        bulletHellSpawner.startShooting = false;
        anim.SetBool("Slam2Hands", true);
    }

    public void SpawnWorm()
    {
        var i = Random.Range(0, spawners.Count - 1);
        var randPosX = Random.Range(spawners[i].transform.position.x - 3, spawners[i].transform.position.x + 3);
        var randPosZ = Random.Range(spawners[i].transform.position.z - 3, spawners[i].transform.position.z + 3);
        var randPos = new Vector3(randPosX, 12.5f, randPosZ);

        var go = Instantiate(worm, randPos, worm.transform.rotation);
        wormsInScene.Add(go);
    }

    private void SpawnWorms()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            for (int j = 0; j < wormsPerSpawner; j++)
            {
                if (spawners[i].gameObject != null)
                {
                    var randPosX = Random.Range(spawners[i].transform.position.x - 3, spawners[i].transform.position.x + 3);
                    var randPosZ = Random.Range(spawners[i].transform.position.z - 3, spawners[i].transform.position.z + 3);
                    var randPos = new Vector3(randPosX, 12.5f, randPosZ);

                    var go = Instantiate(worm, randPos, worm.transform.rotation);
                    wormsInScene.Add(go);
                }
            }
        }

    }

    public void BossIntro()
    {
        anim.SetBool("EatingIntro", true);
    }

    void EnableBoneThrower()
    {
        boneThrower.SetActive(true);
    }

    void EndSlam2Hands()
    {
        anim.SetBool("Slam2Hands", false);
        anim.SetBool("SimpleRage", false);
        Food.SetActive(true);
    }
    void SpawnSpawners()
    {
        StartCoroutine(CleanVomitingLeft());
        StartCoroutine(CleanVomitingRight());
        slowLeftAndRight.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            spawnersPosition[i].SetActive(true);
            spawners.Add(spawnersPosition[i]);
            currentSpawners++;
            spawnersAlive++;
        }
        SpawnWorms();

        var playerController = FindObjectOfType<PlayerController>();
        playerController.cameraChange = true;
        PlayerController.inTopDown = !PlayerController.inTopDown;
        if (playerController.promedyTarget && !PlayerController.inTopDown)
        {
            playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = false;
        }
        else if (playerController.promedyTarget)
        {
            playerController.topDownCamera.GetComponent<TopDownPromedyTargets>().enabled = true;
        }

        fase2Enabled = true;
    }

    void RageFase2()
    {
        anim.SetBool("RageFase2", true);
        foreach (var worm in wormsInScene)
        {
            worm.GetComponent<Worm>().life = 0;
        }
        wormsInScene.Clear();
    }

    IEnumerator VomitingLeft()
    {
        for (int i = 0; i < vomitLeft.Count; i++)
        {
            vomitLeft[i].SetActive(true);
            yield return new WaitForSeconds(0.07f);
        }
    }
    IEnumerator VomitingRight()
    {
        for (int i = 0; i < vomitRight.Count; i++)
        {
            vomitRight[i].SetActive(true);
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator VomitingBack()
    {
        for (int i = 0; i < vomitBack.Count; i++)
        {
            vomitBack[i].SetActive(true);
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator CleanVomitingLeft()
    {
        for (int i = vomitLeft.Count - 1; i >= 0; i--)
        {
            vomitLeft[i].SetActive(false);
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator CleanVomitingRight()
    {
        for (int i = vomitRight.Count - 1; i >= 0; i--)
        {
            vomitRight[i].SetActive(false);
            yield return new WaitForSeconds(0.07f);
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 9 && canTakeDamage)
        {
            _currentHealthPoints -= damage;
            lifeBar.fillAmount -= damage / 200.0f;

            var particleObj = ParticleManager.Instance.GetParticle(ParticleManager.BLOOD_GULA_HIT_EFFECT);
            particleObj.transform.position = c.transform.position;
            
            //psTakeDamage.Play();
        }
    }

    private void StartShooting()
    {
        ChangeBulletHellPatternTo(1);
        bulletHellSpawner.startShooting = true;
        canTakeDamage = true;
        lifeBar.enabled = true;
        totalLifeBar.enabled = true;
        bossBackBar.enabled = true;
    }
    void SetRageFase2()
    {
        if (faseCounter == 0)
        {
            anim.SetBool("EndFase", false);
            faseCounter++;
            anim.SetBool("RangeAttack", true);
        }
        else if (faseCounter == 1)
        {
            anim.SetBool("EndFase", false);
            faseCounter++;
            anim.SetBool("RangeAttack", true);
            thirdPersonBulletPattern.SetActive(true);
        }
        else if (faseCounter == 2)
        {
            anim.SetBool("EndFase", false);
            faseCounter++;
            anim.SetBool("RangeAttack", true);
            thirdPersonBulletPattern.SetActive(true);
            StartCoroutine(VomitingRight());
            StartCoroutine(VomitingLeft());
            StartCoroutine(VomitingBack());
            slowLeftAndRight.SetActive(true);
            slowBack.SetActive(true);
        }
    }

    private void TurnOnSpit()
    {
        if (faseCounter == 1)
            Spit.SetActive(true);
        else if (faseCounter == 2)
            Spit2.SetActive(true);
        else if (faseCounter == 3)
            Spit3.SetActive(true);
    }

    private void SpitCanMove()
    {
        if (faseCounter == 1)
        {
            Spit.GetComponent<Spit>().canMove = true;
            Spit.gameObject.transform.parent = null;

        }
        else if (faseCounter == 2)
        {
            Spit2.GetComponent<Spit>().canMove = true;
            Spit2.gameObject.transform.parent = null;

        }
        else if (faseCounter == 3)
        {
            Spit3.GetComponent<Spit>().canMove = true;
            Spit3.gameObject.transform.parent = null;
        }

    }

    private void ResetFase()
    {
        anim.SetBool("RangeAttack", false);
        anim.SetBool("RageFase2", false);
        SpawnWorms();
    }

    private void EndThirdPersonBulletPattern()
    {
        anim.SetBool("RangeAttack", false);
        anim.SetBool("RageFase2", false);
        anim.SetBool("EndRage", false);
        thirdPersonBulletPattern.SetActive(false);

        if (faseCounter < 3)
        {
            SpawnWorms();
        }
        else if (faseCounter == 3)
        {
            canTakeDamage = true;
            anim.SetBool("Stunned", true);
            SlamArea.SetActive(false);
            damage = 5;
        }
    }

    void endSlam()
    {
        if (GetComponentInChildren<CatchSlam>().alreadySet)
        {
            var catchSlam = GetComponentInChildren<CatchSlam>();
            catchSlam.pj.transform.parent = null;
            catchSlam.pj.transform.position = catchSlam.palmPos;
            catchSlam.pj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            catchSlam.alreadySet = false;
            catchSlam.pj.GetComponent<Rigidbody>().AddForce(new Vector3(250, 125, 250), ForceMode.Impulse);
        }
    }

    private void WinScreen()
    {
        SceneManager.LoadScene("YouWin");
    }
}
