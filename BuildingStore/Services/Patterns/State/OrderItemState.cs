namespace BuildingStore.Services.Patterns.State
{
    public abstract class OrderItemState
    {
        protected OrderItemContext context;

        public void SetContext(OrderItemContext context)
        {
            this.context = context;
        }
        public abstract bool Cancel();
        public abstract bool Pay();
        public abstract bool Complete();
    }
}
