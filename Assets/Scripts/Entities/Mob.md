# Mob Design Plan

We will use the standard navmesh agents (How will it react to the player being midair?)

When casting/winding up an attack the mob should halt all movement 
    - We will use LOS to start the attack, but will ignore LOS once started 
    
Target Builds if they are closer than the player (i.e they are in the way or the player is on them)
Always Targets the closest player
     
Game plan:
    - MobData class to store the configuration data (canAttackAndMove, followDistance, abilities, ignoreBuildings, etc)
    - Find Nearest Player
    - Path to Nearest Player (Animations Here Eventually)
    - When in range start attack/cast/windup (Play Attack Animation)
    