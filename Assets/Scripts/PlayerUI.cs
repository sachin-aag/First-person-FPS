using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;
    private PlayerController controller;
    public void SetPlayerController(PlayerController _controller)

    {
        controller = _controller;

    }
    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
    private void Update()
    {
        if (controller != null)
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());
        }
    }
}
