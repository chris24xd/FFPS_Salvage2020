using UnityEngine;

public class Animatronic : MonoBehaviour
{
    public Animatronic_AI data;

    protected int AI => data.ai;
    public int points;

    public virtual bool CanMove(int max = 20) => Random.Range(1, max + 1) <= AI;
    public virtual bool CanJumpscare() => false;
    public virtual void Jumpscare() {}

    private void Awake()
    {
        points = 10 * AI;
    }
}

public interface ITaseable
{
    public void Tase();
}
