using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    StatueTextSO _textSO;

    public void Init(StatueTextSO textSO)
    {
		_textSO = textSO;
        _text.text = textSO.Text;
	}

    public void LookAt(Vector3 position)
    {
        transform.LookAt(position, Vector3.up);
		transform.Rotate(0, 180, 0);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
}
