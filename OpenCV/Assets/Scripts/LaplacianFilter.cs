using UnityEngine;
using UnityEngine.UI;

public class LaplacianFilter : MonoBehaviour
{
    public Image InputImage; // 필터링할 Image
    public Image OutputImage;
    public void BTN_LaplacianFilter()
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

            // 라플라시안 필터 적용
            ApplyLaplacianFilter(grayscaleValues);

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

    // 라플라시안 필터 적용
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

        // 이미지의 각 픽셀에 라플라시안 필터 적용
        for (int y = kernelRadius; y < height - kernelRadius; y++)
        {
            for (int x = kernelRadius; x < width - kernelRadius; x++)
            {
                float sum = 0.0f;

                // 라플라시안 커널 적용
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
