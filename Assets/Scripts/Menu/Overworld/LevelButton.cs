using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] LevelState levelState;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            OverWorldManager.Instance.ChangeStates(OverWorldManager.Instance.regionState, levelState);
        }
    }
}
