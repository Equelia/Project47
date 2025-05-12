using UnityEngine;

namespace Project47
{
	[CreateAssetMenu(menuName = "Project47/Character/CharacterSettings", fileName = "CharacterSettings")]
	public partial class CharacterSettings : ScriptableObject
	{
		[Header("Properties - Movement")]
		[SerializeField()] public float movementSpeed;
		[SerializeField()] public float movementSpeedShift;

		[Header("Properties - Jump")]
		[SerializeField()] public float jumpForce;
	}
}