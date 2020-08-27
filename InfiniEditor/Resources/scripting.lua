----------------------------------------------------------
-- This is a tutorial for scripting in this program     --
-- The scripting language is Lua                        --
-- a Lua tutorial: http://tylerneylon.com/a/learn-lua/  --
----------------------------------------------------------

-- Main two modes in which you will usually operate are --
-- 1. accessing blocks by their coordinates             --
-- 2. going trough all of the so far placed blocks      --
-- Below are some examples of these and a doc.          --
----------------------------------------------------------


----------------------------------------------------------
-- CREATE A CIRCLE WITH A RADIUS OF 12 OUT OF PLATFORMS
----------------------------------------------------------
for x = - 12, 12 do
    for y = -12, 12 do
        if x*x + y*y < 12*12 then
            Puzzle[{x, y, 0}] = Block(18)
        end
    end
end

------------------------------------------------------------
-- PUT DECALS ON TOP OF ALL BLOCKS, THAT HAVE AIR ABOVE THEM
------------------------------------------------------------
for p, b in Blocks(Puzzle) do
    if Puzzle[{p.X, p.Y+1, p.Z}] == nil then
        b[PosY] = 1
    end
end

------------------------------------------------------------------
-- Printing a set of all types of blocks that are in the clipboard
------------------------------------------------------------------
types = {}
for _, b in Blocks(Puzzle) do
    types[b.Type] = true
end
for k, _ in pairs(types) do
    print(k)
end

----------------------------------
-- Remove all decals in the puzzle
----------------------------------
for _, b in Blocks(Puzzle) do
    for _, d in ipairs(Directions) do
        b[d] = nil
    end
end

-----------------------------------------------------------
-- Copy all blocks in the puzzle that have at least
-- one air next to it to the clipboard
-----------------------------------------------------------
for p, b in Blocks(Puzzle) do
    count = 0
    for _, d in ipairs(Directions) do
        if Puzzle[p+Vec(d)] ~= null then
            count = count + 1
        end
    end
    if count < 6 then
        Clipboard[p] = Block(b)
    end
end

-- Print an ID, name and Group of every blocktype with a function
for id, info in pairs(BlockInfos("functional")) do
    print(id .. " " .. info.Name .. " - " .. info.Group)
end

-- Copy all blocks but stairs and windows from clipboard to puzzle
for p, b in Blocks(Clipboard) do
    if BlockInfo(b.Type):FlagsCondition("!stairs, !window") then
        Puzzle[p] = Block(b)
    end
end


------------------------------------------------------------
-- Direction                                              --
------------------------------------------------------------
--> Directions are represented by the constants:          --
--    PosX, NegX, PosY, NegY, PosZ, NegZ                  --
--> Directions                list of these directions    --
--> BlockDirections            {PosX, NegX, PosZ, NegZ}   --
------------------------------------------------------------

------------------------------------------------------------
-- Vec (3D vector)                                        --
------------------------------------------------------------
-- When iterating trough blocks the position is this type --
-- You can also use this to get a block at that position  --
------------------------------------------------------------
-- Vec(1,2,3)                    a vector [1,2,3]         --
-- Vec(NegY)                    a vector [0, -1, 0]       --
-- Vec(5,0,1)-2*Vec(3,-2,1)        a vector [-1, 4, -1]   --
-- p.X = p.Y + p.Z                accessing coordinates   --
------------------------------------------------------------

--------------------------------------------------------------------------
-- Puzzle and Clipboard                                                 --
--------------------------------------------------------------------------
--> Blocks(Puzzle)          iterator over the pair-position, block      --
--> Puzzle[pos]             block at specified location or nil if there --
--                          isn't a block, assigning to nil deletes it  --
--                          pos can be Lua array or a Vec object        --
--------------------------------------------------------------------------

--------------------------------------------------------------------------
-- Block                                                                --
--------------------------------------------------------------------------
--> b = Block(type)          A new block of specified type              --
--> b = Block(type, facing, role, group)                                --
--                           ..you can also use other paramters         --
--> b2 = Block(b1)           Create a clone of a Block b1               --
--> b.Role                   Role; whether this is world/input/output   --
--> b.Group                  int; id of a group of inputs or outputs    --
--> b.Type                   int; type of the block                     --
--> b.Facing                 Direction; where the block is facing       --
--> b[dir]                   int; decal in the specified direction, nil --
--                           if nothing; assigning to nil deletes it    --
--> b.State                  int; state of the block (i.e. counter)     --
--> Decals(b)                iterator over the pair-direction, type     --
--------------------------------------------------------------------------

--------------------------------------------------------------------------
-- Flag conditions                                                      --
--------------------------------------------------------------------------
-- Flag condition is a semicolon separated list of subconditions, the   --
-- result is true iff any of the subconditions is true.                 --
-- The subconditon is a comma separated list of flags. The subconditon  --
-- is true iff the block has all the flags that don't start with "!" and--
-- none of those that start with "!"                                    --
-- You can browse flags and test conditions in Block reference.         --
--------------------------------------------------------------------------

--------------------------------------------------------------------------
-- BlockInfo                                                            --
--------------------------------------------------------------------------
--> bi = BlockInfo(id)       Gets info about a block with specied id    --
--> DecalInfo(id)            Gets info about a decal with specied id    --
--> BlockInfos(cond)         Gets a table of infos satisfying condition --
--  BlockInfos()             cond (all if not provided), indexed by ids --
--> DecalInfos               same                                       --
--> bi.Type                  int; type                                  --
--> bi.Name                  string; name                               --
--> bi.Group                 string; name of the group in editor        --
--> bi.Decal                 bool; decal or block                       --
--> bi:FlagsCondition(cond)  bool; does the block satisfy the condition --
--> bi.Symmetries            TODO                                       --
--> bi.BoundingBox           TODO                                       --
--------------------------------------------------------------------------