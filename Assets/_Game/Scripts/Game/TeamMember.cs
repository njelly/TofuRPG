namespace Tofunaut.TofuRPG.Game
{
    public class TeamMember : ActorComponent
    {
        public int Team { get; private set; }
        
        public override void Initialize(Actor actor, ActorModel model)
        {
            Team = model.Team;
        }

    }
}