package foundation.softwaredesign.orbi.model.game.character;

import static java.util.Objects.isNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public enum CharacterLevel {
    LEVEL_000(0,-1),
    LEVEL_001(1,0),
    LEVEL_002(2,128),
    LEVEL_003(3,1337),
    LEVEL_004(4,10101),
    LEVEL_005(5,232323),
    LEVEL_006(6,1234321),
    LEVEL_007(7,11111111),
    // TODO make it reachable ;)
    LEVEL_008(8,100000000),
    LEVEL_009(9,1000000000),
    LEVEL_010(10,1000000000),
    LEVEL_011(11,10000000000l),
    LEVEL_012(12,100000000000l),
    LEVEL_013(13,1000000000000l),
    LEVEL_014(14,10000000000000l),
    LEVEL_015(15,100000000000000l),
    LEVEL_016(16,1000000000000000l),
    LEVEL_017(17,10000000000000000l),

    ;

    private int level = 0;
    private long minXp = 0l;

    public int level() {
        return level;
    }

    public long minXp() {
        return minXp;
    }

    CharacterLevel(int level, long minXp) {
        this.level = level;
        this.minXp = minXp;
    }

    public static CharacterLevel getLevel(Long xp) {
        if (isNull(xp))
            return LEVEL_001;
        for (CharacterLevel level : CharacterLevel.values()) {
            CharacterLevel nextlevel = getNextLevel(level);
            if ((xp >=  level.minXp) && (xp < nextlevel.minXp))
                return level;
        }
        return CharacterLevel.LEVEL_000;
    }

    public static CharacterLevel getNextLevel(CharacterLevel level) {
        for (CharacterLevel nextLevel : CharacterLevel.values()) {
            if (nextLevel.level >  level.level)
                return nextLevel;
        }
        return level;
    }
}
