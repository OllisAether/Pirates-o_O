Arrrr, shipmates!

Your final semester project in this course is to create a prototype of a 3D pirate game in Unity based on the POLYGON Pirate Pack (and if you like, adding assets from other packs, like, e.g., the Tropical Jungle Biome), combining all the elements of pathfinding, FSM, observer pattern, object pooling, gameplay systems, ScriptableObjects, juice, cinematics, and visual quality.

It is **mandatory** to have all these elements in the game:

- **Pathfinding** (e.g., for enemies finding the way around obstacles, but this can also be for animals, etc.)
- **FSM** (e.g., for some simple enemy AI, player state, Game-Manager, etc.)
- ✅ **Observer Pattern** for exchanging high-level game events, for minimal dependencies between behaviors.
- **Performant** handling and **spawning of objects** (Pooling, ECS, etc.). In a shooter, this should, e.g., be done for bullets, but generally for every object type that is spawned frequently. Don't do this for occasionally spawned stuff as it wastes lots of memory.
- Using the *Feel Asset* wisely for some juice in the right moments when the player wants that extra bit of (positive or negative) satisfaction.
- Usage of **ScriptableObjects** in one of the possible ways explained during the course. For example, representing abilities, stats, items, etc.
- At least one high-level gameplay system beyond an inventory system (e.g. dialogue, quests, perception, combat, etc.
- ✅ **Interaction-system** for interacting with potentially any object.
- ✅ Usage of a **cinematic camera(s)** with **Cinemachine** and cinematic animation juice with **Timeline** (for example, things like cinematic feel when you (or someone else) enter a bar and everybody goes quiet and looks at the new guy, or when you shoot someone off a building, you cut to some extra nice slow-motion cam shot ... stuff like that).
- **Visual quality** beyond the standard look of the demo scenes. Using the Unity URP and post-processing and/or one of our awesome shaders/FX packs from our Asset Library.

Bonus points are:

- 2 (or more) Player network with NetCode for GameObjects. This can be either coop (shoot-first-and-think-later together) or PvP. Just don't be boring. 
- Ability system with flexibly definable abilities and effects.
- Extra screens, completing the game experience, like menu, mission failed, stats, etc.
- Using the command pattern (not explicitly covered, but can be checked here).
- ✅ Full cutscenes with Timeline/Cinemachine.
- Horse Riding Action! (I mean, pirates on horses. Does it get any better?)
Check out the awesome Horse AnimSet Pro and the Polygon Horse pack in our Asset Library. 
- Animals (we've got an Animated Animals pack for that, fitting the low-poly style... crocodiles and sharks anyone?)

For the scope of the game, it should be a well-rounded and complete small experience, with a clear goal for the players and a clear obstacle in their way, hindering players from reaching that goal.  The game "mechanics" should fit the pirate theme, and can of course include throwing, kicking, vomiting, getting drunk, looting, betraying, etc.

For the theme and rules of the game, of course, you could just do the standard shooter where you kill NPCs or the other player. However, I'd like to remind you of some of the cool clichés available in pirate scenarios: rum, ships, walk the plank, prostitutes, card cheating, wooden legs and hooks, skeleton pirates, treasures and maps, beards, vomiting, parrots, arrrr ... more rum! ... now combine them for some awesome (or crappy?) gameplay. Drunk pirates falling off ships? Fierce quest for the final drops of rum in town? I'm confident you'll find something unconventional and interesting. As long as the aforementioned criteria are met, that's ok.

You'd really benefit from this course if you learn how to make a game interesting, not just some game, so please make it a little experience on its own.

For your visual theme, you should mainly use the assets and props in the POLYGON pirate pack, but of course, you can modify them or use assets from other bundles. Is the world ready for cowboys in a pirate town, or aliens throwing animals at ships? Do we even want that? O_o

For all features, you can use any of the code used in the lessons or on the web/tutorials, etc., and adapt/integrate it into your game.

Your project will be graded along several criteria, like e.g.:

Code/Technology (highest weight in this course)
Design/Juice! (game design, visuals, game feel)
Playability (include a README.txt with instructions, for god's sake).
Idea/Features
Complexity/Extent
And we will check every single mandatory criterion, so please don't submit anything without FSM, Observer, etc.

Please submit a Download Link to a ZIP file (or a git repository) containing a README.txt file and only the usual Unity folders "Assets", "ProjectSettings", and "Packages".

IMPORTANT: provide a README file in the main project folder to explain, containing at least

Prerequisites/requirements for installing/running the game
Short background/setting and objective of the game
Controls (keys or controllers)
Description of how every mandatory requirement is met (e.g., where FSM was used, how the observer was implemented,...)
I don't want to dig through code for hours to understand which keys to press or where you used a state machine pattern. The game should have no external dependencies (except if explicitly documented), so the usual 3 Unity folders should contain everything necessary to run the game on a fresh PC with Unity 6.4.x.

Happy coding, and don't forget to have fun doing this!