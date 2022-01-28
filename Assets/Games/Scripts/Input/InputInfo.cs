using UnityEngine;

public static class InputInfo
{
    public static (Vector2 Normal, Vector2 Raw) PlayerAxis { get { return (new Vector2(PlayerHorizontal.Normal, PlayerVertical.Normal), new Vector2(PlayerHorizontal.Raw, PlayerVertical.Raw)); } }
    public static (float Normal, float Raw) PlayerHorizontal { get { return (Input.GetAxis("PlayerHorizontal"), Input.GetAxisRaw("PlayerHorizontal")); } }
    public static (float Normal, float Raw) PlayerVertical { get { return (Input.GetAxis("PlayerVertical"), Input.GetAxisRaw("PlayerVertical")); } }
    public static (bool OnDown, bool Held, bool OnRelease) PlayerJump { get { return (Input.GetButtonDown("PlayerJump"), Input.GetButton("PlayerJump"), Input.GetButtonUp("PlayerJump")); } }
}
