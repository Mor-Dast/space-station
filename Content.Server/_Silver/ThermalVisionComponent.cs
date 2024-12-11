using Robust.Shared.GameObjects;
using Robust.Shared.ViewVariables;

namespace Content.Server._Silver.ThermalVision
{
    [RegisterComponent]
    public class ThermalVisionComponent : Component
    {
        public override string Name => "ThermalVision";

        [ViewVariables(VVAccess.ReadWrite)]
        public float VisionRadius { get; set; } = 10.0f;

        protected override void OnAdd()
        {
            base.OnAdd();
            EntitySystem.Get<WallVisionSystem>().Register(this);
        }

        protected override void OnRemove()
        {
            base.OnRemove();
            EntitySystem.Get<WallVisionSystem>().Unregister(this);
        }
    }
}