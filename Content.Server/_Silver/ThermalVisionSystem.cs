using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Server.GameObjects;
using Robust.Shared.IoC;
using System.Collections.Generic;

namespace Content.Server.Systems
{
    public class WallVisionSystem : EntitySystem
    {
        [Dependency] private readonly IMapManager _mapManager = default!;

        private readonly HashSet<WallVisionComponent> _wallVisionComponents = new();

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<WallVisionComponent, PlayerAttachSystemMessage>(OnPlayerAttached);
        }

        public void Register(WallVisionComponent component)
        {
            _wallVisionComponents.Add(component);
        }

        public void Unregister(WallVisionComponent component)
        {
            _wallVisionComponents.Remove(component);
        }

        private void OnPlayerAttached(EntityUid uid, WallVisionComponent component, PlayerAttachSystemMessage args)
        {
            UpdateVision(component, args.PlayerSession.AttachedEntity);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            foreach (var component in _wallVisionComponents)
            {
                if (component.Owner.TryGetComponent(out IMapGridComponent? grid) &&
                    component.Owner.TryGetComponent(out TransformComponent? transform))
                {
                    UpdateVision(component, component.Owner);
                }
            }
        }

        private void UpdateVision(WallVisionComponent component, IEntity owner)
        {
            var mapGrid = _mapManager.GetGrid(owner.Transform.GridID);
            var worldPosition = owner.Transform.WorldPosition;

            foreach (var entity in mapGrid.GetEntitiesInRange(owner.Transform.Coordinates, component.VisionRadius))
            {
                if (entity.HasComponent<MobComponent>()) // Assuming MobComponent denotes a living entity
                {
                    entity.Visible = true;
                }
            }
        }
    }
}