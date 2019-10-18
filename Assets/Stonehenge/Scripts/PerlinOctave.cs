using UnityEngine;

public class PerlinOctave
{
    private float amplitude;
    private float frequency;
    private int seed;

    public PerlinOctave(float amplitude, float frequency)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;

        seed = Random.Range(int.MinValue, int.MaxValue);

    }

    private float CosineInterpolation(float a, float b, float x)
    {
        float pi_x = Mathf.PI * x;
        float f = (1 - Mathf.Cos(pi_x)) / 2;
        return a * (1 - f) + f * b;
    }

    private float rand(int x)
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

        float a = rand(floor_x);
        float b = rand(floor_x + 1);

        return CosineInterpolation(a, b, fraction);
    }

    public float ComputeNoise(float x)
    {
        return amplitude * PerlinInterpolate(x * frequency); //the value from 0 to 1 then scaled with amplitude.
    }

}
