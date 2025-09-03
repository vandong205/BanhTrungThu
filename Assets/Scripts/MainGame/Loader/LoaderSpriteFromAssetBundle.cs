using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace InPlay.KiemSonMobile.Utilities.UnityUI
{
    /// <summary>
    /// Tải ảnh từ AssetBundle trong StreamingAssets, đặt vào đối tượng UnityEngine.UI.Image hoặc UnityEngine.SpriteRenderer
    /// </summary>
    [ExecuteAlways]
    public class SpriteFromAssetBundle : MonoBehaviour
    {
        #region Defines
        [SerializeField] private string _BundleDir;     // Tên file bundle (trong StreamingAssets)
        [SerializeField] private string _AtlasName;     // Tên Atlas chứa Sprite
        [SerializeField] private string _SpriteName;    // Tên Sprite
        [SerializeField] private bool _PixelPerfect;
        [SerializeField] private bool _PixelPerfectOnPlay;
#if UNITY_EDITOR
        [SerializeField] private bool _LoadImmedatelly;
#endif
        [SerializeField] private float _Scale = 1f;

        private string lastBundleDir = "";
        private AssetBundle currentBundle;
        #endregion

        #region Properties
        public string BundleDir { get => _BundleDir; set => _BundleDir = value; }
        public string AtlasName { get => _AtlasName; set => _AtlasName = value; }
        public string SpriteName { get => _SpriteName; set => _SpriteName = value; }
        public bool PixelPerfect { get => _PixelPerfect; set => _PixelPerfect = value; }
        public float Scale { get => _Scale; set => _Scale = value; }
        #endregion

        #region Public methods
        public void Load()
        {
            if (string.IsNullOrEmpty(_BundleDir) || string.IsNullOrEmpty(_AtlasName) || string.IsNullOrEmpty(_SpriteName))
                return;

            // Unload bundle cũ nếu có
            if (!string.IsNullOrEmpty(lastBundleDir) && currentBundle != null)
            {
                currentBundle.Unload(false);
                currentBundle = null;
            }

            // Đường dẫn bundle mới
            string bundlePath = System.IO.Path.Combine(Application.streamingAssetsPath, _BundleDir);

            currentBundle = AssetBundle.LoadFromFile(bundlePath);
            if (currentBundle == null)
            {
                Debug.LogError("Không thể load AssetBundle tại -> " + bundlePath);
                return;
            }

            lastBundleDir = _BundleDir;

            // Lấy sprite từ Atlas
            Sprite[] sprites = currentBundle.LoadAssetWithSubAssets<Sprite>(_AtlasName);
            Sprite targetSprite = null;
            foreach (Sprite s in sprites)
            {
                if (s.name == _SpriteName)
                {
                    targetSprite = s;
                    break;
                }
            }

            if (targetSprite == null)
            {
                Debug.LogError($"Không tìm thấy Sprite {_SpriteName} trong Atlas {_AtlasName}");
                return;
            }

            // Nếu là UIImage
            RectTransform transform = this.gameObject.GetComponent<RectTransform>();
            if (transform != null)
            {
                UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.sprite = targetSprite;

                    if (_PixelPerfect)
                    {
                        transform.sizeDelta = image.sprite.rect.size * _Scale;
                    }
                }
            }
            // Nếu là SpriteRenderer
            else
            {
                SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = targetSprite;
                    renderer.drawMode = SpriteDrawMode.Sliced;

                    if (_PixelPerfect)
                    {
                        renderer.size = renderer.sprite.rect.size * _Scale;
                    }
                }
            }
        }
        #endregion

        #region Core MonoBehaviour
        private void OnDisable()
        {
            if (!string.IsNullOrEmpty(lastBundleDir) && currentBundle != null)
            {
                currentBundle.Unload(false);
                currentBundle = null;
                lastBundleDir = null;
            }
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                _PixelPerfect = _PixelPerfectOnPlay;
                Load();
            }
#if UNITY_EDITOR
            else if (Application.isEditor)
            {
                Editor_Load();
            }
#endif
        }
        #endregion

#if UNITY_EDITOR
        private void Editor_Load()
        {
            if (!string.IsNullOrEmpty(_BundleDir) && !string.IsNullOrEmpty(_AtlasName) && !string.IsNullOrEmpty(_SpriteName))
            {
                string url = System.IO.Path.Combine(Application.streamingAssetsPath, _BundleDir);

                UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
                www.SendWebRequest();

                while (!www.isDone) { }

                if (string.IsNullOrEmpty(www.error))
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    if (bundle == null)
                    {
                        Debug.LogError("Không tìm thấy Bundle tại -> " + url);
                        return;
                    }

                    UnityEngine.Sprite[] sprites = bundle.LoadAssetWithSubAssets<UnityEngine.Sprite>(_AtlasName);

                    foreach (UnityEngine.Sprite sprite in sprites)
                    {
                        if (sprite.name == _SpriteName)
                        {
                            RectTransform transform = this.gameObject.GetComponent<RectTransform>();
                            if (transform != null)
                            {
                                UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
                                if (image != null)
                                {
                                    image.sprite = sprite;
                                }
                            }
                            else
                            {
                                SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                                if (renderer != null)
                                {
                                    renderer.sprite = sprite;
                                    renderer.drawMode = SpriteDrawMode.Sliced;
                                }
                            }
                        }
                    }

                    bundle.Unload(false);
                }
                else
                {
                    Debug.LogError("Lỗi tải Bundle -> " + www.error);
                }
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                if (_LoadImmedatelly)
                {
                    Editor_Load();
                    _LoadImmedatelly = false;
                }

                if (_PixelPerfect)
                {
                    RectTransform transform = this.gameObject.GetComponent<RectTransform>();
                    if (transform != null)
                    {
                        UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
                        if (image != null)
                        {
                            transform.sizeDelta = image.sprite == null ? Vector2.zero : image.sprite.rect.size * _Scale;
                        }
                    }
                    else
                    {
                        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
                        if (renderer != null && renderer.sprite != null)
                        {
                            renderer.size = renderer.sprite.rect.size * _Scale;
                        }
                    }

                    _PixelPerfect = false;
                }
            }
        }
#endif
    }
}
