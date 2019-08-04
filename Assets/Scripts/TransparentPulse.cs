using UnityEngine;

public class TransparentPulse : MonoBehaviour {

    public float period = 1;
    [Range(0, 1)]
    public float minValue = 0;
    [Range(0, 1)]
    public float maxValue = 1;

    private Material material;

    private void Start() {
        material = GetComponent<MeshRenderer>().material;
    }
    private void Update() {
        if (minValue > maxValue) {
            Debug.LogError("Maxvalue must be greater than minvalue!");
        } else {
            Color color = material.color;
            color.a = minValue + (maxValue - minValue) * (Mathf.Sin(Time.time * 2 * Mathf.PI / period) * 0.5f + 1f);
            material.color = color;
        }
    }

}
