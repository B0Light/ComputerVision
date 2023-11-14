using UnityEngine;
using UnityEngine.UI;

public class LaplacianFilter : MonoBehaviour
{
    public Image InputImage; // ���͸��� Image
    public Image OutputImage;
    public void BTN_LaplacianFilter()
    {
        // Image�� null�� �ƴ��� Ȯ��
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

            // ���ö�þ� ���� ����
            ApplyLaplacianFilter(grayscaleValues);

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

            // ��� �̹����� Material�� MainTexture�� �Ҵ��ϰų� �ٸ� ������� ���
            OutputImage.sprite = Sprite.Create(outputTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // ���ö�þ� ���� ����
    void ApplyLaplacianFilter(float[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        float[,] laplacianKernel = {
            { 0, 1, 0 },
            { 1, -4f, 1 },
            { 0, 1, 0 }
        };

        int kernelSize = 3;
        int kernelRadius = kernelSize / 2;

        float[,] result = new float[width, height];

        // �̹����� �� �ȼ��� ���ö�þ� ���� ����
        for (int y = kernelRadius; y < height - kernelRadius; y++)
        {
            for (int x = kernelRadius; x < width - kernelRadius; x++)
            {
                float sum = 0.0f;

                // ���ö�þ� Ŀ�� ����
                for (int ky = 0; ky < kernelSize; ky++)
                {
                    for (int kx = 0; kx < kernelSize; kx++)
                    {
                        sum += values[x + kx - kernelRadius, y + ky - kernelRadius] * laplacianKernel[kx, ky];
                    }
                }

                result[x, y] = Mathf.Clamp01(sum);
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
}
