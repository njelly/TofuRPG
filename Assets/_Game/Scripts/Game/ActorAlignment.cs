namespace Tofunaut.TofuRPG.Game
{
    public class ActorAlignment : ActorComponent
    {
        public enum EAlignment
        {
            Neutral,
            Player,
            NeutralWild,
            HostileWild,
            HostileEnemy,
        }
        
        public EAlignment Alignment { get; private set; }
        
        public override void Initialize(Actor actor, ActorModel model)
        {
            Alignment = model.Alignment;
        }

    }
}