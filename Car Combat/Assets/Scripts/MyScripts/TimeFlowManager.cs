using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFlowManager : MonoBehaviour
{

    [SerializeField] float slowDownFactor = 0.05f;
    [SerializeField] float slowDownLength = 2f;

    
    void Update()
    {
        //TestMethod();
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void TestMethod()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DoSlowMotion();
        }
    }
}
