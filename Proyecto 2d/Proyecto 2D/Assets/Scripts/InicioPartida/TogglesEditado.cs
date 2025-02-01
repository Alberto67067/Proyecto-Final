using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; 
public class CustomToggleGroup : MonoBehaviour 
{ 
    public List<Toggle> NumofCharacter;
    public List<Toggle> toggles; 
    public int MaxToggles = 4; 
    private Queue<Toggle> selectedToggles = new Queue<Toggle>(); 
    private bool cambio = false;
    void Start() 
    {
        foreach (var toggle in toggles) 
        { 
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); }); 
        } 
    }
    void Update()
    {
        int togAct = MaxToggles;
        for (int i = 0; i < 2; i++)
        {
            if(NumofCharacter[i].name == "2" && NumofCharacter[i].isOn)
            {
                MaxToggles = 2;
            }
            if(NumofCharacter[i].name == "4" && NumofCharacter[i].isOn)
            {
                MaxToggles = 4;
            }  
        }
        if(togAct != MaxToggles)
        {
            cambio = true;
        }
        if (cambio)
        {
            foreach(var toggle in toggles)
            {
                toggle.isOn = false;
            }
        }
        cambio = false; 
    }
    void OnToggleValueChanged(Toggle toggle) 
    { 
        if (toggle.isOn) 
        { 
            selectedToggles.Enqueue(toggle); 
            // Si hay más toggles seleccionados que el número máximo, destecla el más antiguo 
            if (selectedToggles.Count > MaxToggles) 
            { 
                Toggle t = selectedToggles.Dequeue(); 
                t.isOn = false;
            } 
        } 
        else 
        { 
            // Quita el toggle desteclado de la cola 
            if (selectedToggles.Contains(toggle)) 
            { 
                Queue<Toggle> tempQueue = new Queue<Toggle>(); 
                while (selectedToggles.Count > 0) 
                { 
                Toggle t = selectedToggles.Dequeue();
                if (t != toggle) 
                { 
                    tempQueue.Enqueue(t); 
                } 
            } 
            selectedToggles = tempQueue; 
            } 
        } 
    } 
}
