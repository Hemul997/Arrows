using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestResultsPrinter : MonoBehaviour
{
    private const string DELTA_PRINT_FORMAT = "0.00";

    // UI
    [SerializeField] private Text _textCurrentResult;
    [SerializeField] private Text _textAllResults;

    /// <summary>
    /// Printed current result of testing
    /// </summary>
    /// <param name="horDelta"></param>
    /// <param name="verDelta"></param>
    public void PrintCurrResult(float horDelta, float verDelta)
    {
        _textCurrentResult.text = "h:" + horDelta.ToString(DELTA_PRINT_FORMAT)
                          + "\nv:" + verDelta.ToString(DELTA_PRINT_FORMAT);
    }

    /// <summary>
    /// Printed results of testing
    /// </summary>
    /// <param name="results">List of test results</param>
    public void PrintAllResults(List<Vector2> results)
    {
        foreach (var result in results)
        {
            _textAllResults.text += "h:" + result.x.ToString(DELTA_PRINT_FORMAT)
                                    + "\nv:" + result.y.ToString(DELTA_PRINT_FORMAT) + "\n\n";
        }
    }

    /// <summary>
    /// Reset results of current test iteration
    /// </summary>
    public void ResetCurrResultText()
    {
        _textCurrentResult.text = "h:_.__\nv:_.__";
    }

}
