using GameAnalyticsSDK;

public static class GAManager
{
    static GAManager()
    {
        GameAnalytics.Initialize();
    }
    public static void OnLevelComplete(string level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, level);
    }

    public static void OnMoneyGain(bool isGold, int amount, string itemType, string itemId)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, isGold ? "Gold" : "Cash", amount, itemType, itemId);
    }    
    
    public static void OnMoneySpent(bool isGold, int amount, string itemType, string itemId)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, isGold ? "Gold" : "Cash", amount, itemType, itemId);
    }
}
