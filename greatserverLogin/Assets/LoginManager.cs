using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public struct SignUpForm
{
    public string username;
    public string password;
    public string nickname;
    public int score;

}

public class LoginManager : MonoBehaviour {

    public GameObject isOn;
    public Image signupPannel;
    public InputField loginIDInputField;
    public InputField loginPWInputField;

    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public InputField nicknameInputField;

    private void Awake()
    {
        //isOn = isOn.gameObject.GetComponent<GameObject>(); //이미 public으로 선언 후 수동으로 오브젝트와 참조연결시켜줌
        isOn.SetActive(false);
    }

    public void OnClickLoginButton()
    {
        string username = loginIDInputField.text;
        string password = loginPWInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return;
        }
        else
        {
            Debug.Log("아이디가 없거나 비밀번호가 맞지 않습니다");
        }

        SceneManager.LoadScene("Play");
    }

    //회원가입 버튼 이벤트
    public void OnClickSignUpButton()
    {
        //GameObject isOn =  GameObject.Find("on");// 프로그램 실행 도중에 찾아갈 수 있도록 하는 명령어
        //signupPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //signupPannel.GetComponent<GameObject>().SetActive(true);

        if (isOn.activeSelf == false)
        {
            isOn.SetActive(true);
        }
        else
        {
            isOn.SetActive(false);
        }
    }

    //확인 버튼 이벤트
    public void OnClickConfirmButton()
    {
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;
        string username = usernameInputField.text;
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)
            || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname))
        {
            return;
        }
        else
        {
            Debug.Log("빈 칸을 모두 채워주세요");
        }

        if (password.Equals(confirmPassword))
        {
            //ToDo: 서버에 회원가입 정보 전송
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.username = username;
            signUpForm.password = password;
            signUpForm.nickname = nickname;

            StartCoroutine(SignUp(signUpForm));
        }
        else
        {
            Debug.Log("비밀번호가 서로 다릅니다");
        }

        isOn.SetActive(false);
        //signupPannel.GetComponent<GameObject>().SetActive(false); 실패버전
        //signupPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); 선생님 버전
    }

    //취소 버튼 이벤트
    public void OnClickCancelButton()
    {
        isOn.SetActive(false);
        //signupPannel.GetComponent<GameObject>().SetActive(false);
        //signupPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(900, 0);
    }

    IEnumerator SignUp(SignUpForm form)
    {
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/add", sendData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }

        }
    }

}
