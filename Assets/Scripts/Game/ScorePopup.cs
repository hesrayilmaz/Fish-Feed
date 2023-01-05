using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Color textColor;

    private float dissapearTimer = 0.4f;
    private float moveYSpeed = 10f;
    private float disappearSpeed = 3f;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    
    public void Setup(string scoreAmount, bool isCorrectItem)
    {
        textMesh.SetText(scoreAmount);
        if (isCorrectItem)
            textColor = new Color(255 / 255f, 214 / 255f, 66/255f);
        else
            textColor = new Color(12 / 255f, 160 / 255f, 0);
        textMesh.color = textColor;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        dissapearTimer -= Time.deltaTime;
        if (dissapearTimer < 0)
        {
            //Start disappearing
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }
        if (textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
