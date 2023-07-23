using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirX < 0 && dirY == 0) { // Left
            scale.x *= -1;
            scale.y *= -1;
        }
        else if(dirX == 0 && dirY < 0) { // Down
            scale.y *= -1;
        }
        else if(dirX == 0 && dirY > 0) { // Up
            scale.x *= -1;
        }
        else if(dirX > 0 && dirY > 0) { // Right Up
            rotation.z = 0f;
        }
        else if(dirX > 0 && dirY < 0) { // Right Down
            rotation.z = -90f;
        }
        else if(dirX < 0 && dirY > 0) { // Left Up
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if(dirX < 0 && dirY < 0) { // Left Down
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

}
