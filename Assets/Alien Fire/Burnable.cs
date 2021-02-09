using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    [SerializeField] ParticleSystem fireVFX;
    [SerializeField] BurnDirection direction;
    [SerializeField] bool onFire;
    [SerializeField] float burnDelayMin = .1f;
    [SerializeField] float burnDelayMax = 2;
    [SerializeField] float burnDistance;
    [SerializeField] Material burntMaterial;
    float burnDelayTime;
    Vector3 burnDirection;
    bool startingToBurn = false;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(onFire)
        {
            StartBurning();
        }

        burnDelayTime = Random.Range(burnDelayMin, burnDelayMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(onFire)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, burnDirection, out hit, burnDistance))
            {
                Burnable objectToBurn = hit.transform.GetComponent<Burnable>();
                if (objectToBurn)
                {
                    objectToBurn.IsTouchingFire();
                }
            }
        }
    }

    private void StartBurning()
    {
        fireVFX.Play();
        rend.material = burntMaterial;
        onFire = true;
        if (direction == BurnDirection.Up)
        {
            burnDirection = Vector3.up;
        }
        else if (direction == BurnDirection.Down)
        {
            burnDirection = Vector3.down;
        }
        else if (direction == BurnDirection.Left)
        {
            burnDirection = Vector3.left;
        }
        else //Right
        {
            burnDirection = Vector3.right;
        }
    }

    public void IsTouchingFire()
    {
        if (!startingToBurn)
        {
            startingToBurn = true;
            StartCoroutine(BurnDelay());
        }
    }

    public IEnumerator BurnDelay()
    {
        yield return new WaitForSeconds(burnDelayTime);
        StartBurning();
    }
}

public enum BurnDirection
{
    Up,
    Down,
    Left,
    Right
}
