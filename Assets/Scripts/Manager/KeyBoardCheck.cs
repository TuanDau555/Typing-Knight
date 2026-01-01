using UnityEngine;
using TMPro;
public class KeyBoardCheck : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    private string allowedChars = "abcdefghijklmnopqrstuvwxyz";

    // BIẾN QUAN TRỌNG: Để các script khác biết con này đang giữ chữ gì
    public char myChar;

    void Start()
    {
        SetRandomLetter();
    }

    void SetRandomLetter()
    {
        int randomIndex = Random.Range(0, allowedChars.Length);
        myChar = allowedChars[randomIndex]; // Lưu chữ cái vào biến
        textMesh.text = myChar.ToString();  // Hiển thị lên đầu
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 0)
        {
            textMesh.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            textMesh.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
