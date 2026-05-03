using UnityEngine;

public static class RandomExtensions
{
    // ReSharper disable once InvalidXmlDocComment
    /// <summary>
    /// Returns whether a probability was hit.
    /// </summary>
    /// <param name="chance">The probability, in <b>[0,100] percentage</b> range.</param>
    /// <returns>True if the chance was hit.</returns>
    public static bool Chance(double chance)
    {
        chance /= 100.0; // Use 0-100% range.
        if (chance > 1.0 || chance < 0.0) return false;
        return Random.value < chance;
    }
}
