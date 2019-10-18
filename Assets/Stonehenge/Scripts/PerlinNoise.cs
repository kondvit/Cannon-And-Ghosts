using UnityEngine;

public class PerlinNoise
{

    private PerlinOctave[] octaves;

    public PerlinNoise(float initialAmplitude, float amplitudeScalingFactor, float initialFrequency, float frequencyScalingFactor, int numberOfOctaves)
    {
        octaves = new PerlinOctave[numberOfOctaves];

        float amplitude = initialAmplitude;
        float frequency = initialFrequency;

        octaves[0] = new PerlinOctave(amplitude, frequency);

        for (int i = 1; i < numberOfOctaves; i++)
        {
            amplitude *= amplitudeScalingFactor;
            frequency *= frequencyScalingFactor;
            octaves[i] = new PerlinOctave(amplitude, frequency);
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
