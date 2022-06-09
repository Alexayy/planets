using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    private NoiseSettings _settings;
    private Noise noise = new Noise();

    public NoiseFilter(NoiseSettings settings)
    {
        _settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _settings.numberOfLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + _settings.center);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}
