using UnityEngine;

[CreateAssetMenu(fileName = "StatueTextSO", menuName = "StatueText")]
public class StatueTextSO : ScriptableObject
{
    [TextArea(minLines:10, maxLines:100)]
	public string Text;
}
