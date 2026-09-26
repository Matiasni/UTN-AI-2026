using System;

public interface IDestinationNotifier
{
    event Action OnDestinationReached;
}
