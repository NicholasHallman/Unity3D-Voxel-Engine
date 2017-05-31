using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise { 

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0){
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x/ scale;
                float sampleY = y/ scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }
        return noiseMap;
    }

    public static float GenerateNoise(int x, int z, int octave)
    {
        float scale = 300f;
        float value = 0;
        float amplitude = 1.5f;
        for (var i = 0; i < octave; i++)
        {
            value += Mathf.PerlinNoise(x / scale, z / scale) / amplitude;
            scale /= 8;
            amplitude *= 2;
        }
        return (value/octave) * 255;
    }

    public static float[,] GenerateNoiseMapPlayer(int playerX, int playerY,int distance,int biom)
    {
        float[,] noiseMap = new float[32/distance, 32/distance];
        int relX = playerX;
        int relY = playerY;

        for (int y = 0; y < 32; y += distance)
        {
            for (int x = 0; x < 32; x += distance)
            {
                float perlinValue = 0;
                for(int i = 1; i < 64; i += (int) Mathf.Pow(2,i))
                {
                    float sampleX = relX / (300f / i);
                    float sampleY = relY / (300f / i);

                    perlinValue += Mathf.PerlinNoise(sampleX, sampleY) / i;
                    
                }
                if (biom == 0)
                {
                    noiseMap[x / distance, y / distance] = (perlinValue / 7) + .5f;
                }
                else if(biom == 1) 
                {
                    noiseMap[x / distance, y / distance] = perlinValue;
                }

                relX = relX + distance;
            }
            relY = relY + distance;
            relX = playerX;
        }
        return noiseMap;
    }
}
