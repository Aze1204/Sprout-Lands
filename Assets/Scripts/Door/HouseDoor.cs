using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
// ���������Ź��š�
// �����ߣ�Aze
// ����ʱ�䣺2025-01-23
public class HouseDoor : MonoBehaviour
{
    private bool isOpenDoor;
    private bool isInTrigger;
    public GameObject block;
    private Animator animator;

    private void Awake()
    {
        isOpenDoor = false;
        isInTrigger = false;
        animator = GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isInTrigger = false;
    }

    private void Update()
    {
        if (isInTrigger && Input.GetKeyUp(KeyCode.E))
        {
            isOpenDoor = !isOpenDoor;
            animator.SetBool("isOpenDoor", isOpenDoor);
            block.SetActive(!isOpenDoor);
            Debug.Log(block.name);
            Debug.Log("Door");
        }
    }
}
