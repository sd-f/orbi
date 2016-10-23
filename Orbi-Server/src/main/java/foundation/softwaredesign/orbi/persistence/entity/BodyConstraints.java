package foundation.softwaredesign.orbi.persistence.entity;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public enum BodyConstraints {
    NONE(0),
    FREEZE_POSITION_X(2),
    FREEZE_POSITION_Y(4),
    FREEZE_POSITION_Z(8),
    FREEZE_POSITION(14),
    FREEZE_ROTATION_X(16),
    FREEZE_ROTATION_Y(32),
    FREEZE_ROTATION_Z(64),
    FREEZE_ROTATION(112),
    FREEZE_ALL(126);

    private int contraints;

    public int value() {
        return contraints;
    }

    BodyConstraints(int contraints) {
        this.contraints = contraints;
    }
}
