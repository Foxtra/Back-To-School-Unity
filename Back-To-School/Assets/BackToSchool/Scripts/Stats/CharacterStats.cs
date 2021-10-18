namespace Assets.BackToSchool.Scripts.Stats
{
    public class CharacterStats
    {
        public Stat Damage;
        public Stat MaxHealth;
        public Stat MoveSpeed;

        public CharacterStats(int damage, int maxHealth, int moveSpeed)
        {
            Damage    = new Stat(damage);
            MaxHealth = new Stat(maxHealth);
            MoveSpeed = new Stat(moveSpeed);
        }

        public CharacterStats() : this(1, 10, 5) { }
    }
}