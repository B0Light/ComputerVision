using UnityEngine;
using UnityEngine.UI;

public class AddNoise : MonoBehaviour
{
     // 인스펙터 창에서 이미지를 드래그앤 드롭으로 할당
    public float noiseIntensity = 0.1f; // 노이즈 강도

    public Image InputImage;
    public Image OutputImage;// 필터링할 Image

    public void BTN_AddNoise()
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

            // Color 배열에 노이즈 추가
            for (int i = 0; i < pixels.Length; i++)
            {
                float noise = Random.Range(-noiseIntensity, noiseIntensity);

                pixels[i] = new Color(
                    Mathf.Clamp01(pixels[i].r + noise),
                    Mathf.Clamp01(pixels[i].g + noise),
                    Mathf.Clamp01(pixels[i].b + noise),
                    pixels[i].a
                );
            }

            // Color 배열을 Texture2D로 변환
            Texture2D noisyTexture = new Texture2D(width, height);
            noisyTexture.SetPixels(pixels);
            noisyTexture.Apply();

            // 결과 이미지를 Material의 MainTexture로 할당하거나 다른 방식으로 사용
            OutputImage.sprite = Sprite.Create(noisyTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }
}
