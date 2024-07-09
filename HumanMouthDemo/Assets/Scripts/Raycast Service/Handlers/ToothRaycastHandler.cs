using ATG.Views;

namespace ATG.Raycasting
{
    public sealed class ToothRaycastHandler : IRaycastHandler
    {
        private readonly IRaycastService _raycastService;
        private readonly MouthView _mouth;

        public ToothRaycastHandler(IRaycastService raycastService, MouthView mouth)
        {
            _raycastService = raycastService;
            _mouth = mouth;
        }

        public void Update()
        {
            _mouth.SelectTooth(_raycastService.Raycast()?[0] ?? null);
        }
    }
}