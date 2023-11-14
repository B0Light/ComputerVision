using UnityEngine;
using UnityEngine.UI;

public class GaussianFilter : MonoBehaviour
{
    public Image InputImage; // 인스펙터 창에서 이미지를 드래그앤 드롭으로 할당
    public Image OutputImage;

    public void BTN_GaussianFilter()
    {
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

            // 가우시안 필터 적용
            ApplyGaussianFilter(grayscaleValues);

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


            OutputImage.sprite = Sprite.Create(outputTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }

    // 가우시안 필터 적용
    void ApplyGaussianFilter(float[,] values)
    {
        int width = values.GetLength(0);
        int height = values.GetLength(1);

        float[,] kernel = GenerateGaussianKernel(5, 1.4f);

        int kernelSize = kernel.GetLength(0);
        int kernelRadius = kernelSize / 2;

        float[,] result = new float[width, height];

        // 이미지의 각 픽셀에 가우시안 필터 적용
        for (int y = kernelRadius; y < height - kernelRadius; y++)
        {
            for (int x = kernelRadius; x < width - kernelRadius; x++)
            {
                float sum = 0.0f;

                // 가우시안 커널 적용
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

        // 결과를 원본 배열에 복사
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                values[x, y] = result[x, y];
            }
        }
    }

    // 가우시안 필터 커널 생성
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

        // 정규화
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
