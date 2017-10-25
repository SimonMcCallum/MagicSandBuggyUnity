using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class DummyData {

	public static string GenerateSinString(float from,float to,float scale)
    {
        StringBuilder sb = new StringBuilder();

        for (int x = 0; x < 640; x++)
        {
            for (int y = 0; y < 480; y++)
            {
                float tmp = (Mathf.Sin(y * scale) + 1) / 2;
                tmp += (Mathf.Sin(x * scale / 2) + 1) / 2;
                tmp = (tmp * (to - from)) + from;

                sb.Append((int)(tmp)+" ");
            }
        }

        return sb.ToString();
    }

    public static float[,] GenerateFloatArr(float scale)
    {
        float[,] arr = new float[480, 640];

        for (int x = 0; x < 640; x++)
        {
            for (int y = 0; y < 480; y++)
            {
                float tmp = (Mathf.Sin(y * scale) + 1) / 2;
                tmp += (Mathf.Sin(x * scale / 2) + 1) / 2;
                tmp *= 0.5f;

                arr[y, x] = tmp;
            }
        }

        return arr;
    }
}
