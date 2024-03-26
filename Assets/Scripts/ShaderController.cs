using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1,1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;

    [SerializeField] private float curveTime = 1;
    [SerializeField] private float waitTime = 3;

    float currentXValue = 0;
    float currentYValue = 0;
    float nextXValue = 1;
    float nextYValue = 1;
    float tempXValue = 1;
    float tempYValue = 1;

    private float elapsedTime = 0;
    private float intervalTime;

    private bool changePosition = true;
    private bool updateNextPosition = true;

    void Start()
    {
        curveX = 0;
        curveY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
        if (GameManager.instance.IsGameStarted && !GameManager.instance.IsGameOver)
        {
            SetShaderValues();
        }

        if (updateNextPosition && elapsedTime > 0)
        {
            nextXValue = tempXValue;
            nextYValue = tempYValue;
            updateNextPosition = false;
        }

    }
    void SetShaderValues()
    {
        elapsedTime += Time.deltaTime;
        intervalTime = elapsedTime / curveTime;
        curveX = Mathf.Lerp(currentXValue, nextXValue, intervalTime);
        curveY = Mathf.Lerp(currentYValue, nextYValue, intervalTime);

        if (changePosition)
        {
            changePosition = false;
            StartCoroutine(ChangeNextPosition());
            elapsedTime = 0;
            updateNextPosition = true;
        }
    }
    IEnumerator ChangeNextPosition()
    {
        yield return new WaitForSeconds(waitTime);
        currentXValue = curveX;
        currentYValue = curveY;
        tempXValue = Random.Range(-1.0f, 1.0f);
        tempYValue = curveY;
        changePosition = true;
    }

}
