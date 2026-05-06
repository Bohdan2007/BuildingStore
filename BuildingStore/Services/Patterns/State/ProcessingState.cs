using BuildingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.Patterns.State
{
    public class ProcessingState : OrderItemState
    {
        public override bool Cancel()
        {
            return false;
        }
        public override bool Pay()
        {
            return false;
        }
        public override bool Complete()
        {
            context.TransitionTo(new CompletedState(), ProductStatus.Completed);
            return true;
        }
    }
}
