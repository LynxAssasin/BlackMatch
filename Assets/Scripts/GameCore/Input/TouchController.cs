using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.GameCore
{
    public class TouchController : MonoBehaviour
    {
        private bool enableToCheck;

        private void Start()
        {
            CommandKeeper.Instance.EnableToTouchBoard += Enable;
        }

        private void Enable(bool enable)
        {
            enableToCheck = enable;
        }

        void Update()
        {
            if ((enableToCheck)&&(!UISelected()))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CommandKeeper.Instance.BoardFingerDown();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                CommandKeeper.Instance.BoardFingerUp();
            }
        }

        private bool UISelected()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        private void OnDestroy()
        {
            CommandKeeper.Instance.EnableToTouchBoard -= Enable;
        }

    }
}