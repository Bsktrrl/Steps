using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] LevelState levelState;

    private void Update()
    {
        if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            OverWorldManager overworld = OverWorldManager.Instance;
            OverWorldStates saved = DataManager.Instance.overWorldStates_StoreList;
            if (overworld.levelState != levelState || saved == null ||
                saved.regionState != overworld.regionState || saved.levelState != levelState)
            {
                overworld.ChangeStates(overworld.regionState, levelState);
            }
        }
    }
}
