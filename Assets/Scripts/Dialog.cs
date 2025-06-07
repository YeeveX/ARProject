using System.Linq;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    StatueTextSO _textSO;

    int _lastCharDisplayed = 0;

    public void Init(StatueTextSO textSO)
    {
		_textSO = textSO;
        _text.text = "";
        InvokeRepeating(nameof(WriteToDialog), 0, 0.07f);
	}

    public void LookAt(Vector3 position)
    {
        transform.LookAt(position, Vector3.up);
		transform.Rotate(0, 180, 0);

		var rotX = transform.eulerAngles.x;
		if (rotX > 180)
		{
			rotX -= 360;
		}

		var clampX = Mathf.Clamp(rotX, -20, 20);

        transform.eulerAngles = new Vector3(clampX, transform.eulerAngles.y, 0);
	}

    public bool IsSeen()
    {
        var screenPt = Camera.main.WorldToScreenPoint(transform.position);
        var quarterW = Screen.width / 4;
		var quarterH = Screen.height / 4;

		return screenPt.x > quarterW && screenPt.y > quarterH && screenPt.x < Screen.width-quarterW && screenPt.y < Screen.height-quarterH;
    }

    void WriteToDialog()
    {
		if (isActiveAndEnabled && IsSeen() && _lastCharDisplayed < _textSO.Text.Length)
		{
			_text.text += _textSO.Text[_lastCharDisplayed++];
		}
	}
}
