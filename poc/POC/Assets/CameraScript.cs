using UnityEngine;

public class CameraScript : MonoBehaviour
{

    bool isClic = false;
    int n = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isClic == true)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(Mathf.Round(cursorPos.x) - 0.5f, Mathf.Round(cursorPos.y) - 0.5f);

            if (Input.GetMouseButtonDown(1))
            {
                transform.Rotate(0, 0, 90, Space.World);
            }

        }

    }

    void OnMouseDown()
    {
        n++;

        if (n % 2 == 0)
        {
            isClic = false;
        }
        else
        {
            isClic = true;
        }
    }
}
