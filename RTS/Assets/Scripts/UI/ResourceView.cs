using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {
  public Text CurrentLabel;
  public Text MaxLabel;

  void setCurrent(int value) {
    CurrentLabel.text = value.ToString();
  }

  void setMax(int value) {
    MaxLabel.text = value.ToString();
  }
}
