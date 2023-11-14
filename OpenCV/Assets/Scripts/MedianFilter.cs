using UnityEngine;
using UnityEngine.UI;

public class MedianFilter : MonoBehaviour
{
    public Image InputImage; // ���͸��� Image
    public Image OutputImage;

    public void BTN_MedianFilter()
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

            // �޵�� ���� ����
            ApplyMedianFilter(grayscaleValues);

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

    // �޵�� ���� ����
    void ApplyMedianFilter(float[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        int filterSize = 3; // ���� ũ�� (3x3)

        float[,] result = new float[width, height];

        // �̹����� �� �ȼ��� �޵�� ���� ����
        for (int y = filterSize / 2; y < height - filterSize / 2; y++)
        {
            for (int x = filterSize / 2; x < width - filterSize / 2; x++)
            {
                // �ֺ� �ȼ� ���� ����
                float[] neighborhood = new float[filterSize * filterSize];
                int index = 0;

                for (int ky = 0; ky < filterSize; ky++)
                {
                    for (int kx = 0; kx < filterSize; kx++)
                    {
                        neighborhood[index++] = values[x + kx - filterSize / 2, y + ky - filterSize / 2];
                    }
                }

                // �߰��� ��� �� ����� ����
                System.Array.Sort(neighborhood);
                result[x, y] = neighborhood[neighborhood.Length / 2];
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
