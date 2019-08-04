using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float speed = 5;

    private void Update() {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.Tags.ENEMY)) {
            other.GetComponent<Enemy>().TakeDamage();
            Destroy(gameObject);
        }
    }

}
