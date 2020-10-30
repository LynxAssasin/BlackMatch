using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Game.Tools;
using Game.Settings;
namespace Game.UI
{
    public class UITask : UIBase
    {
        [SerializeField]
        private GameObject prefabItem;
        private ElementsSettings elSettings; 
        private Dictionary<ColorElementType, GameObject> currentItems; 
        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            this.elSettings = elSettings;  
            prefabItem.SetActive(false);
            CommandKeeper.Instance.StartNewWave += StartNewTaskWave;
            CommandKeeper.Instance.UpdateTask += UpdateTasks;
        }

        public void StartNewTaskWave(List<LevelTask> newTasks)
        {

            if ((currentItems != null) && (currentItems.Count > 0))
            {
                foreach (var it in currentItems)
                {
                    GameObject.Destroy(it.Value); 
                }
                currentItems.Clear();
            }
            currentItems = new Dictionary<ColorElementType, GameObject>();

            prefabItem.SetActive(true);
            for (int i = 0; i < newTasks.Count; i++)
            {
                GameObject newObj = Instantiate(prefabItem, prefabItem.transform.parent);
                newObj.transform.localScale = prefabItem.transform.localScale;
                foreach (var element in elSettings.Elements)
                {
                    if (element is GameCore.ColorElement)
                    {
                        var clElement = ((GameCore.ColorElement)element);
                        if (clElement.Type == newTasks[i].element)
                        {
                            newObj.GetComponentInChildren<Image>().sprite = clElement.ElementSprite;
                            newObj.GetComponentInChildren<Text>().text = newTasks[i].count.ToString();
                            currentItems.Add(clElement.Type, newObj); 
                        }
                    }
                }
            }

            prefabItem.SetActive(false);

        }
        public void UpdateTasks(List<LevelTask> newTasks)
        {
            for (int i = 0; i < newTasks.Count; i++)
            {
                currentItems[newTasks[i].element].GetComponentInChildren<Text>().text = newTasks[i].count.ToString();
            }
        }

        protected void OnDestroy()
        {
            CommandKeeper.Instance.StartNewWave -= StartNewTaskWave;
            CommandKeeper.Instance.UpdateTask -= UpdateTasks;
        }
    }
}