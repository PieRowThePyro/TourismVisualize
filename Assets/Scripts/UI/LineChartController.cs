using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LineChartController : MonoBehaviour
{

    List<GameObject> lineList = new List<GameObject>();

    private DD_DataDiagram m_DataDiagram;
    //private RectTransform DDrect;

    private bool m_IsContinueInput = false;
    private float m_Input = 0f;
    private float h = 0;

    void AddALine()
    {

        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.1f) > 1 ? (h - 1) : h, 0.8f, 0.8f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);
        if (null != line)
            lineList.Add(line);
    }

    // Use this for initialization
    void Start()
    {

        GameObject dd = GameObject.Find("DataDiagram");
        if (null == dd)
        {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();

        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };

        AddALine();
        StrategyManager.DoAlgorithmEvent += onButton;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        m_Input += Time.deltaTime;
        ContinueInput(m_Input);
    }

    private void ContinueInput(float f)
    {

        if (null == m_DataDiagram)
            return;

        if (false == m_IsContinueInput)
            return;

        float d = 0f;
        foreach (GameObject l in lineList)
        {
            m_DataDiagram.InputPoint(l, new Vector2(0.1f,
                (Mathf.Sin(f + d) + 1f) * 2f));
            d += 1f;
        }
    }
    private float current = 10;
    public void onButton(object sender, SolutionInfo s)
    {
        current -= 0.2f;
        if (null == m_DataDiagram)
            return;
            
        foreach (GameObject l in lineList)
        {
            m_DataDiagram.InputPoint(l, new Vector2(1, s.fitness));
        }
        
    }

    public void OnAddLine()
    {
        AddALine();
    }


    public void OnContinueInput()
    {

        m_IsContinueInput = !m_IsContinueInput;

    }

}
