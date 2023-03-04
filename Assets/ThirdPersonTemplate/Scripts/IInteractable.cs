namespace ThirdPersonTemplate
{
    public interface IInteractable
    {
        public abstract void OnBeginOverlap(Player player);
        public abstract void OnInteract(Player player);
        public abstract void OnEndOverlap(Player player);
    }
}