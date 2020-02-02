using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour {
    public Team team;
    public float moveSpeed = 40f;
    public float moveSpeedWithItem = 30f;

    public float rotationSpeed = 10f;
    public float rotationSpeedWithItem = 5f;

    public float throwForce = 10f;
    public float pickUpSpeed = 20f;

    public float maxPlayerToItemDst = 0.5f;
    public Transform pickUpPoint;
    public Transform carryPoint;

    public SkinnedMeshRenderer visual;
    public Animator animator;
    private int moveHash;

    public LayerMask itemLayer;
    public LayerMask machineLayer;

    private Rigidbody rb;
    private Transform tr;

    private Item currentItem;
    private bool pickupComplete;

    private Vector3 dashVector;
    public float dashForce = 10f;
    public float dashFalloff = 2f;

    private bool inputDisabled;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        inputDisabled = true;

        moveHash = Animator.StringToHash("MoveSpeed");
    }

    public void Init() {
        inputDisabled = false;
    }

    public void Disable() {
        if(currentItem) {
            currentItem.PutDown(pickUpPoint.position);
            currentItem = null;
        }
        inputDisabled = true;
    }

    public void PlayAnim(string anim) {
        animator.SetTrigger(anim);
    }

    public void Move(Vector3 moveVector) {
        if(inputDisabled) {
            return;
        }

        if(currentItem) {
            Vector3 newVelocity = moveVector * 10f * moveSpeedWithItem * Time.fixedDeltaTime;
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;

            rb.MoveRotation(Quaternion.LookRotation(Vector3.Slerp(tr.forward, moveVector, rotationSpeedWithItem * Time.fixedDeltaTime), Vector3.up));
        } else {
            Vector3 newVelocity = moveVector * 10f * moveSpeed * Time.fixedDeltaTime + dashVector;
            newVelocity.y = rb.velocity.y;
            rb.velocity = newVelocity;

            rb.MoveRotation(Quaternion.LookRotation(Vector3.Slerp(tr.forward, moveVector, rotationSpeed * Time.fixedDeltaTime), Vector3.up));
        }
    }

    void Update() {
        if(dashVector.sqrMagnitude > 0.01f) {
            dashVector = Vector3.Lerp(dashVector, Vector3.zero, dashFalloff * Time.deltaTime);

            if(dashVector.sqrMagnitude < 0.01f) {
                dashVector = Vector3.zero;
            }
        }

        //animator.SetFloat(moveHash, rb.velocity.sqrMagnitude);
    }

    void FixedUpdate() {
        if(inputDisabled) {
            return;
        }

        if(!pickupComplete && currentItem) {
            currentItem.transform.position = Vector3.Lerp(currentItem.transform.position, carryPoint.position, Time.fixedDeltaTime * pickUpSpeed);
            currentItem.transform.rotation = Quaternion.Lerp(currentItem.transform.rotation, carryPoint.rotation, Time.fixedDeltaTime * pickUpSpeed * 2f);

            if((currentItem.transform.position - carryPoint.position).sqrMagnitude < 0.015f) {
                pickupComplete = true;
                currentItem.transform.position = carryPoint.position;
                currentItem.transform.rotation = carryPoint.rotation;
            }
        }
    }

    public void Dash(Vector3 dir) {
        if(inputDisabled) {
            return;
        }

        if(currentItem || dashVector.sqrMagnitude > 0.1f) {
            return;
        }
        dashVector = dir * dashForce;
    }

    public void ItemInteraction() {
        if(inputDisabled) {
            return;
        }

        if(currentItem == null) {
            currentItem = GetClosestItem();

            if(currentItem) {
                pickupComplete = false;
                currentItem.PickedUp(tr);
            } else {
                ConveyorLever lever = GetClosestLever();

                if(lever) {
                    lever.Trigger(team);
                }
            }
        } else {
            Machine machine = GetClosestMachine();

            if(machine) {
                if(machine.Use(team, currentItem)) {
                    currentItem = null;
                }
            } else {
                currentItem.PutDown(pickUpPoint.position);
                currentItem = null;
            }
        }
    }

    public void ThrowItem() {
        if(currentItem == null) {
            return;
        }

        currentItem.Thrown(rb.velocity + (tr.forward + tr.up * 0.5f).normalized * throwForce);
        currentItem = null;
    }

    private Item GetClosestItem() {
        Collider[] colliders = Physics.OverlapBox(pickUpPoint.position, pickUpPoint.localScale * 0.5f, Quaternion.identity, itemLayer);

        float closest = 1000f;
        int closestIndex = -1;
        for(int i = 0; i < colliders.Length; i++) {
            float newDist = (pickUpPoint.position - colliders[i].transform.position).sqrMagnitude;
            if(newDist < closest) {
                closest = newDist;
                closestIndex = i;
            }
        }

        if(closestIndex != -1) {
            if(colliders[closestIndex].transform.parent == null) {
                return colliders[closestIndex].transform.GetComponent<Item>();
            }
            return colliders[closestIndex].transform.parent.GetComponent<Item>();
        }

        return null;
    }

    private Machine GetClosestMachine() {
        Collider[] colliders = Physics.OverlapBox(pickUpPoint.position, pickUpPoint.localScale * 0.5f, Quaternion.identity, machineLayer);

        float closest = 1000f;
        int closestIndex = -1;
        for(int i = 0; i < colliders.Length; i++) {
            float newDist = (pickUpPoint.position - colliders[i].transform.position).sqrMagnitude;
            if(newDist < closest && colliders[i].GetComponent<Machine>()) {
                closest = newDist;
                closestIndex = i;
            }
        }

        if(closestIndex != -1) {
            return colliders[closestIndex].transform.GetComponent<Machine>();
        }

        return null;
    }

    private ConveyorLever GetClosestLever() {
        Collider[] colliders = Physics.OverlapBox(pickUpPoint.position, pickUpPoint.localScale * 0.5f, Quaternion.identity, machineLayer);

        float closest = 1000f;
        int closestIndex = -1;
        for(int i = 0; i < colliders.Length; i++) {
            float newDist = (pickUpPoint.position - colliders[i].transform.position).sqrMagnitude;
            if(newDist < closest) {
                closest = newDist;
                closestIndex = i;
            }
        }

        if(closestIndex != -1) {
            return colliders[closestIndex].transform.GetComponent<ConveyorLever>();
        }

        return null;
    }
}
