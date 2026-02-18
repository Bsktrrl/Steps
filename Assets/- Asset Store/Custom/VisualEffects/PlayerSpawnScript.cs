using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
    [SerializeField] ParticleSystem PS1;
    [SerializeField] ParticleSystem PS2;
    [SerializeField] GameObject playerBody;
    bool spawning;
    bool spawnCorrection0;
    bool spawnCorrection1;
    bool spawnCorrection2;
    bool spawnCorrection3;

    Vector3 oversize0 = new Vector3(0.1f, 2.5f, 0.1f);
    Vector3 oversize1 = new Vector3(0.75f, 1.5f, 0.75f);
    Vector3 oversize2 = new Vector3(1.125f, 0.75f, 1.125f);
    Vector3 oversize3 = new Vector3(0.95f, 1.1f, 0.95f);

    float spawnSpeed0 = 10f;
    float spawnSpeed1 = 5f;
    float spawnSpeed2 = 4.5f;
    float spawnSpeed3 = 4f;
    float spawnSpeed4 = 3.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Spawn());
        }

        if (spawning)
        {
            playerBody.transform.localScale = Vector3.Lerp(playerBody.transform.localScale, oversize0, Time.deltaTime * spawnSpeed0);
        }

        if (spawnCorrection0)
        {
            playerBody.transform.localScale = Vector3.Lerp(playerBody.transform.localScale, oversize1, Time.deltaTime * spawnSpeed1);
        }

        if (spawnCorrection1)
        {
            playerBody.transform.localScale = Vector3.Lerp(playerBody.transform.localScale, oversize2, Time.deltaTime * spawnSpeed2);
        }

        if (spawnCorrection2)
        {
            playerBody.transform.localScale = Vector3.Lerp(playerBody.transform.localScale, oversize3, Time.deltaTime * spawnSpeed3);
        }

        if (spawnCorrection3)
        {
            playerBody.transform.localScale = Vector3.Lerp(playerBody.transform.localScale, Vector3.one, Time.deltaTime * spawnSpeed4);
        }
    }

    IEnumerator Spawn()
    {
        playerBody.transform.localScale = Vector3.zero;
        PS1.Play();

        yield return new WaitForSeconds(5);

        PS2.Play();
        spawning = true;

        yield return new WaitForSeconds(0.1f);

        spawning = false;
        spawnCorrection0 = true;

        yield return new WaitForSeconds(0.1f);

        spawnCorrection0 = false;
        spawnCorrection1 = true;

        yield return new WaitForSeconds(0.33f);

        spawnCorrection1 = false;
        spawnCorrection2 = true;

        yield return new WaitForSeconds(0.33f);

        spawnCorrection2 = false;
        spawnCorrection3 = true;

        yield return new WaitForSeconds(1);

        spawnCorrection3 = false;
        playerBody.transform.localScale = Vector3.one;
    }
}
