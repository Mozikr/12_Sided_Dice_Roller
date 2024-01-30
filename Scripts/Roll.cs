using UnityEngine;
using UnityEngine.UI;

public class Roll : MonoBehaviour
{
    private const float forceMultiplier = 10f;
    
    private Rigidbody diceRb;
    private Vector3 force;
    private GameController controller;
    private DragAndRoll dragAndRoll;

    [SerializeField] private GameObject dice;
    [SerializeField] private Button button;
    [SerializeField] private int rollMinRandomValue = 15;
    [SerializeField] private int rollMaxRandomValue = 30;
    
    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        diceRb = dice.GetComponent<Rigidbody>();
        dragAndRoll = FindObjectOfType<DragAndRoll>();
    }
    public void RollDice()
    {
        if (!dragAndRoll.isThrown)
        {
            controller.isRoll = true;
            dragAndRoll.isThrown = true;
            force = new Vector3(Random.Range(rollMinRandomValue, rollMaxRandomValue),
                Random.Range(rollMinRandomValue, rollMaxRandomValue),
                Random.Range(rollMinRandomValue, rollMaxRandomValue));
            diceRb.useGravity = true;
            diceRb.AddForce(force * forceMultiplier, ForceMode.Impulse);
            button.interactable = false;
        }
    }
}
