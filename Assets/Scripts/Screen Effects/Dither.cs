using UnityEngine;

namespace Assets.Scripts.Cam.Effects {
	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.Camera))]
	[AddComponentMenu("Image Effects/Custom/Dither")]
	public class Dither : MonoBehaviour {
		public Texture2D pattern;

		[Range(0.0f, 1.0f)]
		public float threshold = 0.45f;
		[Range(0.0f, 1.0f)]
		public float strength = 0.45f;
		public int width = 5;
		public int height = 5;

		private Material m_material;
		private Shader shader;

		private Material material {
			get {
				if (m_material == null) {
					shader = Shader.Find("Oxysoft/Dither");
					m_material = new Material(shader) { hideFlags = HideFlags.DontSave };
				}

				return m_material;
			}
		}

		public void OnRenderImage(RenderTexture src, RenderTexture dest) {
			if (material) {
				material.SetTexture("_Dither", pattern);
				material.SetInt("_Width", pattern.width);
				material.SetInt("_Height", pattern.height);
				material.SetFloat("_Threshold", threshold);
				material.SetFloat("_Strength", strength);
				RenderTexture scaled = RenderTexture.GetTemporary(width, height);
				scaled.filterMode = FilterMode.Point;
				Graphics.Blit(src, scaled);
				Graphics.Blit(scaled, dest, material);
				// Graphics.Blit(scaled, dest);
				RenderTexture.ReleaseTemporary(scaled);
			}
		}

		private void OnDisable() {
#if UNITY_EDITOR
			if (m_material)
				DestroyImmediate(m_material);
#else
			if (m_material)
				Destroy(m_material);
#endif
		}
	}
}