
public static class Players {
    private static int[] position;
    private static int[] direction;

    public static void SetPlayersInitialPosition()
    {
        position = new int[4];
        direction = new int[4];

        for (int numPlayer = 0; numPlayer < 4; numPlayer++)
        {
            position[numPlayer] = 0;
            direction[numPlayer] = 1;
        }
    }

    public static void SetCurrentSlot(int numPlayer, int currentSlot)
    {
        position[numPlayer] = currentSlot;
    }

    public static void SetCurrentDirection(int numPlayer, int currentDirection)
    {
        direction[numPlayer] = currentDirection;
    }

    public static int GetCurrentSlot(int numPlayer)
    {
        return position[numPlayer];
    }

    public static int GetCurrentDirection(int numPlayer)
    {
        return direction[numPlayer];
    }

    public static int GetNextSlot(int numPlayer)
    {
        return Map.GetNextSlot(position[numPlayer], direction[numPlayer]);
    }
}
