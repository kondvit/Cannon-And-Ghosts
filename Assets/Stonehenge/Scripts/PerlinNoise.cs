using UnityEngine;

public class PerlinNoise
{
    private float initialAmplitude;
    private float amplitudeScalingFactor;
    private float initialFrequency;
    private float frequencyScalingFactor;
    private int numberOfOctaves;
    private int seed;

    public PerlinNoise(float initialAmplitude, float amplitudeScalingFactor, float initialFrequency, float frequencyScalingFactor, int numberOfOctaves)
    {
        this.initialAmplitude = initialAmplitude;
        this.amplitudeScalingFactor = amplitudeScalingFactor;
        this.initialFrequency = initialFrequency;
        this.frequencyScalingFactor = frequencyScalingFactor;
        this.numberOfOctaves = numberOfOctaves;

        seed = (int)System.DateTime.Now.Ticks;
    }

    private float CosineInterpolation(float a, float b, float x)
    {
        float pi_x = 3.1415927f * x;
        float f = (1 - Mathf.Cos(pi_x)) / 2;
        return a * (1 - f) + f * b;
    }

    /***************************************
     * FindNode
     * IN: x value that depends on octave
     * OUT: returns i'th octave's random node value corresponding to that x
     * 
     * Since the seed is saved on creation of the object, each octave can be restored from it.
     * 
     ***************************************/
    private float FindNode(int x)
    {
        Random.InitState(seed + x);
        return Random.value;
    }



    /***************************************
     * PerlinInterpolate
     * IN: x value that depends on octave
     * OUT: cosine interpolated value from 0 to 1 in the ith octave at x.
     * 
     ***************************************/
    private float PerlinInterpolate(float x)
    {
        int floor_x = (int)x;
        float fraction = x - floor_x;

        float a = FindNode(floor_x);
        float b = FindNode(floor_x + 1);

        return CosineInterpolation(a, b, x);
    }

    public float GenerateNoise(float x)
    {
        float total = 0;
        float amplitude = initialAmplitude;
        float frequency = initialFrequency;

        for (int i = 0; i <numberOfOctaves; i++)
        {
            frequency = frequency * Mathf.Pow(frequencyScalingFactor, i);
            amplitude = amplitude * Mathf.Pow(amplitudeScalingFactor, i);

            total += amplitude * PerlinInterpolate(x * frequency); //the value from 0 to 1 then scaled with amplitude.
        }

        return total;
    }



}
