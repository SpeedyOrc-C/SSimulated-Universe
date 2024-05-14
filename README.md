ßimulated Universe
==================

This is a simulator for simulated universe (battles) in video game Honkai: Star Rail.

DISCLAIMER
----------

Since all the code in this repository is not obtained by any form of reverse engineering to this game, this program's behaviour is for reference only and may differ from the actual game.

Why you can't undo an ultimate?
-------------------------------

This is because all the entities (visible or invisible) and buffs are allowed to listen a bunch of events, and this includes unleashing the ultimate. When the specific event is triggered, it's broadcast to all the concerned listeners, and they may do whatever they want. Which means, unleashing an ultimate not only reduces your energy, but also creates a bunch of side effects that makes undoing an overhead. Whole battle's state must be saved before any ultimate if undoing is allowed.
