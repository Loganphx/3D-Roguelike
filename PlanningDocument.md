# Roguelike Planning Document

Core Mechanics:
- Trees (choppable, drop wood on death)
  - Drop Varying type of wood depending on tree type
- Ore Veins/Nodes (minable, drop ore on death)
  - Drop Varying type of ore on death
- 4 Tiers of Chests with random power ups (Common, Rare, Epic, Legendary)

- Powerups(Permanent Buffs)
- Totems/Pillars that when interacted with spawn enemies, but when killing all enemies a random powerup is dropped.
- Day/Night System
- Spawn Enemies at night
- As Days Progress enemies scale in difficulty.
- Skills(Crafting, Smelting, Mining, Woodcutting/Carpentry)

AI Concepts/Behaviour:
- Halt Movement when attacking
- AI actively avoids the player to prevent from clipping into the player.
- Ranged Attacks have a max range but not a minimum
- Swarm Behaviour(Mobs surround the player)
- Avoid Projectiles (Maybe?)
- Supporting Varying Abilities and Ranged Attacks

- Network-Related Features/Concepts:
- State Management (basically have the last x amount of ticks worth of data for the entire game) ex: GameState[50]
- Maybe hitbox bounds eventually, but for now just trust the client's input
- Area of Interest (would be easy with chunks, but probably is best to do a distance check to each player before sending that data to them)
  - To do proper area of interest this would also require some robust networking solution that builds different packets for each player or at least deliniate packets for each player across chunks.
  - This would also require doing some of sort of queuing of changes (my idea is to enqueue dirty entities then if they are within range of a player we figure out what is dirty ideally using some sort of dirty flag and then use that to figure out what data needs to be emmited, and ideally then cache that data we emitted to reuse it for other players.

Planned Concepts:
- Use Prefab Composition to easily support creating prefab dynamically without using unity (Future).
- Use Components with interfaces to make networking easy, or use an Entity concept (with an entity maintaining an array of their components) and then letting the entity
- Use Dirty Flags for state management
- Have All State be independant of the components to support roll backs and copying of this state to the GameState buffer.
- Chunking(kinda a pain in the ass, but probably will require having a Main pool that is independant of the chunks, then have that main pool manage Chunk Pools(which just manages the chunk data for that chunk)
