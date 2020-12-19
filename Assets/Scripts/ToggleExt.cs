using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ToggleExt
{
    //トグルをよぶときに、メソッドを呼び出さずにONにする。
    public static void SetIsOnWithoutCallback(this Toggle self, bool isOn)
    {
        var onValueChanged = self.onValueChanged;
        self.onValueChanged = new Toggle.ToggleEvent();
        self.isOn = isOn;
        self.onValueChanged = onValueChanged;
    }
}
