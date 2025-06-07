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
			rotX -= 360; // Normalize to -180 to 180 range
		}
		// Clamp the X rotation to a range of -20 to 20 degrees

		var clampX = Mathf.Clamp(rotX, -20, 20);

        transform.eulerAngles = new Vector3(clampX, transform.eulerAngles.y, 0);
	}

    public bool IsSeen()
    {
        var screenPt = Camera.main.WorldToScreenPoint(transform.position);
        return screenPt.x > 0 && screenPt.y > 0 && screenPt.x < Screen.width && screenPt.y < Screen.height;
    }

    void WriteToDialog()
    {
		if (isActiveAndEnabled && IsSeen() && _lastCharDisplayed < _textSO.Text.Length)
		{
			_text.text += _textSO.Text[_lastCharDisplayed++];

            if (_text.isTextOverflowing)
            {
				//remove the first row
				var lines = _text.text.Split('\n');
				if (lines.Length > 1)
				{
					_text.text = string.Join("\n", lines.Skip(1));
					//_lastCharDisplayed -= lines[0].Length + 1; // +1 for the newline character
				}
				else
				{
					_text.text = ""; // If only one line, clear it
					//_lastCharDisplayed = 0;
				}
			}
		}
	}
}
