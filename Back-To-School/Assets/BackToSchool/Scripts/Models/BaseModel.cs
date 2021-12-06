using Assets.BackToSchool.Scripts.Interfaces.Core;


namespace Assets.BackToSchool.Scripts.Models
{
    public abstract class BaseModel : IModel
    {
        public virtual void Dispose() { }
    }
}