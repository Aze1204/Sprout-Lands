using UnityEngine;
using UnityEngine.UI;

namespace Cursor
{
    public class CursorVisuals : MonoBehaviour
    {
        [Header("Sprites")] 
        public Sprite normal;
        public Sprite point;
        public Sprite hold;

        private Image cursorImage;
        private Image buildImage;

        public void Initialize(Image cursorImg, Image buildImg)
        {
            cursorImage = cursorImg;
            buildImage = buildImg;
            buildImage.gameObject.SetActive(false);
            SetCursorImage(normal);
        }

        public void SetCursorImage(Sprite sprite)
        {
            cursorImage.sprite = sprite;
            cursorImage.color = new Color(1, 1, 1, 1);
        }

        public void SetCursorValid()
        {
            cursorImage.color = new Color(1, 1, 1, 1);
        }

        public void SetCursorInvalid()
        {
            cursorImage.color = new Color(1, 1, 1, 0.5f);
        }
    }
}