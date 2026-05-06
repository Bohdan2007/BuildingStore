using BuildingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.Patterns.State
{
    public class CreatedState : OrderItemState
    {
        public override bool Cancel()
        {
            return true;
        }
        public override bool Pay()
        {
            context.TransitionTo(new ProcessingState(), ProductStatus.Processing);
            return true;
        }
        public override bool Complete()
        {
            return false;
        }
    }
}
