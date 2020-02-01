using UnityEngine;

namespace Assets.Scripts.Cam.Effects {
	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.Camera))]
	[AddComponentMenu("Image Effects/Custom/Retro Pixel Max")]
	public class RetroPixelMax : MonoBehaviour {
		[Header("Colors")]
		[ColorUsageAttribute(true, true)] public Color[] colors;

		protected Material m_material;
		protected Shader shader;

		protected Material material {
			get {
				return m_material;
			}
		}

		protected void Awake() {
			if (m_material == null) {
				shader = Shader.Find("Oxysoft/RetroPixelMax");
				m_material = new Material(shader) { hideFlags = HideFlags.DontSave };
			}
		}

		public void OnRenderImage(RenderTexture src, RenderTexture dest) {
			if (material && colors.Length > 0) {
				material.SetInt("_ColorCount", colors.Length);
				material.SetColorArray("_Colors", colors);

				Graphics.Blit(src, dest, material);
			} else {
				Graphics.Blit(src, dest);
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