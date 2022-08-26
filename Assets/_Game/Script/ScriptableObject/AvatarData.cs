using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Avatar",menuName = "Avatar")]
public class AvatarData : ScriptableObject
{
    public List<Avatar> listAvatar;
}

[System.Serializable]
public class Avatar
{
    public Sprite avatar;
}
