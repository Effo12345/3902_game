namespace ECS.Events;

public class CoinPickupEvent : IEvent
{
    public int CoinValue = CONSTANTS.SCORE.COIN_VALUE;
    public int CoinCount = CONSTANTS.SCORE.COIN_COUNT;
}