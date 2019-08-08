using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI currencyText;

	public void updateCurrency(int currency) {currencyText.text = currency.ToString();}
}
