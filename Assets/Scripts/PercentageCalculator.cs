using UnityEngine;
using System.Collections;

public class PercentageCalculator : MonoBehaviour {

    public static bool isWithinRange(int target)
    {
        if (target > 100)
            target = 100;
        else if (target < 0)
            target = 0;

        return Random.Range(0, 100) <= target;
    }
}
