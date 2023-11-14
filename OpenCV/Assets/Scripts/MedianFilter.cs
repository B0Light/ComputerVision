using UnityEngine;
using UnityEngine.UI;

public class MedianFilter : MonoBehaviour
{
    public Image InputImage; // 필터링할 Image
    public Image OutputImage;

    public void BTN_MedianFilter()
    {
        // Image가 null이 아닌지 확인
        if (InputImage != null)
        {
            // Image에서 Sprite를 가져옴
            Sprite inputSprite = InputImage.sprite;

            // Sprite의 텍스처를 가져옴
            Texture2D inputTexture = (Texture2D)inputSprite.texture;
            // Texture2D를 Color 배열로 변환
            Color[] pixels = inputTexture.GetPixels();

            // 이미지의 너비와 높이
            int width = inputTexture.width;
            int height = inputTexture.height;

            // Color 배열을 2D 그레이스케일 배열로 변환
            float[,] grayscaleValues = new float[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grayscaleValues[x, y] = pixels[y * width + x].grayscale;
                }
            }

            // 메디안 필터 적용
            ApplyMedianFilter(grayscaleValues);

            // 2D 그레이스케일 배열을 Color 배열로 변환
            Color[] outputPixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    outputPixels[y * width + x] = new Color(grayscaleValues[x, y], grayscaleValues[x, y], grayscaleValues[x, y]);
                }
            }

            // 결과 이미지 생성
            Texture2D outputTexture = new Texture2D(width, height);
            outputTexture.SetPixels(outputPixels);
            outputTexture.Apply();

            // 결과 이미지를 Material의 MainTexture로 할당하거나 다른 방식으로 사용
            OutputImage.sprite = Sprite.Create(outputTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // 메디안 필터 적용
    void ApplyMedianFilter(float[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        int filterSize = 3; // 필터 크기 (3x3)

        float[,] result = new float[width, height];

        // 이미지의 각 픽셀에 메디안 필터 적용
        for (int y = filterSize / 2; y < height - filterSize / 2; y++)
        {
            for (int x = filterSize / 2; x < width - filterSize / 2; x++)
            {
                // 주변 픽셀 값을 수집
                float[] neighborhood = new float[filterSize * filterSize];
                int index = 0;

                for (int ky = 0; ky < filterSize; ky++)
                {
                    for (int kx = 0; kx < filterSize; kx++)
                    {
                        neighborhood[index++] = values[x + kx - filterSize / 2, y + ky - filterSize / 2];
                    }
                }

                // 중간값 계산 및 결과에 저장
                System.Array.Sort(neighborhood);
                result[x, y] = neighborhood[neighborhood.Length / 2];
            }
        }

        // 결과를 원본 배열에 복사
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                values[x, y] = result[x, y];
            }
        }
    }
}
