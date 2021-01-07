
public class ComboStage
{
    public int stage;
    public float delayToStartTrail;
    public float delayToStopTrail;
    public float delayToFireSpell;

    public ComboStage(int stage, float delayToStartTrail, float delayToStopTrail, float delayToFireSpell)
    {
        this.stage = stage;
        this.delayToStartTrail = delayToStartTrail;
        this.delayToStopTrail = delayToStopTrail;
        this.delayToFireSpell = delayToFireSpell;
    }
}
