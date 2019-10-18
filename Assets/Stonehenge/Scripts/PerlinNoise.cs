using UnityEngine;

public class PerlinNoise
{

    private PerlinOctave[] octaves;

    public PerlinNoise(float initialAmplitude, float amplitudeScalingFactor, float initialFrequency, float frequencyScalingFactor, int numberOfOctaves, float end)
    {
        octaves = new PerlinOctave[numberOfOctaves];

        float amplitude = initialAmplitude;
        float frequency = initialFrequency;

        int segmentCountInInterval = (int)(end * frequency);
        frequency = segmentCountInInterval / end;
        if (frequency >= 1)
        {
            Debug.Log("Frequency is set too high, can not guarantee to fit all the octaves in the interval [0, end]");
        }

        octaves[0] = new PerlinOctave(amplitude, frequency, end);

        for (int i = 1; i < numberOfOctaves; i++)
        {
            amplitude *= amplitudeScalingFactor;
            frequency *= frequencyScalingFactor;
            octaves[i] = new PerlinOctave(amplitude, frequency, end);
        }
    } 

    public float GenerateNoise(float x)
    {
        float totalNoise = 0;

        foreach(PerlinOctave oct in octaves)
        {
            totalNoise += oct.ComputeNoise(x);
        }
        return totalNoise;
    }
}
