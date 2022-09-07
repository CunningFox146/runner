//!!!!!! MAKE SURE TO KEEP THIS ENUM UP-TO-DATE WITH PLAYER ANIMATOR

namespace Runner.Player
{
    [System.Serializable]
    public enum PlayerState
    {
        Sitting = -1,
        Running,
        Jump,
        Falling,
        Slide,
        Death,
        SideChange,

        /* SHOP STATES GO HERE */
        DefaultShop,
        GirlShop,
        ZombieShop,
    }
}
