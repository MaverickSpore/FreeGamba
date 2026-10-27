using UnityEngine;

public static class MavRandom
{
    public static float LogRange(float min, float max)
    {
        if (min <= 0)
        {
            min = 0.01f;
        }
        if (max <= min)
        {
            max = min + 0.1f;
        }
        float logMin = Mathf.Log(min);
        float logMax = Mathf.Log(max);
        float randomLogValue = Random.Range(logMin, logMax);
        return Mathf.Exp(randomLogValue);
    }
    public static float ExpRange(float min, float max, float exponent)
    {
        if (min <= 0)
        {
            min = 0.01f;
        }
        if (max <= min)
        {
            max = min + 0.1f;
        }

        return min + (max - min) * Mathf.Pow(1 - Random.value, exponent);
    }
    public static float GeoRange(float min, float max, float falloff = 3f)
    {
        // MIN and MAX are exclusive boundaries
        int first = Mathf.FloorToInt(min) + 1;
        int last = Mathf.CeilToInt(max) - 1;

        if (last < first)
            return first;

        falloff = Mathf.Max(0.01f, falloff);

        // Calculate total weight
        float totalWeight = 0f;

        for (int i = first; i <= last; i++)
        {
            totalWeight += Mathf.Pow(falloff, last - i);
        }

        // Generate weighted random value
        float roll = Random.value * totalWeight;

        // Determine selected rarity
        for (int i = first; i <= last; i++)
        {
            roll -= Mathf.Pow(falloff, last - i);

            if (roll < 0f)
                return i;
        }

        return last;
    }
}
