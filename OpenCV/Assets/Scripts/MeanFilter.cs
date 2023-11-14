using UnityEngine;
using UnityEngine.UI;

public class MeanFilter : MonoBehaviour
{
    public Image InputImage; // ���͸��� Image
    public Image OutputImage;

    public void BTN_MeanFilter()
    {
        // Image�� null�� �ƴ��� Ȯ��
        if (InputImage != null)
        {
            // Image���� Sprite�� ������
            Sprite inputSprite = InputImage.sprite;

            // Sprite�� �ؽ�ó�� ������
            Texture2D inputTexture = (Texture2D)inputSprite.texture;

            // �ؽ�ó�� �ȼ� �迭�� ������
            Color[] pixels = inputTexture.GetPixels();

            // �ؽ�ó�� �ʺ�� ���̸� ������
            int width = inputTexture.width;
            int height = inputTexture.height;

            // ���͸��� �ȼ� �迭
            Color[] filteredPixels = new Color[pixels.Length];

            // �� �ȼ��� ���� ��� ���� ����
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // �ȼ� �ֺ� �ȼ��� ��� ���
                    Color averageColor = CalculateAverageColor(x, y, pixels, width, height);

                    // ���͸��� �ȼ��� ����
                    filteredPixels[y * width + x] = averageColor;
                }
            }

            // ���͸��� ����� ������ ���ο� �ؽ�ó ����
            Texture2D filteredTexture = new Texture2D(width, height);
            filteredTexture.SetPixels(filteredPixels);
            filteredTexture.Apply();

            // Image�� Sprite�� ���ο� �ؽ�ó�� ����
            OutputImage.sprite = Sprite.Create(filteredTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // Ư�� �ȼ� �ֺ��� �ȼ� ���� ��� ���
    Color CalculateAverageColor(int x, int y, Color[] pixels, int width, int height)
    {
        // �ֺ� �ȼ��� �� ���� ��Ÿ���� ����
        float totalR = 0f, totalG = 0f, totalB = 0f;

        // �ֺ� �ȼ��� ���� ��Ÿ���� ����
        int count = 0;

        // �ȼ� �ֺ��� �ȼ��� ��ȸ�ϸ鼭 �� �հ� ���� ���
        for (int offsetY = -1; offsetY <= 1; offsetY++)
        {
            for (int offsetX = -1; offsetX <= 1; offsetX++)
            {
                int neighborX = Mathf.Clamp(x + offsetX, 0, width - 1);
                int neighborY = Mathf.Clamp(y + offsetY, 0, height - 1);

                Color neighborColor = pixels[neighborY * width + neighborX];

                totalR += neighborColor.r;
                totalG += neighborColor.g;
                totalB += neighborColor.b;

                count++;
            }
        }

        // �� ���� ������ ������ ��� ���
        float averageR = totalR / count;
        float averageG = totalG / count;
        float averageB = totalB / count;

        return new Color(averageR, averageG, averageB);
    }
}
