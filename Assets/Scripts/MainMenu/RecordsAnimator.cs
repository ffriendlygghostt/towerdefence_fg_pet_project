using System.Collections;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

public class RecordsAnimator : MonoBehaviour
{
    [Header("Fields")] 
    public TMP_Text[] recordFields;

    [Header("Settigns")]
    public float initialDelay = 2.5f;
    public float animationDuration = 2f;
    public float repeatDelay = 7f;
    public float updateInterval = 0.05f;

    void Start()
    {
        StartCoroutine(AnimateRecordsLoop());
    }

    IEnumerator AnimateRecordsLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return StartCoroutine(AnimateRecords());
            yield return new WaitForSeconds(repeatDelay);
        }
    }

    IEnumerator AnimateRecords()
    {
        float timer = 0f;

        int[] targetNumbers = new int[recordFields.Length];

        for (int i = 0; i < recordFields.Length; i++)
        {
            string text = recordFields[i].text;
            int num = 0;

            Match match = Regex.Match(text, @"\d+");
            if (match.Success)
            {
                num = int.Parse(match.Value);
            }

            targetNumbers[i] = num;
        }

        while (timer < animationDuration)
        {
            timer += updateInterval;

            for (int i = 0; i < recordFields.Length; i++)
            {
                int digits = targetNumbers[i].ToString().Length;
                int randomValue;

                switch (digits)
                {
                    case 1: randomValue = Random.Range(0, 10); break;
                    case 2: randomValue = Random.Range(10, 100); break;
                    case 3: randomValue = Random.Range(100, 1000); break;
                    case 4: randomValue = Random.Range(1000, 10000); break;
                    case 5: randomValue = Random.Range(10000, 100000); break;
                    default: randomValue = targetNumbers[i]; break;
                }

                recordFields[i].text = randomValue.ToString();
            }

            yield return new WaitForSeconds(updateInterval);
        }

        for (int i = 0; i < recordFields.Length; i++)
        {
            recordFields[i].text = targetNumbers[i].ToString();
        }
    }
}
