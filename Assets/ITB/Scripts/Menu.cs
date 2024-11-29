using UnityEngine.InputSystem;

namespace UnityEngine.XR.Interaction.Toolkit.Inputs
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] GameObject menu;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnOpenMenu()
        {
            Debug.Log("OpenMenu");
            menu.SetActive(!menu.activeSelf);
        }
    }
}
