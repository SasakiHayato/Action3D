using UnityEngine;

public class RadialBlur : MonoBehaviour
{
    [SerializeField] Shader _shader;
    [SerializeField, Range(4, 16)] int _sampleCount = 8;
    [SerializeField, Range(0, 1)] float _strenght = 0.5f;
    public float SetStrength { set { _strenght = value; } }

    Material _material = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            if (_shader == null)
            {
                Graphics.Blit(source, destination);
                return;
            }
            else
            {
                _material = new Material(_shader);
            }
        }

        _material.SetInt("_SampleCount", _sampleCount);
        _material.SetFloat("_Strength", _strenght);
        Graphics.Blit(source, destination, _material);
    }
}
