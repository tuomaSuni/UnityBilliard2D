using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    private GameObject currentRock;
    [SerializeField] private BoxCollider2D limit;
    [SerializeField] public List<Rigidbody2D> listOfBalls = new List<Rigidbody2D>();
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Transform set;
    public bool HasGameEnded = false;
    public bool HasGameWon;

    private void Start()
    {
        currentRock = Instantiate(rock, new Vector3(-5, 0, 0), Quaternion.identity);
        currentRock.GetComponent<rockLogic>().sm = this;
    }

    private void Update()
    {
        if (currentRock == null && !HasGameEnded)
        {
            currentRock = Instantiate(rock);
            currentRock.GetComponent<rockLogic>().sm = this;
        }
        if (currentRock == null == false && currentRock.GetComponent<CircleCollider2D>().isTrigger == false && limit.enabled == true) limit.enabled = false;

        if (set.childCount == 0)
        {
            EndGame(true);
        }
    }

    public bool AllBallsHasStopped()
    {
        foreach (Rigidbody2D rb in listOfBalls)
        {
            if (rb.velocity.magnitude > 0.02f)
            {
                return false;
            }
        }
        return true;
    }
    public void EndGame(bool won)
    {
        HasGameEnded = true;
        HasGameWon = won;
        Destroy(currentRock);
        gamePanel.SetActive(true);
    }
}