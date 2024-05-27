using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//플레이어 죽음, 부활, 카메라 비율 조절
public class M_GameManager : MonoBehaviour
{
    //플레이어 히트
    public Image p1HpImage;
    public Image p2HpImage;


    //플레이어 부활
    public Transform respawnPosition;
    public GameObject p1DiePanel;
    public GameObject p2DiePanel;
    public GameObject p1PlayCanvas;
    public GameObject p2PlayCanvas;
    public Image p1DamageEffect;
    public Image p2DamageEffect;
    public Image p1HealImage;
    public Image p2HealImage;
    public int dieCount = 0;
    float p1HpCount = 0;
    float p2HpCount = 0;

    bool isBossDie = false;

    //FadeInOut
    public Image fadeInOutPanel;

    public Image endImage;

    public static M_GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        endImage.enabled = false;
        p1DamageEffect.enabled = false;
        p2DamageEffect.enabled = false;
    }

    void Update()
    {
        if (isBossDie)
            return;
        if (Input.GetKey(KeyCode.I))
        {
            if (Input.GetKeyDown(KeyCode.O))
                IeNextScene();
        }
            
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (D_Boss.instance)
            {
                if (D_Boss.instance.hipHP > 0)
                    D_Boss.instance.hipHP = 0;
                else if (D_Boss.instance.bellyHP > 0)
                    D_Boss.instance.bellyHP = 0;
                else
                    D_Boss.instance.chainHP = 0;
            }
        }
            
            DieCheck();
    }

    void DieCheck()
    {
        if (D_Boss.instance)
        {
            if (D_Boss.instance.bossHP <= 0)
            {
                endImage.enabled = true;
                isBossDie = true;
                StartCoroutine(IeEndScene());
            }
        }
        if (dieCount >= 2)
        {
            GameOver();
        }
        if (p1DiePanel.activeSelf == true)
        {
            Player1Heal();
        }
        else if (p2DiePanel.activeSelf == true)
        {
            Player2Heal();
        }
        if (p1HpImage && p2HpImage)
        {
            p1HpImage.fillAmount = M_Player.instance.hp / 10f;
            p2HpImage.fillAmount = M_Player2.instance.hp / 10f;
        }
    }

    public void Player1Die()
    {
        
        dieCount++;
        if (dieCount == 2)
            return;
        p1DiePanel.SetActive(true);
        p1HealImage.fillAmount = 0;
        p1HpCount = 0;

        Camera camera;
        //P2 카메라, 캔버스 비율 늘리기
        camera = M_Player2.instance.playerCamera;
        camera.rect = new Rect(0.3f, 0, 0.7f, 1f);
        p2PlayCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 1, 1);
        p2DamageEffect.enabled = true;
        //P1 카메라, 캔버스 비율 줄이기
        camera = M_Player.instance.playerCamera;
        camera.rect = new Rect(0, 0, 0.3f, 1f);
        p1DiePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1152f, 1080);
        p1PlayCanvas.SetActive(false);
        M_Player2.instance.cameraCenter = 0.65f;
    }

    public void Player2Die()
    {
        dieCount++;
        if (dieCount == 2)
            return;
        p2DiePanel.SetActive(true);
        p2HealImage.fillAmount = 0;
        p2HpCount = 0;

        Camera camera;
        camera = M_Player.instance.playerCamera;
        camera.rect = new Rect(0, 0, 0.7f, 1f);
        p1PlayCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 1, 1);
        camera = M_Player2.instance.playerCamera;
        p1DamageEffect.enabled = true;
        p1HpImage.transform.localScale = new Vector3(1.4f, 1, 1);
        camera.rect = new Rect(0.7f, 0, 0.3f, 1f);
        p2DiePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(576f, 1080);
        p2PlayCanvas.SetActive(false);
        M_Player.instance.cameraCenter = 0.35f;
    }
    void Player1Heal()
    {
        p1HpCount += 0.03f;
        p1HealImage.fillAmount = p1HpCount / 10f;
        if (Input.GetButtonDown("JoyTriangle"))
        {
            p1HpCount++;
        }
        if (p1HpCount >= 10)
        {
            P1Respawn();
        }
    }

    void Player2Heal()
    {
        //카메라 비율 조정
        Camera camera;
        camera = M_Player.instance.playerCamera;
        camera.rect = new Rect(0, 0, 0.7f, 1f);
        camera = M_Player2.instance.playerCamera;
        camera.rect = new Rect(0.7f, 0, 0.3f, 1f);
        p2DiePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(576f, 1080);

        p2HpCount += 0.03f;
        p2HealImage.fillAmount = p2HpCount / 10f;
        if (Input.GetKeyDown(KeyCode.E))
        {
            p2HpCount++;
        }
        if (p2HpCount >= 10)
        {
            P2Respawn();
        }
    }

    public void P1Respawn()
    {
        if (dieCount > 0)
            dieCount--;
        p1DiePanel.SetActive(false);
        M_Player.instance.transform.position = respawnPosition.position;
        M_Player.instance.gameObject.SetActive(true);
        M_Player.instance.isDie = false;
        M_Player.instance.freeLookCam.enabled = true;
        M_Player.instance.hp = 10;
        Camera camera;
        camera = M_Player.instance.playerCamera;
        camera.rect = new Rect(0, 0, 0.5f, 1f);
        p2PlayCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 1, 1);
        p2DamageEffect.enabled = false;
        camera = M_Player2.instance.playerCamera;
        camera.rect = new Rect(0.5f, 0, 0.5f, 1f);
        p1DiePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(960f, 1080);
        p1PlayCanvas.SetActive(true);
        p2HpImage.transform.localScale = new Vector3(3, 1.5f, 1.5f);
        M_Player2.instance.cameraCenter = 0.75f;
    }

    public void P2Respawn()
    {
        if (dieCount > 0)
            dieCount--;
        p2DiePanel.SetActive(false);
        M_Player2.instance.transform.position = respawnPosition.position;
        M_Player2.instance.gameObject.SetActive(true);
        M_Player2.instance.isDie = false;
        M_Player2.instance.freeLookCam.enabled = true;
        M_Player2.instance.hp = 10;
        Camera camera;
        camera = M_Player.instance.playerCamera;
        camera.rect = new Rect(0, 0, 0.5f, 1f);
        p1PlayCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 1, 1);
        p1DamageEffect.enabled = false;
        camera = M_Player2.instance.playerCamera;
        camera.rect = new Rect(0.5f, 0, 0.5f, 1f);
        p2DiePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(960f, 1080);
        p2PlayCanvas.SetActive(true);
        p2HpImage.transform.localScale = new Vector3(3, 1.5f, 1.5f);
        M_Player.instance.cameraCenter = 0.25f;
    }

    void GameOver()
    {
        if (dieCount >= 2)
        {
            StartCoroutine(IeFadeIn());
            StartCoroutine(IeGameOver());
        }
    }

    IEnumerator IeGameOver()
    {
        dieCount = 0;
        yield return new WaitForSecondsRealtime(3f);
        P1Respawn();
        P2Respawn();
        StartCoroutine(IeFadeOut());
    }

    public IEnumerator IeFadeOut()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        float a;
        for (a = 1; a > 0; a -= 0.01f)
        {
            fadeInOutPanel.color = new Color(0, 0, 0, a);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator IeFadeIn()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        float a;
        for (a = 0; a < 1; a += 0.01f)
        {
            fadeInOutPanel.color = new Color(0, 0, 0, a);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
    }


    IEnumerator IeEndScene()
    {
        yield return new WaitForSeconds(4f);
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        endImage.color = new Color(1, 1, 1, 0);
        float a;
        for (a = 0; a < 1; a += 0.03f)
        {
            fadeInOutPanel.color = new Color(0, 0, 0, a);
            endImage.color = new Color(1, 1,1 , a);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        endImage.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator IeNextScene()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        float a;
        for (a = 0; a < 1; a += 0.01f)
        {
            fadeInOutPanel.color = new Color(0, 0, 0, a);
            yield return new WaitForSeconds(0.01f);
        }
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(IeFadeOut());
    }
}
