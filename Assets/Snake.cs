using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _len = 1;
    [SerializeField] private Transform[] _tails;

    private LinkedList<Transform> _tailesLinked;
    private float _timeModifier;
    
    private void Start()
    {
        _tailesLinked = new LinkedList<Transform>();
        _tailesLinked.AddLast(transform);
        Array.ForEach(_tails, t => _tailesLinked.AddLast(t));
        _timeModifier = _len / _speed;
    }
    
    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        LinkedListNode<Transform> node = _tailesLinked.First;
        while (node != null)
        {
            node.Value.position += node.Value.forward * Time.deltaTime * _speed;
            node = node.Next;
        }
    }
    
    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
            StartCoroutine(RotateTail(-90));

        if (Input.GetKeyDown(KeyCode.D)) 
            StartCoroutine(RotateTail(90));
    }

    private IEnumerator RotateTail(int angle)
    {
        int idx = 1;
        LinkedListNode<Transform> node = _tailesLinked.First;
        node.Value.Rotate(0, angle, 0);

        node = node.Next;
        while (node != null)
        {
            yield return new WaitForSeconds(_timeModifier);
            node.Value.Rotate(0, angle, 0);
            node = node.Next;
            idx += 1;
        }
    }
}