using UnityEngine;

public class GameConstants
{
    
    public struct LEVEL_EVENTS
    {
       
        public static string OBJECTIVE_FAILED = "OBJECTIVE_FAILED";
        public static string LEVEL_FAILED = "LEVEL_FAILED";
        public static string LEVEL_SUCCESSED = "LEVEL_SUCCESSED";
        public static string LEVEL_STARTED = "LEVEL_STARTED";
        
        public static string LevelFinished = "LevelFinished";

        public static string REACHED_FINISH = "REACHED_FINISH";
    }

   
    public struct GameEvents
    {
       
        public static string GAME_STARTED = "GAME_STARTED";
        public static string PLAYER_ATTACK_STARTED = "PLAYER_ATTACK_STARTED";
        public static string PLAYER_ATTACK_ENDED = "PLAYER_ATTACK_ENDED";
        public static string PLAYER_IS_DEAD = "PLAYER_IS_DEAD";
        public static string HITTED_TO_OBSTACLE = "HITTED_TO_OBSTACLE";
        public static string PICKUP_HEALTH = "PICKUP_HEALTH";
        public static string PICKUP_AMMO = "PICKUP_AMMO";
        public static string PLAYER_IS_SHOOTED = "PLAYER_IS_SHOOTED";


    }


}