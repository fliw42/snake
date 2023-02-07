using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            LinkedListNode<Transform> last = _tailesLinked.Last;
            _tailesLinked.RemoveLast();
            LinkedListNode<Transform> body = _tailesLinked.Last;
            Transform newTransform = Instantiate(body.Value, body.Value.position - body.Value.forward * _len, body.Value.rotation);
            last.Value.position -= body.Value.forward * _len;
            _tailesLinked.AddLast(newTransform);
            _tailesLinked.AddLast(last);
            var position = new Vector3(Random.Range(-2f, 12.4f), 0.5f, Random.Range(15f, -24.6f));

            // что, где, какой-поворот
            Instantiate(other.gameObject, position, Quaternion.identity);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            foreach (var t in _tailesLinked)
            {
                Destroy(t.gameObject);
            }
        }
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