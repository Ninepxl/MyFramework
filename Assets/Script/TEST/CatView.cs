using UnityEngine;
using UnityEngine.UI;

public sealed class CatView : MonoBehaviour
{
    Text levelText;
    Button levelUpButton;
    // public void Initialize(CatViewModel viewModel)
    // {
    //     ApplyLevel(viewModel.Cmd.GetLevel());
    //     viewModel.Model.Subsecibe(model =>
    //     {
    //         ApplyLevel(model.Level);
    //         PlayDirection();
    //     }).AddTo(this);
    // }
    public void ApplyLevel(int level)
    {
        levelText.text = $"LV: {level}";
    }
    public void PlayDirection()
    {

    }
}