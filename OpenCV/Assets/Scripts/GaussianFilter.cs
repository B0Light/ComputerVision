using UnityEngine;
using UnityEngine.UI;

public class GaussianFilter : MonoBehaviour
{
    public Image InputImage; // �ν����� â���� �̹����� �巡�׾� ������� �Ҵ�
    public Image OutputImage;

    public void BTN_GaussianFilter()
    {
        if (InputImage != null)
        {
            // Image���� Sprite�� ������
            Sprite inputSprite = InputImage.sprite;

            // Sprite�� �ؽ�ó�� ������
            Texture2D inputTexture = (Texture2D)inputSprite.texture;

            // Texture2D�� Color �迭�� ��ȯ
            Color[] pixels = inputTexture.GetPixels();

            // �̹����� �ʺ�� ����
            int width = inputTexture.width;
            int height = inputTexture.height;

            // Color �迭�� 2D �׷��̽����� �迭�� ��ȯ
            float[,] grayscaleValues = new float[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grayscaleValues[x, y] = pixels[y * width + x].grayscale;
                }
            }

            // ����þ� ���� ����
            ApplyGaussianFilter(grayscaleValues);

            // 2D �׷��̽����� �迭�� Color �迭�� ��ȯ
            Color[] outputPixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    outputPixels[y * width + x] = new Color(grayscaleValues[x, y], grayscaleValues[x, y], grayscaleValues[x, y]);
                }
            }

            // ��� �̹��� ����
            Texture2D outputTexture = new Texture2D(width, height);
            outputTexture.SetPixels(outputPixels);
            outputTexture.Apply();


            OutputImage.sprite = Sprite.Create(outputTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // ����þ� ���� ����
    void ApplyGaussianFilter(float[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        float[,] kernel = GenerateGaussianKernel(5, 1.4f);

        int kernelSize = kernel.GetLength(0);
        int kernelRadius = kernelSize / 2;

        float[,] result = new float[width, height];

        // �̹����� �� �ȼ��� ����þ� ���� ����
        for (int y = kernelRadius; y < height - kernelRadius; y++)
        {
            for (int x = kernelRadius; x < width - kernelRadius; x++)
            {
                float sum = 0.0f;

                // ����þ� Ŀ�� ����
                for (int ky = 0; ky < kernelSize; ky++)
                {
                    for (int kx = 0; kx < kernelSize; kx++)
                    {
                        sum += values[x + kx - kernelRadius, y + ky - kernelRadius] * kernel[kx, ky];
                    }
                }

                result[x, y] = sum;
            }
        }

        // ����� ���� �迭�� ����
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                values[x, y] = result[x, y];
            }
        }
    }

    // ����þ� ���� Ŀ�� ����
    float[,] GenerateGaussianKernel(int size, float sigma)
    {
        float[,] kernel = new float[size, size];
        float sum = 0.0f;
        int radius = size / 2;

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                kernel[x + radius, y + radius] = Mathf.Exp(-(x * x + y * y) / (2 * sigma * sigma));
                sum += kernel[x + radius, y + radius];
            }
        }

        // ����ȭ
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                kernel[x, y] /= sum;
            }
        }

        return kernel;
    }
}
