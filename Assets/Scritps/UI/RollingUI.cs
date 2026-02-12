using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollingUI : MonoBehaviour
{
    private LevelSeletionPanel level;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private Button btnLevel1;
    [SerializeField] private Button btnLevel2;
    [SerializeField] private Button btnLevel3;
    public Transform OptionGroup;
    private Transform[] options;
    private Coroutine currentPIE;
    [Range(1, 4)]
    public int optionCount;
    [Range(1, 100)]
    public float speed;
    [Range(0, 10f)]
    public float yOffset;
    private float halfNum;
    private float radius = 500;

    Dictionary<Transform, Vector3> OptionP = new Dictionary<Transform, Vector3>();
    Dictionary<Transform, int> OptionS = new Dictionary<Transform, int>();

    void Awake()
    {
        level = GetComponentInParent<LevelSeletionPanel>();

    }


    void Start()
    {
        AddButtonEvent();

        halfNum = optionCount / 2;
        options = new Transform[optionCount];

        for (int i = 0; i < optionCount; i++)
        {
            options[i] = OptionGroup.GetChild(i);
            if (i >= 1)
            {
                OptionGroup.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        btnLeft.gameObject.SetActive(true);
        btnRight.gameObject.SetActive(true);

        InitPosition();
        InitSibling();
    }

    private void AddButtonEvent()
    {

        btnLeft.onClick.AddListener(() =>
        {
            ClickLeft();
        });

        btnRight.onClick.AddListener(() =>
        {
            ClickRight();
        });

        btnLevel1.onClick.AddListener(() =>
        {
            MoveTrans(btnLevel1.gameObject);

            level.SelectLevelScene1(true);
        });
        btnLevel2.onClick.AddListener(() =>
        {

            MoveTrans(btnLevel2.gameObject);
            level.SelectLevelScene2(true);
        });
        btnLevel3.onClick.AddListener(() =>
        {
            MoveTrans(btnLevel3.gameObject);
            level.SelectLevelScene3(true);
        });

    }

    private void MoveTrans(GameObject target)
    {
        if (OptionGroup.GetChild(0).gameObject == target)
        {
            ClickLeft();
        }
        else if (OptionGroup.GetChild(1).gameObject == target)
        {
            ClickRight();
        }
    }

    public void LevelShow(string _level)
    {

        transform.GetComponentInParent<UI>().SetupScore(GameModel.Score.ToString(), 0);
        transform.Find(_level).GetComponent<Button>().interactable = true;

    }

    public void ClickLeft()
    {
        StartCoroutine(MoveLeft());

    }

    public void ClickRight()
    {

        StartCoroutine(MoveRight());

    }

    IEnumerator MoveLeft()
    {
        if (currentPIE != null)
        {
            yield return currentPIE;
        }

        Vector3 p = OptionP[options[0]];
        int s = OptionS[options[0]];
        Vector3 targetP;

        for (int i = 0; i < optionCount; i++)
        {
            if (i == optionCount - 1)
            {
                targetP = p;
                OptionS[options[i]] = s;
            }
            else
            {
                targetP = options[(i + 1) % optionCount].localPosition;
                OptionS[options[i]] = OptionS[options[i + 1 % optionCount]];
            }

            options[i].SetSiblingIndex(OptionS[options[i]]);
            currentPIE = StartCoroutine(MoveToTarget(options[i], targetP));

        }

        yield return null;
    }

    IEnumerator MoveRight()
    {
        if (currentPIE != null)
        {
            yield return currentPIE;
        }

        Vector3 p = OptionP[options[optionCount - 1]];
        int s = OptionS[options[optionCount - 1]];
        Vector3 targetP;

        for (int i = optionCount - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                targetP = p;
                OptionS[options[i]] = s;
            }
            else
            {
                targetP = options[(i - 1) % optionCount].localPosition;
                OptionS[options[i]] = OptionS[options[i - 1 % optionCount]];
            }

            options[i].SetSiblingIndex(OptionS[options[i]]);
            currentPIE = StartCoroutine(MoveToTarget(options[i], targetP));

        }

        yield return null;

    }


    IEnumerator MoveToTarget(Transform tf, Vector3 target)
    {

        float tempSpeed = (tf.localPosition - target).magnitude * speed;

        while (tf.localPosition != target)
        {
            tf.localPosition = Vector3.MoveTowards(tf.localPosition, target, tempSpeed * Time.deltaTime);

            yield return null;
        }

        OptionP[tf] = target;

        yield return null;


    }

    void InitSibling()
    {
        for (int i = 0; i < optionCount; i++)
        {
            if (i <= halfNum)
            {
                if (optionCount % 2 == 0)
                {
                    options[i].SetSiblingIndex((int)halfNum - i);
                }
                else
                {
                    options[i].SetSiblingIndex((int)((optionCount - 1) / 2) - i);
                }
            }
            else
            {
                options[i].SetSiblingIndex(options[optionCount - i].GetSiblingIndex());
            }
        }

        for (int i = 0; i < optionCount; i++)
        {
            OptionS.Add(options[i], options[i].GetSiblingIndex());
        }
    }


    void InitPosition()
    {

        float angle = 0;

        for (int i = 0; i < optionCount; i++)
        {

            angle = (360.0f / ((float)optionCount)) * i * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            float y = 0;

            if (i != 0)
            {
                y = i * yOffset;

                if (i > halfNum)
                {
                    y = (optionCount - i) * yOffset;
                }
            }

            options[i].localPosition = new Vector3(x, y, z);
            OptionP.Add(options[i], options[i].localPosition);

        }
    }


}
