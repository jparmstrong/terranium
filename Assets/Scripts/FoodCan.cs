using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCan : MonoBehaviour
{
    public Transform foodParent;
    public GameObject mealwormPrefab;

    bool feeding = false;
    bool first_click = false;
    float turn_height = 4;

    public Vector3 worldPosition;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.transform.position;
    }

    private void OnMouseDown()
    {
        if (!feeding)
        {
            feeding = true;
            first_click = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (feeding && Input.GetKeyDown(KeyCode.Mouse1))
        {
            feeding = !feeding;
            if (!feeding)
            {
                this.transform.position = initialPosition;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if(feeding)
        {
            if (first_click)
            {
                first_click = false;
                return;
            }

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane + 16;

            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;

            float interpRatio = (float)System.Math.Max(System.Math.Min(worldPosition.y, turn_height), 0) / turn_height;
            transform.rotation = Quaternion.Euler(-100f * interpRatio, 0, 0);

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.F))
            {
                GameObject mw = Instantiate(mealwormPrefab, this.transform.position + Vector3.back * (this.transform.lossyScale.y / 2), Quaternion.identity);
                Rigidbody rb = mw.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), ForceMode.Impulse);
                mw.transform.parent = foodParent;
            }
        }

    }
}
