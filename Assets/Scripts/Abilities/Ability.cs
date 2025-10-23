using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ActivateAbility());
    }
    protected virtual IEnumerator ActivateAbility()
    {
        return null;
    }
}
