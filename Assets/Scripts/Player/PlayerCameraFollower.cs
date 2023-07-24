using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollower : Singleton<PlayerCameraFollower>
{
    private enum YRotation { Followed, None }

    [SerializeField]
    private YRotation rotation = YRotation.None;

    private PlayerAimer followedPlayer = null;

    private void Start()
    {
        if (ClientProvider.Instance.IsInitialized)
        {
            RegisterPlayer();
        }
        else
        {
            ClientProvider.Instance.OnInitializeEvent += (_, _) => RegisterPlayer();
        }
    }

    private void RegisterPlayer()
    {
        followedPlayer = ClientProvider.Instance.ClientPlayer.GetComponent<PlayerAimer>();
    }

    private void LateUpdate()
    {
        if (followedPlayer == null)
        {
            return;
        }

        Vector3 position = followedPlayer.transform.position;
        position.y = transform.position.y;
        transform.position = position;

        float yRotation = 0;
        if (rotation == YRotation.Followed)
        {
            yRotation = followedPlayer.YRotation;
        }

        transform.rotation = Quaternion.Euler(90, yRotation, 0);
    }
}
