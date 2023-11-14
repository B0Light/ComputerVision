using UnityEngine;
using UnityEngine.UI;

public class MeanFilter : MonoBehaviour
{
    public Image InputImage; // 필터링할 Image
    public Image OutputImage;

    public void BTN_MeanFilter()
    {
        // Image가 null이 아닌지 확인
        if (InputImage != null)
        {
            // Image에서 Sprite를 가져옴
            Sprite inputSprite = InputImage.sprite;

            // Sprite의 텍스처를 가져옴
            Texture2D inputTexture = (Texture2D)inputSprite.texture;

            // 텍스처의 픽셀 배열을 가져옴
            Color[] pixels = inputTexture.GetPixels();

            // 텍스처의 너비와 높이를 가져옴
            int width = inputTexture.width;
            int height = inputTexture.height;

            // 필터링된 픽셀 배열
            Color[] filteredPixels = new Color[pixels.Length];

            // 각 픽셀에 대해 평균 필터 적용
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 픽셀 주변 픽셀의 평균 계산
                    Color averageColor = CalculateAverageColor(x, y, pixels, width, height);

                    // 필터링된 픽셀에 대입
                    filteredPixels[y * width + x] = averageColor;
                }
            }

            // 필터링된 결과를 적용한 새로운 텍스처 생성
            Texture2D filteredTexture = new Texture2D(width, height);
            filteredTexture.SetPixels(filteredPixels);
            filteredTexture.Apply();

            // Image의 Sprite를 새로운 텍스처로 설정
            OutputImage.sprite = Sprite.Create(filteredTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // 특정 픽셀 주변의 픽셀 값의 평균 계산
    Color CalculateAverageColor(int x, int y, Color[] pixels, int width, int height)
    {
        // 주변 픽셀의 총 합을 나타내는 변수
        float totalR = 0f, totalG = 0f, totalB = 0f;

        // 주변 픽셀의 수를 나타내는 변수
        int count = 0;

        // 픽셀 주변의 픽셀을 순회하면서 총 합과 개수 계산
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

        // 총 합을 개수로 나누어 평균 계산
        float averageR = totalR / count;
        float averageG = totalG / count;
        float averageB = totalB / count;

        return new Color(averageR, averageG, averageB);
    }
}
