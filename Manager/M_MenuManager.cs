using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_MenuManager : MonoBehaviour
{
    public Image joyStickImage;
    public Image keyboardImage;
    public Image joyStickStartImage;
    public Image keyboardStartImage;
    public Image selectMayImage;
    public Image selectCodyImage;
    public GameObject leftHand;
    public GameObject rightHand;

    int joyCount = 0;
    int keyCount = 0;
    bool isMay = false;
    bool isCody = false;
    bool isJoyReady = false;
    bool isKeyReady = false;

    float time = 0;
    void Start()
    {
        M_GameManager.instance.StartCoroutine("IeFadeOut");
    }

    void Update()
    {

        time += Time.deltaTime;
        //둘 다 준비되면 각각 어떤 캐릭터를 선택했는 지 저장
        if (isJoyReady && isKeyReady)
        {
            isJoyReady = false;
            isKeyReady = false;
            if (keyCount == -1)
            {
                PlayerPrefs.SetInt("KeyPlayer", 1);
                PlayerPrefs.SetInt("JoyPlayer", 2);
            }
            else if (keyCount == 1)
            {
                PlayerPrefs.SetInt("KeyPlayer", 2);
                PlayerPrefs.SetInt("JoyPlayer", 1);
            }
            print(joyCount);
            print(keyCount);
            M_GameManager.instance.StartCoroutine("IeNextScene");
        }
        if (joyCount >= -1 && joyCount <= 1 && time > 0.5f)
        {
            JoyStickSelect();
        }
        if (keyCount >= -1 && keyCount <= 1)
        {
            KeyboardSelect();
        }   
    }

    void JoyStickSelect()
    {
        float h = Input.GetAxis("JoyHorizontal");
        //나중에 조이스틱 왼쪽으로 바꾸기
        if (h < -0.05f)
        {
            if (joyCount == 1)
                isCody = false;
            joyCount--;
            if (joyCount <= -1)
            {
                if (!isMay)
                    SelectMay(joyStickImage);
                else
                {
                    joyCount++;
                }
            }
            else if (joyCount == 0)
            {
                StartCoroutine(IeDeselect("JoyStick", "Cody"));
            }
            time = 0;
        }
        //나중에 조이스틱 오른쪽으로 바꾸기
        else if (h > 0.05f)
        {
            if (joyCount == -1)
                isMay = false;
            joyCount++;
            if (joyCount >= 1)
            {
                if (!isCody)
                    SelectCody(joyStickImage);
                else
                {
                    joyCount--;
                }
            }
            else if (joyCount == 0)
            {
                StartCoroutine(IeDeselect("JoyStick", "May"));
            }
            time = 0;
        }
        if (joyCount != 0)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                joyStickImage.transform.GetChild(0).GetComponent<Text>().text = "OK!!";
                isJoyReady = true;
            }
        }
    }

    void KeyboardSelect()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (keyCount == 1)
                isCody = false;
            keyCount--;
            if (keyCount <= -1)
            {
                if (!isMay)
                    SelectMay(keyboardImage);
                else
                {
                    keyCount++;
                }
            }
            else if (keyCount == 0)
            {
                StartCoroutine(IeDeselect("Keyboard", "Cody"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (keyCount == -1)
                isMay = false;
            keyCount++;
            if (keyCount >= 1)
            {
                if (!isCody)
                    SelectCody(keyboardImage);
                else
                {
                    keyCount--;
                }
            }
            else if (keyCount == 0)
            {
                StartCoroutine(IeDeselect("Keyboard", "May"));
            }
        }
        if (keyCount != 0)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                keyboardImage.transform.GetChild(0).GetComponent<Text>().text = "OK!!";
                isKeyReady = true;
            }
        }
    }

    IEnumerator IeDeselect(string controllerName, string dollName)
    {
        if (controllerName == "Keyboard")
        {
            isKeyReady = false;
            keyboardImage.transform.GetChild(0).GetComponent<Text>().text = "Keyboard";
            for (int i = 1; i <= 50; i++)
            {
                keyboardImage.color = Color.Lerp(keyboardImage.color, keyboardStartImage.color, i / 100f);
                keyboardImage.rectTransform.position = Vector3.Lerp(keyboardImage.rectTransform.position, keyboardStartImage.rectTransform.position, i / 100f);
                keyboardImage.rectTransform.sizeDelta = Vector2.Lerp(keyboardImage.rectTransform.sizeDelta, keyboardStartImage.rectTransform.sizeDelta, i / 100f);
                if (dollName == "May")
                    leftHand.transform.Rotate(0, 0, -1f);
                else if (dollName == "Cody")
                    rightHand.transform.Rotate(0, 0, 1f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (controllerName == "JoyStick")
        {
            isJoyReady = false;
            joyStickImage.transform.GetChild(0).GetComponent<Text>().text = "JoyStick";
            for (int i = 1; i <= 50; i++)
            {
                joyStickImage.color = Color.Lerp(joyStickImage.color, joyStickStartImage.color, i / 100f);
                joyStickImage.rectTransform.position = Vector3.Lerp(joyStickImage.rectTransform.position, joyStickStartImage.rectTransform.position, i / 100f);
                joyStickImage.rectTransform.sizeDelta = Vector2.Lerp(joyStickImage.rectTransform.sizeDelta, joyStickStartImage.rectTransform.sizeDelta, i / 100f);
                if (dollName == "May")
                    leftHand.transform.Rotate(0, 0, -1f);
                else if (dollName == "Cody")
                    rightHand.transform.Rotate(0, 0, 1f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    void SelectMay(Image image)
    {
        isMay = true;
        image.transform.GetChild(0).GetComponent<Text>().text += "\nOk?";
        StartCoroutine(IeSelectMay(image));
    }

    IEnumerator IeSelectMay(Image image)
    {
        for (int i = 1; i <= 50; i++)
        {
            image.color = Color.Lerp(image.color, selectMayImage.color, i / 100f);
            image.rectTransform.position = Vector3.Lerp(image.rectTransform.position, selectMayImage.rectTransform.position, i / 100f);
            image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, selectMayImage.rectTransform.sizeDelta, i / 100f);
            leftHand.transform.Rotate(0, 0, 1f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void SelectCody(Image image)
    {
        isCody = true;
        image.transform.GetChild(0).GetComponent<Text>().text += "\nOk?";
        StartCoroutine(IeSelectCody(image));
    }

    IEnumerator IeSelectCody(Image image)
    {
        for (int i = 1; i <= 50; i++)
        {
            image.color = Color.Lerp(image.color, selectCodyImage.color, i / 100f);
            image.rectTransform.position = Vector3.Lerp(image.rectTransform.position, selectCodyImage.rectTransform.position, i / 100f);
            image.rectTransform.sizeDelta = Vector2.Lerp(image.rectTransform.sizeDelta, selectCodyImage.rectTransform.sizeDelta, i / 100f);
            rightHand.transform.Rotate(0, 0, -1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
