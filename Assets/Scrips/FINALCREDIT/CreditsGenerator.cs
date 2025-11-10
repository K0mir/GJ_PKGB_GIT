using UnityEngine;
using UnityEngine.UI;
public class CreditsGenerator : MonoBehaviour
{
  [System.Serializable]
    public class CreditSection
    {
        public string sectionTitle;
        public string[] names;
    }
    
    public CreditSection[] creditSections;
    public GameObject titlePrefab;
    public GameObject namePrefab;
    public Transform contentParent;
    
    void Start()
    {
        GenerateCredits();
    }
    
    void GenerateCredits()
    {
        foreach (CreditSection section in creditSections)
        {
            // Crear título de sección
            GameObject title = Instantiate(titlePrefab, contentParent);
            title.GetComponent<Text>().text = section.sectionTitle;
            
            // Crear nombres
            foreach (string name in section.names)
            {
                GameObject nameObj = Instantiate(namePrefab, contentParent);
                nameObj.GetComponent<Text>().text = name;
            }
            
            // Espacio entre secciones
            GameObject space = new GameObject("Space");
            space.transform.SetParent(contentParent);
            space.AddComponent<RectTransform>().sizeDelta = new Vector2(100, 30);
        }
    }
}
