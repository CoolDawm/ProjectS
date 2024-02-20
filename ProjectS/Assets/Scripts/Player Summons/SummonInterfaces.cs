
public interface ISummoner
{
    float detectionRadius { get; }
    float aggroRadius { get; }
    void Attack();
    void Die();
}

public interface ISummon
{
    void ChasePlayer();
    void Idle();
}
