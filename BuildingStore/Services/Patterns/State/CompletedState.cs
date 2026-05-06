using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public class CompletedState : OrderItemState
    {
        public override bool Cancel() => false; 
        public override bool Pay() => false; 
        public override bool Complete() => false;
    }
}
