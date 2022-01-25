//adapted from example script available at
//https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;

    Animator animator;
    public int aimLayer;
    public int hurtLayer;
    bool playerAim = true;

    private float layerWeightVelocity;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        aimLayer = animator.GetLayerIndex("Aiming");
        hurtLayer = animator.GetLayerIndex("hurt");

    }
    // Update is called once per frame
    void Update()
    {

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (translation != 0 || rotation != 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
        if(playerAim == true)
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 0f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", false);
            speed = 10.0F;
        }
        if (playerAim == false )
        {
            float currentAimLayerWeight = animator.GetLayerWeight(aimLayer);
            animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentAimLayerWeight, 1f, ref layerWeightVelocity, 0.2f));
            animator.SetBool("Aim", true);
            speed = 2.0F;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAim = !playerAim;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("GetDamage");
        }
    }
    void AfterGetHurt()
    {
        float currentHurtLayerWeight = animator.GetLayerWeight(hurtLayer);
        animator.SetLayerWeight(aimLayer, Mathf.SmoothDamp(currentHurtLayerWeight, 0f, ref layerWeightVelocity, 0.2f));
    }
}
