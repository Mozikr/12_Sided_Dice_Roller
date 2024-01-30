using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndRoll : MonoBehaviour
{
    public bool isThrown = false;

    private Camera mainCam;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 velocity = Vector3.zero;
    private GameController controller;

    [SerializeField] private InputAction mouseClick;
    [SerializeField] private float mouseDragPhysicSpeed = 10f;
    [SerializeField] private float mouseDragSpeed = 1f;
    [SerializeField] private Rigidbody diceRb;
    [SerializeField] private float minMagnitude = 10f;

    private void Awake()
    {
        controller = FindObjectOfType<GameController>();
        diceRb = FindObjectOfType<Rigidbody>();
        diceRb.useGravity = false;
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.Disable();
        mouseClick.performed -= MousePressed;
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Dice")) && !isThrown)
            {
                controller.isRoll = true;
                StartCoroutine(DragUpdate(hit.collider.gameObject));
                diceRb.useGravity = true;
            }
        }
    }

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCam.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        while (mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicSpeed;
                yield return waitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position,
                    ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                yield return null;
            }
        }

        isThrown = true;

        if (IsDiceSpeedTooLow())
        {
            controller.RestartDicePosition();
        }
    }

    private bool IsDiceSpeedTooLow()
    {
        return diceRb.velocity.magnitude <= minMagnitude;
    }
}
