using UnityEngine;
using UnityEngine.UI;

public class MachineProgressBar : MonoBehaviour
{
    [SerializeField] CoffeeMachineInteractable machine;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject root;
    [SerializeField] float showHideLerp = 10f;

    float _visible;

    void Update()
    {
        if (machine == null || fillImage == null || root == null) return;
        float target = machine.IsProcessing ? 1f : 0f;
        _visible = Mathf.MoveTowards(_visible, target, showHideLerp * Time.deltaTime);
        root.SetActive(_visible > 0.001f);
        fillImage.fillAmount = machine.IsProcessing ? machine.Progress01 : 0f;
    }
}
