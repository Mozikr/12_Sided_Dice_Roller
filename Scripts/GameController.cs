using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public bool isRoll = false;
    
    private const float dicePositionDesk = 0.8f;
    private const float dicePositionOffDesk = -2f;
    private const float invokeWaitTime = 2f;
    private const float timeToBump = 6f;
    private const float bumpForceMultiplier = 5f;
    
    private Vector3 diceStartPosition = new Vector3(-2, 2, -4);
    private Quaternion diceRotationZero = Quaternion.Euler(0, 0, 0);
    private Rigidbody diceRb;
    private Transform diceTransform;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    private Vector3 force;
    private bool addPoints = true;
    private bool isRestarted = false;
    private float bumpTimer = 0f;
    private DisplayPanel displayPanel;
    private DragAndRoll dragAndRoll;

    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject restartText;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private int bumpMinRandomValue = 15;
    [SerializeField] private int bumpMaxRandomValue = 30;
    
    private void Start()
    {
        diceTransform = dice.GetComponent<Transform>();
        diceRb = dice.GetComponent<Rigidbody>();
        displayPanel = FindObjectOfType<DisplayPanel>();
        dragAndRoll = FindObjectOfType<DragAndRoll>();
    }
    private void Update()
    {
        if (isDiceLayingOnDesk() && !isRestarted)
        {
            isRestarted = true;
            displayPanel.DisplayResult();
            Invoke("RestartDicePosition", invokeWaitTime);
            if (addPoints)
            {
                displayPanel.AddPoints();
                addPoints = false;
            }
        }
        else if (IsDiceOffDesk())
        {
            StartCoroutine(RestartDicePositionAfterFall());
        }

        if(diceRb.velocity.y > 0f)
        {
            displayPanel.DisplayResultInMove();
            diceRb.useGravity = true;
            bumpTimer = 0;
        }
        else if(isRoll)
        {
            bumpTimer += Time.deltaTime;
            if(bumpTimer > timeToBump)
            {
                Bump();
                bumpTimer = 0;
            }
        }
    }

    public void RestartDicePosition()
    {
        SetDiceToDefaultPosition();
        addPoints = true;
    }
    
    private IEnumerator RestartDicePositionAfterFall()
    {
        SetDiceToDefaultPosition();
        
        restartText.SetActive(true);
        yield return waitForSeconds;
        restartText.SetActive(false);
        yield return waitForSeconds;
    }

    private void SetDiceToDefaultPosition()
    {
        isRestarted = false;
        diceRb.useGravity = false;
        diceTransform.position = diceStartPosition;
        diceTransform.rotation = diceRotationZero;
        diceRb.velocity = Vector3.zero;
        button.interactable = true;
        isRoll = false;
        dragAndRoll.isThrown = false;
    }

    private void Bump()
    {
        force = new Vector3(Random.Range(bumpMinRandomValue, bumpMaxRandomValue),
            Random.Range(bumpMinRandomValue, bumpMaxRandomValue), Random.Range(bumpMinRandomValue, bumpMaxRandomValue));
        diceRb.AddForce(force * bumpForceMultiplier, ForceMode.Impulse);
    }

    private bool isDiceLayingOnDesk()
    {
        return diceTransform.position.y <= dicePositionDesk && diceRb.velocity.y == 0f;
    }

    private bool IsDiceOffDesk()
    {
        return diceTransform.position.y < dicePositionOffDesk;
    }
}
