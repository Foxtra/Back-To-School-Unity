namespace Assets.BackToSchool.Scripts.Stats
{
    public class CharacterStats
    {
        public Stat Armor;
        public Stat Damage;
        public Stat FireRate;
        public Stat MaxAmmo;
        public Stat MaxHealth;
        public Stat MoveSpeed;
        public Stat ReloadSpeed;

        public CharacterStats(int armor, int damage, int fireRate, int maxAmmo, int maxHealth, int moveSpeed, int reloadSpeed)
        {
            Armor       = new Stat(armor);
            Damage      = new Stat(damage);
            FireRate    = new Stat(fireRate);
            MaxAmmo     = new Stat(maxAmmo);
            MaxHealth   = new Stat(maxHealth);
            MoveSpeed   = new Stat(moveSpeed);
            ReloadSpeed = new Stat(reloadSpeed);
        }

        public CharacterStats() : this(0, 1, 1, 10, 5, 5, 1) { }

        //public virtual void TakeDamage(int damage)
        //{
        //    damage -= Armor.GetValue();
        //    damage =  Mathf.Clamp(damage, 0, int.MaxValue);

        //    currentHealth -= damage;
        //    Debug.Log(transform.name + " takes " + damage + " damage.");

        //    if (currentHealth <= 0) Die();
        //}
    }
}