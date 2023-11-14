using UnityEngine;
using UnityEngine.UI;

public class AddNoise : MonoBehaviour
{
     // �ν����� â���� �̹����� �巡�׾� ������� �Ҵ�
    public float noiseIntensity = 0.1f; // ������ ����

    public Image InputImage;
    public Image OutputImage;// ���͸��� Image

    public void BTN_AddNoise()
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

            // Color �迭�� ������ �߰�
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

            // Color �迭�� Texture2D�� ��ȯ
            Texture2D noisyTexture = new Texture2D(width, height);
            noisyTexture.SetPixels(pixels);
            noisyTexture.Apply();

            // ��� �̹����� Material�� MainTexture�� �Ҵ��ϰų� �ٸ� ������� ���
            OutputImage.sprite = Sprite.Create(noisyTexture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        }
    }
}
