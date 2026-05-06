using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public class OrderItemContext
    {
        public OrderItem Item { get; private set; }
        private OrderItemState state;

        public OrderItemContext(OrderItem item)
        {
            Item = item;
            InitializeState(item.ProductStatus);
        }
        public void TransitionTo(OrderItemState state, ProductStatus newDbStatus)
        {
            this.state = state;
            this.state.SetContext(this);
            Item.ProductStatus = newDbStatus; 
        }
        public bool Cancel() => state.Cancel();
        public bool Pay() => state.Pay();
        public bool Complete() => state.Complete();
        private void InitializeState(ProductStatus status)
        {
            switch (status)
            {
                case ProductStatus.Created:
                    TransitionTo(new CreatedState(), ProductStatus.Created); 
                    break;
                case ProductStatus.Processing:
                    TransitionTo(new ProcessingState(), ProductStatus.Processing); 
                    break;
                case ProductStatus.Completed:
                    TransitionTo(new CompletedState(), ProductStatus.Completed); 
                    break;
            }
        }
    }
}
