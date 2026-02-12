using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_RollingUI : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionParent;
    private Transform[] options;
    Dictionary<Transform, Vector3> OptionP = new Dictionary<Transform, Vector3>();

    [Range(0, 10)]
    public int optionCount;
    [Range(0, 100f)]
    public float yOffset;
    private float halfCount;


    void Awake()
    {
        for (int i = 0; i < optionCount; i++)
        {
            GameObject optino = Instantiate(optionPrefab, optionParent);
        }

        halfCount = optionCount / 2;
        options = new Transform[optionCount];

        for (int i = 0; i < optionCount; i++)
        {
            options[i] = optionParent.GetChild(i);
        }

        InitPosition();
    }

    private void InitPosition()
    {
        float radius = 500;
        float angle = 0;


        for (int i = 0; i < optionCount; i++)
        {
            angle = (360f / Mathf.RoundToInt(optionCount)) * i * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            float y = 0;

            if (i != 0)
            {
                y = i * yOffset;

                if (i > halfCount)
                {
                    y = (optionCount - i) * yOffset;
                }


            }

            options[i].localPosition = new Vector3(x, y, z);

            OptionP.Add(options[i], options[i].localPosition);
        }
    }

}
